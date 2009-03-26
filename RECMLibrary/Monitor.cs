using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Data.Linq;
using Parise.RaisersEdge.ConnectionMonitor.Data.Entities;

namespace Parise.RaisersEdge.ConnectionMonitor
{
    /// <summary>
    /// Enum of configuration values used by recm
    /// </summary>
    public enum MonitorSettings
    {
        /// <summary>
        /// Default Enum - Not Used
        /// </summary>
        Unknown = 0,
        DBConnectionString,
        LogSource,
        LogName,
        /// <summary>
        /// "true" - Enabled
        /// "false" - Disabled
        /// </summary>
        LoggingEnabled,

        /// <summary>
        /// Fully qualified name of the stored procedure that cleans up the LOCKCONNECTIONS table
        /// :::: Example: [REDatabaseName].[dbo].[CleanupDeadConnectionLocks]
        /// </summary>
        /// Here is the stored proc from BB
        /// --------------------------------
        /// set ANSI_NULLS ON
        /// set QUOTED_IDENTIFIER ON
        /// go
        /// 
        /// CREATE PROCEDURE [dbo].[CleanupDeadConnectionLocks] 
        /// -- SP used by CLockManager to clean up dead connection lock
        ///  AS
        /// begin
        /// 	set nocount on
        /// 	delete from lockconnections 
        /// 	      where not EXISTS (SELECT MASTER..SYSPROCESSES.SPID FROM MASTER..SYSPROCESSES WHERE MASTER..SYSPROCESSES.SPID = LOCKCONNECTIONS.SPID AND MASTER..SYSPROCESSES.LOGIN_TIME = LOCKCONNECTIONS.LOGIN_TIME)
        /// 
        /// end
        /// ----------------------------------
        DeadLockSP,

        /// <summary>
        /// The number of licenses that must be in use before the monitor will attempt to free idle connections.
        /// </summary>
        NumLicenses,

        /// <summary>
        /// The least number of minutes a connection has to be idle before it becomes a bootable candidate.
        /// </summary>
        LeastMinutesIdle,

        /// <summary>
        /// The interval in seconds to check for idle connections
        /// </summary>
        PollingInterval
    }

    /// <summary>
    /// Wraps monitor functionality with a timer for use in a service
    /// Inspired by http://www.codeguru.com/columns/dotnet/article.php/c6919__2/
    /// </summary>
    public class Monitor
    {
        private System.Diagnostics.EventLog _log;
        private System.Timers.Timer _monitor;
        private System.Timers.Timer _heartBeat;
        private bool loggingEnabled;

