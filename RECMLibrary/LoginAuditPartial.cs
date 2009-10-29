using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parise.RaisersEdge.ConnectionMonitor.Data.Entities
{
    public partial class LoginAudit
    {
        public string SysProccessHostName
        {
            get
            {
                if (!string.IsNullOrEmpty(HostName))
                {
                    return this.HostName.Split(':')[0];
                }
                else
                {
                    return this.HostName;
                }
            }
        }
    }
}
