using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using Parise.RaisersEdge.ConnectionMonitor.Data.Entities;

namespace DeveloperConsoler
{
    class Program
    {
        static void Main(string[] args)
        {
            Parise.RaisersEdge.ConnectionMonitor.Data.RecmDataContext db = new Parise.RaisersEdge.ConnectionMonitor.Data.RecmDataContext();

            // Clean up dead connection locks - very important
            db.ExecuteCommand("exec [UNFF].[dbo].[CleanupDeadConnectionLocks]");

            // Retrieve entire connection list from DB and order by idle time
            // ToList() forces LINQ to retrieve data from the server
            var connectionList = db.LockConnections.Where(l => l.User.Name != "Shelby" && l.sysprocess != null).Select(l => new { Lock = l, REProcess = l.sysprocess, RelatedProcesses = l.sysprocess != null ? l.sysprocess.RelatedProcesses : null }).ToList().OrderByDescending(a => a.REProcess.IdleTime.TotalMilliseconds);

            foreach (var connection in connectionList)
            {
                Console.WriteLine(connection.Lock.LoginTime.Value.ToShortTimeString() + " -- " + connection.Lock.MachineName + " -- " + connection.Lock.User.Name + " -- " + connection.REProcess.hostname);

                var process = connection.REProcess;
                Console.WriteLine(process.spid + " -- " + process.program_name.Trim() + " -- " + process.status.Trim() + " -- " + process.IdleTimeFormatted("{h:D2}:{m:D2}:{s:D2}:{ms:D2}\n"));
            }

            // Get licenses in use (Distinct RE User Names)
            var inUse = connectionList.Select(l => l.Lock.User).Distinct().Count();

            var licenseCount = 6;
            var maxMinutesIdle = 100.0;

            if (inUse >= licenseCount)
            {
                // Get candidates that are over the idle limit
                var bootCandidates = connectionList.Where(a => a.REProcess.IdleTime.TotalMinutes >= maxMinutesIdle);

                // Take enough to free one license - this ensures proper freeing if the service is restarted
                var bootList = bootCandidates.Take(inUse - licenseCount + 1);

                Console.WriteLine(string.Format("{0}/{1} in use.\nAttempting to free {2} license(s).\n\nBooting {4} of {3} candidates.\n", inUse, licenseCount, inUse - licenseCount + 1, bootCandidates.Count(), bootList.Count()));
                foreach (var connection in bootList)
                {
                    Console.WriteLine(connection.Lock.MachineName + " -- " + connection.Lock.User.Name + " -- " + connection.REProcess.hostname);

                    // Refresh the current status of the process and related items                    
                    var freshProcess = connection.REProcess;
                    db.Refresh(RefreshMode.OverwriteCurrentValues, connection.REProcess);

                    // We only want to kill sleeping processes!!!
                    var sleepingCount = freshProcess.RelatedProcesses.Where(p => p.status.Trim().Equals("sleeping", StringComparison.CurrentCultureIgnoreCase)).Count();
                    if (sleepingCount == freshProcess.RelatedProcesses.Count)
                    {
                        foreach (var process in freshProcess.RelatedProcesses)
                        {
                            Console.WriteLine("\tkill " + process.spid + " -- " + process.program_name.Trim() + " -- " + process.status.Trim() + " -- " + process.IdleTimeFormatted("{h:D2}:{m:D2}:{s:D2}:{ms:D2}"));
                            //uncomment the line below to terminate a process.
                            //db.ExecuteCommand("kill " + process.spid);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Process is active....skipping.\n");
                    }
                    Console.ReadLine();
                }
            }

            db.Dispose();
        }
    }
}
