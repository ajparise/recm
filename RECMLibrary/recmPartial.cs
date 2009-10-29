using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Collections;
using Parise.RaisersEdge.ConnectionMonitor.Data.Entities;
namespace Parise.RaisersEdge.ConnectionMonitor.Data
{
    /// <summary>
    /// Main data context for REC Monitor
    /// </summary>
    public partial class RecmDataContext
    {
        public int Kill(sysprocess process)
        {
            return this.ExecuteCommand("kill " + process.spid);
        }

        public IEnumerable<LoginAudit> GetTodaysLoginConnections()
        {
            // Must use enumerable here to split the hostname string using partial property
            return this.LoginAudits.GroupBy(a => a.HostName).Where(a => a.Max(l => l.LoginTime) >= DateTime.Today).Select(l => l.FirstOrDefault()).AsEnumerable();
        }

        /// <summary>
        /// All RE Connections - may or may not have an associated SQL process
        /// </summary>
        public IEnumerable<FilteredLockConnection> LockConnections_AllActiveREConnections
        {
            get
            {
                var tl = GetTodaysLoginConnections();
                var sys = sysprocesses;

                var tlsys = tl.Join(sys, l => l.SysProccessHostName, s => s.hostname.Trim(), (l, s) => new { Lock = l, Process = s }).GroupBy(a => a.Lock).Select(a =>
                    new FilteredLockConnection { Lock = a.Key, AllProcesses = a.Select(l => l.Process).AsEnumerable().OrderBy(r => r.IdleTime.TotalMilliseconds) });

                return tlsys;
            }
        }


        /// <summary>
        /// RE Client Connections that may or may not have an associated SQL Process
        /// </summary>
        public IEnumerable<FilteredLockConnection> LockConnections_AllActiveREConnections_ClientOnly
        {
            get
            {
                var tl = GetTodaysLoginConnections().Where(u => !u.UserName.Contains("Shelby"));
                var sys = sysprocesses;

                var tlsys = tl.Join(sys, l => l.SysProccessHostName, s => s.hostname.Trim(), (l, s) => new { Lock = l, Process = s }).GroupBy(a => a.Lock).Select(a =>
                    new FilteredLockConnection { Lock = a.Key, AllProcesses = a.Select(l => l.Process).AsEnumerable().OrderBy(r => r.IdleTime.TotalMilliseconds) });

                return tlsys;
            }
        }

        /// <summary>
        /// RE Network Connections that may or may not have an associated SQL Process
        /// Filters network connections by RE User name - "Shelby"
        /// </summary>
        public IEnumerable<FilteredLockConnection> LockConnections_AllActiveREConnections_NetworkOnly
        {
            get
            {
                var tl = GetTodaysLoginConnections().Where(u => u.UserName.Contains("Shelby"));
                var sys = sysprocesses;

                var tlsys = tl.Join(sys, l => l.SysProccessHostName, s => s.hostname.Trim(), (l, s) => new { Lock = l, Process = s }).GroupBy(a => a.Lock).Select(a =>
                    new FilteredLockConnection { Lock = a.Key, AllProcesses = a.Select(l => l.Process).AsEnumerable().OrderBy(r => r.IdleTime.TotalMilliseconds) });

                return tlsys;
            }
        }
    }

}
