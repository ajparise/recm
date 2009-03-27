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

        public IEnumerable<FilteredLockConnection> LockConnections_AllActiveREConnections
        {
            get
            {
                return this.LockConnections.Select(l =>
                    new FilteredLockConnection
                    {
                        Lock = l,
                        REProcess = l.sysprocess,
                        AllProcesses = l.sysprocess != null ? l.sysprocess.RelatedProcesses : null
                    });
            }
        }

        public IEnumerable<FilteredLockConnection> LockConnections_AllActiveREConnections_ClientAliveOnly
        {
            get
            {
                return this.LockConnections.Where(l => l.User.Name != "Shelby" && l.sysprocess != null)
                    .Select(l =>
                    new FilteredLockConnection
                    {
                        Lock = l,
                        REProcess = l.sysprocess,
                        AllProcesses = l.sysprocess != null ? l.sysprocess.RelatedProcesses : null
                    });
            }
        }

        public IEnumerable<FilteredLockConnection> LockConnections_AllActiveREConnections_ClientOnly
        {
            get
            {
                return this.LockConnections.Where(l => l.User.Name != "Shelby")
                    .Select(l =>
                    new FilteredLockConnection
                    {
                        Lock = l,
                        REProcess = l.sysprocess,
                        AllProcesses = l.sysprocess != null ? l.sysprocess.RelatedProcesses : null
                    });
            }
        }

        public IEnumerable<FilteredLockConnection> LockConnections_AllActiveREConnections_NetworkAliveOnly
        {
            get
            {
                return this.LockConnections.Where(l => l.User.Name == "Shelby" && l.sysprocess != null)
                    .Select(l =>
                    new FilteredLockConnection
                    {
                        Lock = l,
                        REProcess = l.sysprocess,
                        AllProcesses = l.sysprocess != null ? l.sysprocess.RelatedProcesses : null
                    });
            }
        }

        public IEnumerable<FilteredLockConnection> LockConnections_AllActiveREConnections_NetworkOnly
        {
            get
            {
                return this.LockConnections.Where(l => l.User.Name == "Shelby")
                    .Select(l =>
                    new FilteredLockConnection
                    {
                        Lock = l,
                        REProcess = l.sysprocess,
                        AllProcesses = l.sysprocess != null ? l.sysprocess.RelatedProcesses : null
                    });
            }
        }
    }

}