        public Dictionary<MonitorSettings, string> Settings { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public Monitor(System.Diagnostics.EventLog AppEventLog)
        {            
            this._log = AppEventLog;
            loggingEnabled = this._log != null;

            Settings = LoadSettings();

            // Start the monitor
            this._monitor = new System.Timers.Timer();
            //this._timer.Interval = 1000;            
            this._monitor.Interval = int.Parse(Settings[MonitorSettings.PollingInterval]);
            this._monitor.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
            this._monitor.Enabled = false;

            // Start the heartbeat
            this._heartBeat = new System.Timers.Timer();
            this._heartBeat.Interval = 60000;
            this._heartBeat.Elapsed += new System.Timers.ElapsedEventHandler(_heartBeat_Elapsed);
            this._heartBeat.Enabled = false;
        }

        /// <summary>
        /// Use this to retrieve your settings
        /// </summary>
        public static Dictionary<MonitorSettings, string> LoadSettings()
        {
            // Loads settings from app.config
            return Enum.GetValues(typeof(MonitorSettings)).Cast<MonitorSettings>().Where(s => s != MonitorSettings.Unknown).ToDictionary(a => a, s => ConfigurationManager.AppSettings.Get(s.ToString()));
        }

        /// <summary>
        /// Start the timer.
        /// </summary>
        public void StartMonitor()
        {
            this._monitor.Enabled = true;
            this._heartBeat.Enabled = true;
        }

        /// <summary>
        /// Stop the timer.
        /// </summary>
        public void StopMonitor()
        {
            this._monitor.Enabled = false;
            this._heartBeat.Enabled = false;
        }


        /*
         * Respond to the _Timer elapsed event.
         */
        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                var logEntry = string.Empty; // Holds data for the final log entry

                // Connect to the database using the connection string specified in the app settings
                var db = new Parise.RaisersEdge.ConnectionMonitor.Data.RecmDataContext(Settings[MonitorSettings.DBConnectionString]);

                #region Dead Connection Lock Clean Up (RE Stored Procedure)
                
                // Clean up dead connection locks - very important
                // Affects the dbo.LOCKCONNECTIONS table

                var dls = Settings[MonitorSettings.DeadLockSP];
                var spResult = db.ExecuteCommand("exec " + dls);

                logEntry += string.Format("Executed command: {0}\nResult: {1}\n\n", "exec " + dls, spResult); 
                
                #endregion

                // Retrieve entire connection list from DB and order by idle time
                // ToList() forces LINQ to retrieve data from the server
                var connectionList = db.LockConnections.Where(l => l.User.Name != "Shelby" && l.sysprocess != null).Select(l => new { Lock = l, REProcess = l.sysprocess, RelatedProcesses = l.sysprocess != null ? l.sysprocess.RelatedProcesses : null }).ToList().OrderByDescending(a => a.REProcess.IdleTime.TotalMilliseconds);

                // Get licenses in use (Distinct RE User Names)
                var inUse = connectionList.Select(l => l.Lock.User).Distinct().Count();

                var licenseCount = int.Parse(Settings[MonitorSettings.NumLicenses]);
                var maxMinutesIdle = double.Parse(Settings[MonitorSettings.LeastMinutesIdle]);

                logEntry += string.Format("{0}/{1} licenses in use.", inUse, licenseCount);

                if (inUse >= licenseCount)
                {
                    // Get candidates that are over the idle limit
                    var bootCandidates = connectionList.Where(a => a.REProcess.IdleTime.TotalMinutes >= maxMinutesIdle);

                    // Take enough to free one license - this ensures proper freeing if the service is restarted
                    var bootList = bootCandidates.Take(inUse - licenseCount + 1);

                    logEntry += string.Format("Attempting to free {0} license(s).\n\nBooting {1} of {2} candidates.\n\n", inUse - licenseCount + 1, bootCandidates.Count(), bootList.Count());
                    foreach (var connection in bootList)
                    {
                        // Refresh the current status of the process and related items                    
                        var freshProcess = connection.REProcess;
                        db.Refresh(RefreshMode.OverwriteCurrentValues, connection.REProcess);

                        // We only want to kill sleeping processes!!!
                        var sleepingCount = freshProcess.RelatedProcesses.Where(p => p.status.Trim().Equals("sleeping", StringComparison.CurrentCultureIgnoreCase)).Count();
                        if (sleepingCount == freshProcess.RelatedProcesses.Count)
                        {
                            logEntry += connection.Lock.MachineName + " -- " + connection.Lock.User.Name + " -- " + connection.REProcess.hostname + "\n";
                            foreach (var process in freshProcess.RelatedProcesses)
                            {
                                logEntry += "\tIssued kill " + process.spid + " -- " + process.program_name.Trim() + " -- " + process.status.Trim() + " -- " + process.IdleTimeFormatted("{h:D2}:{m:D2}:{s:D2}:{ms:D2}\n");
                                                                
                                var killResult = -1;
                                //uncomment the line below to terminate a process.
                                //killResult = db.ExecuteCommand("kill " + process.spid);
                                
                                logEntry += "\t\tResult: " + killResult + "\n";
                                
                            }
                            logEntry += "\n";
                        }
                        else
                        {
                            logEntry += "Candidate process became active....skipping.\n\n";
                        }
                    }
                }

                Log(logEntry, System.Diagnostics.EventLogEntryType.Information);               

                db.Dispose();
            }
            catch (Exception err)
            {
                Log(err);
            }
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
