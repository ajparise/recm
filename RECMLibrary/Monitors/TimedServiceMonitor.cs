using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Data.Linq;
using Parise.RaisersEdge.ConnectionMonitor.Data.Entities;

namespace Parise.RaisersEdge.ConnectionMonitor.Monitors
{
    /// <summary>
    /// Wraps monitor functionality with a timer for use in a service
    /// </summary>
    public class TimedREConnectionMonitor : REConnectionMonitor
    {
        private System.Diagnostics.EventLog _log;
        private System.Timers.Timer _monitor;
        //private System.Timers.Timer _heartBeat;
        private bool loggingEnabled;

        /// <summary>
        /// Constructor
        /// </summary>
        public TimedREConnectionMonitor(System.Diagnostics.EventLog AppEventLog, bool useAppConfig) : base(useAppConfig)
        {            
            this._log = AppEventLog;
            loggingEnabled = this._log != null;

            base.CleanedUpDeadLocks += new OnCleanedUpDeadLocks(TimedREConnectionMonitor_CleanedUpDeadLocks);
            base.CleaningUpDeadLocks += new DatabaseEvent(TimedREConnectionMonitor_CleaningUpDeadLocks);
            base.ConnectedToDatabase += new DatabaseEvent(TimedREConnectionMonitor_ConnectedToDatabase);
            base.ConnectingToDatabase += new OnConnectingToDatabase(TimedREConnectionMonitor_ConnectingToDatabase);
            base.FreedConnection += new FreedConnectionEvent(TimedREConnectionMonitor_FreedConnection);
            base.FreeingConnection += new FreeingConnectionEvent(TimedREConnectionMonitor_FreeingConnection);
            base.StatusMessage += new OnStatusMessage(TimedREConnectionMonitor_StatusMessage);
            base.GenericError += new ErrorEvent(TimedREConnectionMonitor_GenericError);
            base.FreeingLock += new FreeingLockEvent(TimedREConnectionMonitor_FreeingLock);
            base.SkippedFreeingConnection += new SkippedFreeingConnectionEvent(TimedREConnectionMonitor_SkippedFreeingConnection);

            // Start the monitor
            this._monitor = new System.Timers.Timer();
            //this._timer.Interval = 1000;            
            this._monitor.Interval = int.Parse(Settings[MonitorSettings.PollingInterval]);
            this._monitor.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
            this._monitor.Enabled = false;

            // Start the heartbeat
            //this._heartBeat = new System.Timers.Timer();
            //this._heartBeat.Interval = 60000;
            //this._heartBeat.Elapsed += new System.Timers.ElapsedEventHandler(_heartBeat_Elapsed);
            //this._heartBeat.Enabled = false;
        }

        /// <summary>
        /// Start the timer.
        /// </summary>
        public void StartMonitor()
        {
            this._monitor.Enabled = true;
            //this._heartBeat.Enabled = true;
        }

        /// <summary>
        /// Stop the timer.
        /// </summary>
        public void StopMonitor()
        {
            this._monitor.Enabled = false;
            //this._heartBeat.Enabled = false;
        }

        StringBuilder logBuffer = new StringBuilder();


        void TimedREConnectionMonitor_GenericError(ErrorEventArgs<Exception> e)
        {
            Log(e.Error);
        }

        void TimedREConnectionMonitor_SkippedFreeingConnection(string reasonSkipped, FreeingEventArgs e)
        {
            logBuffer.AppendFormat("Skipped - {0}\n", reasonSkipped);
        }

        void TimedREConnectionMonitor_StatusMessage(string message)
        {
            Log("Status Message\n" + message, EventLogEntryType.Information);
        }

        void TimedREConnectionMonitor_FreeingLock(FilteredLockConnection connection)
        {
            logBuffer.AppendFormat("\nFreeing connections from lock {0} -- {1} -- {2}\n", connection.Lock.MachineName, connection.Lock.User.Name, connection.REProcess.hostname);
        }

        void TimedREConnectionMonitor_FreeingConnection(FreeingEventArgs e)
        {
        }

