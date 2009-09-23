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

        /// <summary>
        /// All RE Connections - may or may not have an associated SQL process
        /// </summary>
        public IEnumerable<FilteredLockConnection> LockConnections_AllActiveREConnections
        {
            get
            {
                return this.LockConnections.Select(l =>
                    new FilteredLockConnection
                    {
                        Lock = l,
                        REProcess = l.sysprocess,
                        AllProcesses = l.sysprocess != null ? l.sysprocess.RelatedProcesses.OrderBy(a => a.last_batch) : null
                    });
            }
        }

        /// <summary>
        /// All RE Connections that have an associated SQL process
        /// </summary>
        public IEnumerable<FilteredLockConnection> LockConnections_AllActiveREConnectionsAliveOnly
        {
            get
            {
                return this.LockConnections.Select(l =>
                    new FilteredLockConnection
                    {
                        Lock = l,
                        REProcess = l.sysprocess,
                        AllProcesses = l.sysprocess != null ? l.sysprocess.RelatedProcesses.OrderBy(a => a.last_batch) : null
                    }).Where(l => l.REProcess != null);
            }
        }

        /// <summary>
        /// RE Client Connections that have an associated SQL process
        /// This may be used as an alternative to calling the clean up stored procedure
        /// </summary>
        public IEnumerable<FilteredLockConnection> LockConnections_AllActiveREConnectionsAliveOnly_ClientOnly
        {
            get
            {
                return this.LockConnections.Where(l => l.User.Name != "Shelby")
                    .Select(l =>
                    new FilteredLockConnection
                    {
                        Lock = l,
                        REProcess = l.sysprocess,
                        AllProcesses = l.sysprocess != null ? l.sysprocess.RelatedProcesses.OrderBy( a => a.last_batch) : null
                    }).Where(l => l.REProcess != null);
            }
        }

        /// <summary>
        /// RE Client Connections that may or may not have an associated SQL Process
        /// </summary>
        public IEnumerable<FilteredLockConnection> LockConnections_AllActiveREConnections_ClientOnly
        {
            get
            {
                //this.LoadOptions.LoadWith<LockConnection>(a => a.sysprocess);
                //this.LoadOptions.LoadWith<sysprocess>(a => a.RelatedProcesses);
                //this.LoadOptions.AssociateWith<User>(l => l.Name != "Shelby");
                return this.LockConnections.Where(l => l.User.Name != "Shelby")
                    .Select(l =>
                    new FilteredLockConnection
                    {
                        Lock = l,
                        REProcess = l.sysprocess,
                        AllProcesses = l.sysprocess != null ? l.sysprocess.RelatedProcesses.OrderBy(a => a.last_batch) : null
                    });
            }
        }

        /// <summary>
        /// RE Network Connections that have an associated SQL Process
        /// Filters network connections by RE User name - "Shelby"
        /// This may be used as an alternative to calling the clean up stored procedure
        /// </summary>
        public IEnumerable<FilteredLockConnection> LockConnections_AllActiveREConnectionsAliveOnly_NetworkOnly
        {
            get
            {
                return this.LockConnections.Where(l => l.User.Name.ToLower().Contains("Shelby"))
                    .Select(l =>
                    new FilteredLockConnection
                    {
                        Lock = l,
                        REProcess = l.sysprocess,
                        AllProcesses = l.sysprocess != null ? l.sysprocess.RelatedProcesses.OrderBy(a => a.last_batch) : null
                    }).Where(l => l.REProcess != null);
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
                return this.LockConnections.Where(l => l.User.Name.ToLower().Contains("Shelby"))
                    .Select(l =>
                    new FilteredLockConnection
                    {
                        Lock = l,
                        REProcess = l.sysprocess,
                        AllProcesses = l.sysprocess != null ? l.sysprocess.RelatedProcesses.OrderBy(a => a.last_batch) : null
                    });
            }
        }
    }

}
