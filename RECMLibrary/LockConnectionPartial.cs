using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parise.RaisersEdge.ConnectionMonitor.Data.Entities
{
    public partial class sysprocess
    {
        private TimeSpan? _idleTime = null;
        public TimeSpan IdleTime
        {
            get
            {
                if (!_idleTime.HasValue)
                {
                    _idleTime = DateTime.Now.Subtract(this.last_batch);
                }

                return _idleTime.Value;
            }
        }

        public string IdleTimeFormatted(string format)
        {
            format = format.Replace("{ms", "{3").Replace("{h", "{0").Replace("{m", "{1").Replace("{s", "{2");
            var idleTime = IdleTime;
            return string.Format(format, idleTime.Hours, idleTime.Minutes, idleTime.Seconds, idleTime.Milliseconds);
        }
    }
}
