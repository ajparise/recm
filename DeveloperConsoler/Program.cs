using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using Parise.RaisersEdge.ConnectionMonitor.Data.Entities;
using Parise.RaisersEdge.ConnectionMonitor;
using Parise.RaisersEdge.ConnectionMonitor.Monitors;
using Parise.RaisersEdge.ConnectionMonitor.Data;

namespace DeveloperConsoler
{
    class Program
    {
        static void Main(string[] args)
        {
            var monitor = new REConnectionMonitor(true);
            monitor.StatusMessage += new REConnectionMonitor.OnStatusMessage(monitor_StatusMessage);
            Console.WriteLine("Monitor Settings (from app config)");
            Console.WriteLine(new String(monitor.Settings.Select(a => string.Format("{0}: {1}\n", Enum.GetName(a.Key.GetType(), a.Key), a.Value)).SelectMany(a => a).ToArray()));

            bool debug = true; // WARNING: when debug = false, processes will be terminated
            var freed = monitor.FreeConnections(debug);

            //Console.WriteLine("Connections that would be freed based on app.config settings");
            //foreach (var c in freed)
            //{
            //    Console.WriteLine("\n{0} -- {1} -- {2}", c.Lock.MachineName, c.Lock.User.Name, c.REProcess != null ? c.REProcess.hostname.Trim() : "N/A (Dead Lock)");
            //    foreach (var p in c.AllProcesses.OrderBy(a => a.IdleTime.TotalMilliseconds))
            //    {
            //        Console.WriteLine("\t{3} -- {0} -- {1} -- {2}",
            //           p.spid,
            //           p.program_name.Trim(),
            //           p.status.Trim(),
            //           p.IdleTimeFormatted("{h:D2}:{m:D2}:{s:D2}:{ms:D3}"));
            //    }
            //    Console.ReadLine();
            //}

            RecmDataContext db = new RecmDataContext(monitor.Settings[MonitorSettings.DBConnectionString]);

            monitor = null;

            // You should always call this stored proc before retrieving a connection list
            db.CleanupDeadConnectionLocks();
                        

            Console.WriteLine("LockConnections_AllActiveREConnections_ClientAliveOnly");            

            // Get active alive client connections
            var connections = db.LockConnections_AllActiveREConnectionsAliveOnly_ClientOnly.ToList().OrderByDescending(a => a.REProcess.IdleTime.TotalMilliseconds);

            // Calculate licenses in use by getting a distinct count of user names
            Console.WriteLine("Licenses in use: {0}", connections.Select(l => l.Lock.User.Name).Distinct().Count());
            Console.ReadLine();

            foreach (var c in connections)
            {
                Console.WriteLine("\n{0} -- {1} -- {2}", c.Lock.MachineName, c.Lock.User.Name, c.REProcess != null ? c.REProcess.hostname.Trim() : "N/A (Dead Lock)");
                foreach (var p in c.AllProcesses.OrderBy(a => a.IdleTime.TotalMilliseconds))
                {
                    Console.WriteLine("\t{3} -- {0} -- {1} -- {2}",
                       p.spid,
                       p.program_name.Trim(),
                       p.status.Trim(),
                       p.IdleTimeFormatted("{h:D2}:{m:D2}:{s:D2}:{ms:D3}"));
                }
                Console.ReadLine();
            }

            Console.WriteLine("LockConnections_AllActiveREConnections_NetworkAliveOnly");

            connections = db.LockConnections_AllActiveREConnectionsAliveOnly_NetworkOnly.ToList().OrderByDescending(a => a.REProcess.IdleTime.TotalMilliseconds);
            Console.WriteLine("Licenses in use: {0}", connections.Select(l => l.Lock.User.Name).Distinct().Count());
            Console.ReadLine();

            foreach (var c in connections)
            {
                Console.WriteLine("\n{0} -- {1} -- {2}", c.Lock.MachineName, c.Lock.User.Name, c.REProcess != null ? c.REProcess.hostname.Trim() : "N/A (Dead Lock)");
                foreach (var p in c.AllProcesses.OrderBy(a => a.IdleTime.TotalMilliseconds))
                {
                    Console.WriteLine("\t{3} -- {0} -- {1} -- {2}",
                       p.spid,
                       p.program_name.Trim(),
                       p.status.Trim(),
                       p.IdleTimeFormatted("{h:D2}:{m:D2}:{s:D2}:{ms:D3}"));
                }
                Console.ReadLine();
            }

        }

        static void monitor_StatusMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
