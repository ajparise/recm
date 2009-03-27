using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Data.Linq;
using Parise.RaisersEdge.ConnectionMonitor.Data.Entities;
using Parise.RaisersEdge.ConnectionMonitor.Data;

namespace Parise.RaisersEdge.ConnectionMonitor.Monitors
{

    #region Event Args

    public class ErrorEventArgs<T> : EventArgs where T : Exception
    {
        public readonly T Error;
        public bool EndExecution;

        public ErrorEventArgs(T err) : base()
        {
            Error = err;
        }
    }

    public class LockConnectionEventArgs : EventArgs
    {
        public readonly FilteredLockConnection Connection;
        public LockConnectionEventArgs(FilteredLockConnection connection)
            : base()
        {
            Connection = connection;
        }
    }

    public class FreeingEventArgs : LockConnectionEventArgs
    {
        public readonly sysprocess Process;
        public bool Cancel { get; set; }
        public FreeingEventArgs(FilteredLockConnection connection, sysprocess process)
            : base(connection)
        {
            Process = process;
        }
    }

    public class FreedEventArgs : LockConnectionEventArgs
    {
        public readonly sysprocess Process;
        public readonly int Result;
        public FreedEventArgs(FilteredLockConnection connection, sysprocess process, int result)
            : base(connection)
        {
            Process = process;
            Result = result;
        }
    }

    #endregion

    /// <summary>
    /// Wraps monitor functionality
    /// </summary>
    public partial class REConnectionMonitor
    {        
        /// <summary>
        /// Constructor
        /// </summary>
        public REConnectionMonitor(bool useAppConfiguration)
        {
            if (useAppConfiguration)
                Settings = LoadSettingsFromConfig();
        }

        #region Settings
            public Dictionary<MonitorSettings, string> Settings { get; set; }

            /// <summary>
            /// Use this to retrieve settings from the app config using enum names as key values
            /// </summary>
            public static Dictionary<MonitorSettings, string> LoadSettingsFromConfig()
            {
                // Loads settings from app.config
                return Enum.GetValues(typeof(MonitorSettings)).Cast<MonitorSettings>().Where(s => s != MonitorSettings.Unknown).ToDictionary(a => a, s => ConfigurationManager.AppSettings.Get(s.ToString()));
            }        
        #endregion

        #region Public Events

        public event OnCleanedUpDeadLocks CleanedUpDeadLocks;
        public delegate void OnCleanedUpDeadLocks(int result);

        public event DatabaseEvent ConnectedToDatabase;
        public event DatabaseEvent CleaningUpDeadLocks;
        public delegate void DatabaseEvent(RecmDataContext db);

        public event OnConnectingToDatabase ConnectingToDatabase;
        public delegate void OnConnectingToDatabase(ref string connectionString);

        public event OnStatusMessage StatusMessage;
        public delegate void OnStatusMessage(string message);

        public event FreeingConnectionEvent FreeingConnection;
        public delegate void FreeingConnectionEvent(FreeingEventArgs e);

        public event SkippedFreeingConnectionEvent SkippedFreeingConnection;
        public delegate void SkippedFreeingConnectionEvent(string reasonSkipped, FreeingEventArgs e);

        public event FreedConnectionEvent FreedConnection;
        public delegate void FreedConnectionEvent(FreedEventArgs e);

        public event FreeingLockEvent FreeingLock;
        public delegate void FreeingLockEvent(FilteredLockConnection connection);

        public event ErrorEvent GenericError;
        public delegate void ErrorEvent(ErrorEventArgs<Exception> e);
        
        #endregion

