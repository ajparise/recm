using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parise.RaisersEdge.ConnectionMonitor.Monitors
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

        ///// <summary>
        ///// Fully qualified name of the stored procedure that cleans up the LOCKCONNECTIONS table
        ///// :::: Example: [REDatabaseName].[dbo].[CleanupDeadConnectionLocks]
        ///// </summary>
        ///// Here is the stored proc from BB
        ///// --------------------------------
        ///// set ANSI_NULLS ON
        ///// set QUOTED_IDENTIFIER ON
        ///// go
        ///// 
        ///// CREATE PROCEDURE [dbo].[CleanupDeadConnectionLocks] 
        ///// -- SP used by CLockManager to clean up dead connection lock
        /////  AS
        ///// begin
        ///// 	set nocount on
        ///// 	delete from lockconnections 
        ///// 	      where not EXISTS (SELECT MASTER..SYSPROCESSES.SPID FROM MASTER..SYSPROCESSES WHERE MASTER..SYSPROCESSES.SPID = LOCKCONNECTIONS.SPID AND MASTER..SYSPROCESSES.LOGIN_TIME = LOCKCONNECTIONS.LOGIN_TIME)
        ///// 
        ///// end
        ///// ----------------------------------
        //DeadLockSP,

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
}