        void TimedREConnectionMonitor_FreedConnection(FreedEventArgs e)
        {            
            logBuffer.AppendFormat("\tIssued kill for:\n\t\tSPID: {0}\n\t\tProgram Name: {1}\n\t\tStatus: {2}\n\t\tIdle Time: {3}\n",
                e.Process.spid, 
                e.Process.program_name.Trim(),
                e.Process.status.Trim(),
                e.Process.IdleTimeFormatted("{h:D2} hour(s), {m:D2} minute(s), {s:D2} second(s), {ms:D2} millisecond(s)"));
        }

        void TimedREConnectionMonitor_ConnectingToDatabase(ref string connectionString)
        {
            logBuffer.AppendFormat("Database Connection\n-----------------------\n\tConnecting with: {0}\n", connectionString);
        }

        void TimedREConnectionMonitor_ConnectedToDatabase(Parise.RaisersEdge.ConnectionMonitor.Data.RecmDataContext db)
        {
            logBuffer.AppendFormat("\tSuccessfully connected to database {0}/\n", db.Connection.Database);
        }

        void TimedREConnectionMonitor_CleaningUpDeadLocks(Parise.RaisersEdge.ConnectionMonitor.Data.RecmDataContext db)
        {
            logBuffer.AppendFormat("\nDead Locks\n-----------------------\n\tCleaning up dead locks for {0}\n", db.Connection.Database);
        }

        void TimedREConnectionMonitor_CleanedUpDeadLocks(int result)
        {
            logBuffer.AppendFormat("\tCleaned up dead locks with result {0}.\n", result);
        }

        /*
         * Respond to the _Timer elapsed event.
         */
        public void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var freed = this.FreeConnections();

            var fullLog = logBuffer.ToString();
            logBuffer = new StringBuilder(); // Clear the log buffer
            
            Log(fullLog, EventLogEntryType.Information);

            // Send mail if we freed a connection and email notifications are turned on
            if (freed.Count() > 0 && Settings[MonitorSettings.EmailNotifications].ToLower().Equals("true"))
            {
                var freeMessages = freed.Select(f => new { 
                    Msg = string.Format("You have been automatically disconnected from Raisers Edge after being idle for {0}.\n\nPlease restart Raiser's Edge to re-connect.\n\nThis is an automated message, please do not respond.", f.AllProcesses.First().IdleTimeFormatted("{h:D2} hour(s), {m:D2} minute(s), {s:D2} second(s), {ms:D2} millisecond(s)")), 
                    Subject = "Message for " + f.Lock.User.Name + " from Raiser's Edge Connection Monitor",
                    Email = f.Lock.MachineName.Split(':')[1] + "@unf.edu" });

                // Get recipient list
                var recipientsList = Settings[MonitorSettings.EmailAddresses].Replace(";", ",");                

                // Get email alias for from address
                var alias = Settings[MonitorSettings.EmailFromAlias];

                // set up mail client
                var host = Settings[MonitorSettings.EmailHost];

                string emailLog = "Sending log to " + recipientsList + ".\n" +
                                    "Sending disconnect email(s) to " + new String(freeMessages.SelectMany(a => a.Email + ", ").ToArray()) + ".\n\n";

                try
                {
                    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(host);
                    client.Send(alias, recipientsList, "RECM Log", fullLog);
                    emailLog = "Sent log to " + recipientsList + "\n\n";

                    foreach (var mailMsg in freeMessages)
                    {
                        // Un comment this line to send client emails..
                        //client.Send(alias, mailMsg.Email, mailMsg.Subject, mailMsg.Msg);
                        emailLog += "To: " + mailMsg.Email + "\nSubject: " + mailMsg.Subject + "\n" + mailMsg.Msg + "\n\n";
                    }                    
                }
                catch(Exception err)
                {
                    emailLog += "\n\n" + err.Message + "\n" + err.StackTrace;
                    Log(emailLog, EventLogEntryType.Error);
                    return;
                }
                Log(emailLog, EventLogEntryType.Information);
            }                                   
        }

        private void Log(string message, EventLogEntryType type)
        {
            if (loggingEnabled)
                _log.WriteEntry(message, type);
        }

        private void Log(string message)
        {
            this.Log(message, EventLogEntryType.Information);
        }

        private void Log(Exception e)
        {
            this.Log(e.Message + "\n" + e.StackTrace, EventLogEntryType.Error);
        }
    }
}