        // Base implementation 
        public virtual IEnumerable<FilteredLockConnection> FreeConnections()
        {
        try
            {
                // Connect to the database using the connection string specified in the app settings
                var connectionString = Settings[MonitorSettings.DBConnectionString];

                if (ConnectingToDatabase != null)
                    ConnectingToDatabase(ref connectionString);

                var db = new Parise.RaisersEdge.ConnectionMonitor.Data.RecmDataContext();

                if (ConnectedToDatabase != null)
                    ConnectedToDatabase(db);

                #region Dead Connection Lock Clean Up (RE Stored Procedure)
                
                // Clean up dead connection locks - very important
                // Affects the dbo.LOCKCONNECTIONS table

                if (CleaningUpDeadLocks != null)
                    CleaningUpDeadLocks(db);

                var spResult = db.CleanupDeadConnectionLocks();

                if (CleanedUpDeadLocks != null)
                    CleanedUpDeadLocks(spResult);
                
                #endregion

                // Retrieve entire connection list from DB and order by idle time
                // ToList() forces LINQ to retrieve data from the server
                var connectionList = db.LockConnections_AllActiveREConnections_ClientAliveOnly.ToList().OrderByDescending(a => a.REProcess.IdleTime.TotalMilliseconds);

                // Get licenses in use (Distinct RE User Names)
                var inUse = connectionList.Select(l => l.Lock.User).Distinct().Count();

                var totalLicenseCount = int.Parse(Settings[MonitorSettings.NumLicenses]);
                var leastMinutesIdle = double.Parse(Settings[MonitorSettings.LeastMinutesIdle]);

                if (StatusMessage != null)
                    StatusMessage(string.Format("{0}/{1} licenses in use.", inUse, totalLicenseCount));

                if (inUse >= totalLicenseCount)
                {
                    // Get candidates that are over the idle limit
                    var bootCandidates = connectionList.Where(a => a.REProcess.IdleTime.TotalMinutes >= leastMinutesIdle);

                    // Take enough to free one license - this ensures proper freeing if the service is restarted
                    var bootList = bootCandidates.Take(inUse - totalLicenseCount + 1);

                    if (StatusMessage != null)
                        StatusMessage(string.Format("Attempting to free {0} license(s).\n\nBooting {1} of {2} candidates.\n\n", inUse - totalLicenseCount + 1, bootCandidates.Count(), bootList.Count()));

                    foreach (var connection in bootList)
                    {
                        if (FreeingLock != null)
                            FreeingLock(connection);

                        // Refresh the current status of the process and related items                    
                        var freshProcess = connection.REProcess;
                        db.Refresh(RefreshMode.OverwriteCurrentValues, connection.REProcess);

                        // We only want to kill sleeping processes!!!
                        var sleepingCount = freshProcess.RelatedProcesses.Where(p => p.status.Trim().Equals("sleeping", StringComparison.CurrentCultureIgnoreCase)).Count();
                        if (sleepingCount == freshProcess.RelatedProcesses.Count)
                        {
                            //loggableEntry += connection.Lock.MachineName + " -- " + connection.Lock.User.Name + " -- " + connection.REProcess.hostname + "\n";
                            //"\tIssued kill " + process.spid + " -- " + process.program_name.Trim() + " -- " + process.status.Trim() + " -- " + process.IdleTimeFormatted("{h:D2}:{m:D2}:{s:D2}:{ms:D2}\n");
                            foreach (var process in freshProcess.RelatedProcesses)
                            {

                                var killResult = 99;

                                //uncomment the line below to terminate a process.
                                bool killIt = true;

                                if (FreeingConnection != null)
                                {
                                    var args = new FreeingEventArgs(connection, process);
                                    FreeingConnection(args);
                                    killIt = !args.Cancel;
                                }

                                if (killIt)
                                {
                                    //killResult = db.Kill(process);

                                    if (FreedConnection != null)
                                        FreedConnection(new FreedEventArgs(connection, process, killResult));
                                }
                            }
                        }
                        else
                        {
                            if (SkippedFreeingConnection != null)
                                SkippedFreeingConnection("Some related processes are active.", new FreeingEventArgs(connection, connection.REProcess));
                        }
                    }

                    db.Dispose();
                    return bootList;
                }
                else
                {
                    db.Dispose();
                    return new List<FilteredLockConnection> { };
                }
            }
            catch (Exception err)
            {
                if (GenericError != null)
                    GenericError(new ErrorEventArgs<Exception>(err));

                return new List<FilteredLockConnection> { };
            }
        }
    }
}
