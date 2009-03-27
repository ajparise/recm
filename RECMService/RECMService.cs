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
using Parise.RaisersEdge.ConnectionMonitor.Data.Entities;
using Parise.RaisersEdge.ConnectionMonitor.Monitors;
namespace Parise.RaisersEdge.ConnectionMonitor
{
    public partial class RECMService : ServiceBase
    {
        int locksFreed = 0;
        int processesKilled = 0;
        int locksSkipped = 0;
        DateTime startTime = DateTime.Now;

        bool loggingEnabled = false;

        public RECMService()
        {
            InitializeComponent();

            var settings = TimedREConnectionMonitor.LoadSettingsFromConfig();
    
            loggingEnabled = settings[MonitorSettings.LoggingEnabled].ToLower() == "true";

            if (loggingEnabled)
            {
                if (!System.Diagnostics.EventLog.SourceExists(settings[MonitorSettings.LogSource]))
                    System.Diagnostics.EventLog.CreateEventSource(settings[MonitorSettings.LogSource], settings[MonitorSettings.LogName]);

                recmEventLog.Source = settings[MonitorSettings.LogSource];
                recmEventLog.Log = settings[MonitorSettings.LogName];
            }
        }



        private TimedREConnectionMonitor monitor;
        private System.Threading.Thread monitorThread;
        protected override void OnStart(string[] args)
        {
            try
            {
                monitor = new TimedREConnectionMonitor(recmEventLog, true);
                monitor.FreedConnection += new REConnectionMonitor.FreedConnectionEvent(monitor_FreedConnection);
                monitor.SkippedFreeingConnection += new REConnectionMonitor.SkippedFreeingConnectionEvent(monitor_SkippedFreeingConnection);
                monitorThread = new System.Threading.Thread(new System.Threading.ThreadStart(monitor.StartMonitor));
                monitorThread.Start();

                var settingDisplay = new String(monitor.Settings.Select(a => string.Format("{0}: {1}\n", Enum.GetName(a.Key.GetType(), a.Key), a.Value)).SelectMany(a => a).ToArray());
                Log("RECM Service has started\n\nSettings\n---------\n" + settingDisplay, EventLogEntryType.Information);

                base.OnStart(args);
            }
            catch (Exception err)
            {
                Log(err);
                this.Stop();
            }
        }

        void monitor_SkippedFreeingConnection(string reasonSkipped, FreeingEventArgs e)
        {
            locksSkipped++;
        }

        void monitor_FreedConnection(FreedEventArgs e)
        {
            locksFreed++;
            processesKilled += e.Connection.AllProcesses.Count();
        }

        protected override void OnStop()
        {
            monitorThread.Abort();
            monitor.StopMonitor();

            var runTime = DateTime.Now.Subtract(startTime);

            Log(
                string.Format("RECM Service has stopped.\n\nStatistics\n----------------\nRun Time: {3:D2} hour(s), {4:D2} minute(s), {5:D2} second(s), {6:D2} millisecond(s)\nLocks Freed: {0}\nLocks Skipped: {1}\nProcesses Killed: {2}",
                    locksFreed, locksSkipped, processesKilled, runTime.Hours, runTime.Minutes, runTime.Seconds, runTime.Milliseconds), EventLogEntryType.Information);
            base.OnStop();
        }

        protected override void OnContinue()
        {
            Log("RECM Continued", EventLogEntryType.Information);
            monitorThread.Resume();
            base.OnContinue();
        }

        protected override void OnPause()
        {            
            Log("RECM Paused", EventLogEntryType.Information);
            monitorThread.Suspend();
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

    }

}
