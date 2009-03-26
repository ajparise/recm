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

namespace Parise.RaisersEdge.ConnectionMonitor
{
    public partial class RECMService : ServiceBase
    {
        bool loggingEnabled = false;

        public RECMService()
        {
            InitializeComponent();

            var settings = Monitor.LoadSettings();
    
            loggingEnabled = settings[MonitorSettings.LoggingEnabled].ToLower() == "true";

            if (loggingEnabled)
            {
                if (!System.Diagnostics.EventLog.SourceExists(settings[MonitorSettings.LogSource]))
                    System.Diagnostics.EventLog.CreateEventSource(settings[MonitorSettings.LogSource], settings[MonitorSettings.LogName]);

                recmEventLog.Source = settings[MonitorSettings.LogSource];
                recmEventLog.Log = settings[MonitorSettings.LogName];
            }
        }
        


        private Monitor monitor;
        private System.Threading.Thread monitorThread;
        protected override void OnStart(string[] args)
        {
            try
            {
                monitor = new Monitor(recmEventLog);
                monitorThread = new System.Threading.Thread(new System.Threading.ThreadStart(monitor.StartMonitor));
                monitorThread.Start();

                var settingDisplay = new String(Monitor.LoadSettings().Select(a => string.Format("{0}: {1}\n", Enum.GetName(a.Key.GetType(), a.Key), a.Value)).SelectMany(a => a).ToArray());
                Log("RECM Started with the following settings\n\n" + settingDisplay, EventLogEntryType.Information);
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

    }

}
