using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Parise.RaisersEdge.ConnectionMonitor
{
    public class RecmDataConnection : SqlDataConnection
    {
        public RecmDataConnection(string connectionString) : base(connectionString)
        {
        }
    }
}
