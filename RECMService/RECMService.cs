using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Data.Linq;

namespace Parise.RaisersEdge.ConnectionMonitor
{
    public partial class RECMService : ServiceBase
    {
        bool loggingEnabled = false;
        public RECMService()
        {
            InitializeComponent();

            loggingEnabled = ConfigurationSettings.AppSettings["LoggingEnabled"] == "true";

            if (loggingEnabled)
            {
                if (!System.Diagnostics.EventLog.SourceExists(ConfigurationSettings.AppSettings["LogSource"]))
                    System.Diagnostics.EventLog.CreateEventSource(ConfigurationSettings.AppSettings["LogSource"], ConfigurationSettings.AppSettings["LogName"]);

                recmEventLog.Source = ConfigurationSettings.AppSettings["LogSource"];
                recmEventLog.Log = ConfigurationSettings.AppSettings["LogName"];
            }
        }

        private Monitor monitor;
        private System.Threading.Thread monitorThread;
        protected override void OnStart(string[] args)
        {
            try
            {
                monitor = new Monitor(1000, recmEventLog);
                monitorThread = new System.Threading.Thread(new System.Threading.ThreadStart(monitor.StartMonitor));
                monitorThread.Start();

                Log("RECM Started", EventLogEntryType.Information);
                base.OnStart(args);
            }
            catch (Exception err)
            {
                Log(err);
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            monitorThread.Abort();
            monitor.StopMonitor();
            Log("RECM Stopped", EventLogEntryType.Information);
            base.OnStop();
        }

        protected override void OnContinue()
        {
            Log("RECM Continued", EventLogEntryType.Information);
            base.OnContinue();
        }

        protected override void OnPause()
        {
            Log("RECM Paused", EventLogEntryType.Information);
            base.OnPause();
        }

        protected override void OnShutdown()
        {
            Log("RECM Shutdown", EventLogEntryType.Information);
            base.OnShutdown();
        }

        private void Log(string message, EventLogEntryType type)
        {
            if (loggingEnabled)
                recmEventLog.WriteEntry(message, type);
        }

        private void Log(string message)
        {
            this.Log(message, EventLogEntryType.Information);
        }

        private void Log(Exception e)
        {
            this.Log(e.Message + "\n" + e.StackTrace, EventLogEntryType.Error);
        }


        /// <summary>
        /// Inspired by http://www.codeguru.com/columns/dotnet/article.php/c6919__2/
        /// </summary>
        public class Monitor
        {
            private System.Diagnostics.EventLog _log;
            private System.Timers.Timer _timer;
            private bool loggingEnabled;

            private int _interval = 1000;

            /// <summary>
            /// Constructor
            /// </summary>
            public Monitor(int pollingInterval, System.Diagnostics.EventLog AppEventLog)
            {
                this._log = AppEventLog;
                loggingEnabled = this._log == null;

                this._timer = new System.Timers.Timer();
                //this._interval = pollingInterval;
                this._timer.Interval = pollingInterval;
                this._timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
                this._timer.Enabled = false;
            }

            /// <summary>
            /// Start the timer.
            /// </summary>
            public void StartMonitor()
            {
                this._timer.Enabled = true;
            }

            /// <summary>
            /// Stop the timer.
            /// </summary>
            public void StopMonitor()
            {
                this._timer.Enabled = false;
            }


            /*
             * Respond to the _Timer elapsed event.
             */
            private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
            {
                try
                {
                    var logEntry = string.Empty;

                    Parise.RaisersEdge.ConnectionMonitor.Data.RecmDataContext db = new Parise.RaisersEdge.ConnectionMonitor.Data.RecmDataContext(ConfigurationSettings.AppSettings["DBCS"]);

                    // Clean up dead connection locks - very important
                    int spResult = db.ExecuteCommand("exec " + ConfigurationSettings.AppSettings["DeadLockSP"]);

                    Log(string.Format("Executed command: {0}\nResult: {1}", "exec " + ConfigurationSettings.AppSettings["DeadLockSP"], spResult), EventLogEntryType.Information);

                    // Retrieve entire connection list from DB and order by idle time
                    // ToList() forces LINQ to retrieve data from the server
                    var connectionList = db.LockConnections.Where(l => l.User.Name != "Shelby" && l.sysprocess != null).Select(l => new { Lock = l, REProcess = l.sysprocess, RelatedProcesses = l.sysprocess != null ? l.sysprocess.RelatedProcesses : null }).ToList().OrderByDescending(a => a.REProcess.IdleTime.TotalMilliseconds);

                    // Get licenses in use (Distinct RE User Names)
                    var inUse = connectionList.Select(l => l.Lock.User).Distinct().Count();

                    var licenseCount = 1;
                    var maxMinutesIdle = 100.0;

                    if (inUse >= licenseCount)
                    {
                        // Get candidates that are over the idle limit
                        var bootCandidates = connectionList.Where(a => a.REProcess.IdleTime.TotalMinutes >= maxMinutesIdle);

                        // Take enough to free one license - this ensures proper freeing if the service is restarted
                        var bootList = bootCandidates.Take(inUse - licenseCount + 1);

                        logEntry += string.Format("{0}/{1} in use.\nAttempting to free {2} license(s).\n\nBooting {4} of {3} candidates.\n\n", inUse, licenseCount, inUse - licenseCount + 1, bootCandidates.Count(), bootList.Count());
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
                                    //uncomment the line below to terminate a process.
                                    //db.ExecuteCommand("kill " + process.spid);                            
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

}
