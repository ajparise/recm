using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parise.RaisersEdge.ConnectionMonitor.Data.Entities
{
    public class FilteredLockConnection
    {
        public LockConnection Lock { get; set; }
        public sysprocess REProcess { get; set; }
        public IEnumerable<sysprocess> AllProcesses { get; set; }
    }
}
