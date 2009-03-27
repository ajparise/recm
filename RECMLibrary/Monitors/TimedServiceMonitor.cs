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

            if (loggingEnabled)
            {
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
            }

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
            logBuffer.AppendFormat("\nDatabase Connection\n\tConnecting with: {0}\n", connectionString);
        }

        void TimedREConnectionMonitor_ConnectedToDatabase(Parise.RaisersEdge.ConnectionMonitor.Data.RecmDataContext db)
        {
            logBuffer.AppendFormat("\tSuccessfully connected to database {0}/\n", db.Connection.Database);
        }

        void TimedREConnectionMonitor_CleaningUpDeadLocks(Parise.RaisersEdge.ConnectionMonitor.Data.RecmDataContext db)
        {
            logBuffer.AppendFormat("\nDead Locks\n\tCleaning up dead locks for {0}\n", db.Connection.Database);
        }

        void TimedREConnectionMonitor_CleanedUpDeadLocks(int result)
        {
            logBuffer.AppendFormat("\tCleaned up dead locks with result {0}.\n", result);
        }

        /*
         * Respond to the _Timer elapsed event.
         */
        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.FreeConnections();

            Log(logBuffer.ToString(), EventLogEntryType.Information);
            
            logBuffer = new StringBuilder();
        }


        void _heartBeat_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Log("Heart-beat Check", EventLogEntryType.SuccessAudit);
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
