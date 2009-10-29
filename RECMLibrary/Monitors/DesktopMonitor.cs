using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Parise.RaisersEdge.ConnectionMonitor.Data.Entities;

namespace Parise.RaisersEdge.ConnectionMonitor.Monitors
{
    public class DesktopREConnectionMonitor : REConnectionMonitor
    {
        public DesktopREConnectionMonitor() : base(false)
        {
            base.Settings = Enum.GetValues(typeof(MonitorSettings)).Cast<MonitorSettings>().Where(s => s != MonitorSettings.Unknown).ToDictionary(a => a, s => Properties.Settings.Default.PropertyValues[s.ToString()] == null ? "" : (string)Properties.Settings.Default.PropertyValues[s.ToString()].PropertyValue);
        }

        public int CountActiveConnections()
        {
            int activeCount = 0;

            using ( var db = new Parise.RaisersEdge.ConnectionMonitor.Data.RecmDataContext(base.Settings[MonitorSettings.DBConnectionString]) )
            {
                activeCount = db.LockConnections_AllActiveREConnections.Count();
            }

            return activeCount;
        }
    }
}
