using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Parise.RaisersEdge.ConnectionMonitor
{
    public class SqlDataConnection
    {
        SqlConnection _connection;
        SqlDataAdapter _adapter;
        string _connectionString;

        public SqlDataConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    try
                    {
                        _connection = new SqlConnection(_connectionString);
                    }
                    catch
                    {
                        throw;
                    }                    
                }

                return _connection;
            }
        }

        protected SqlDataAdapter CreateAdapter(string selectCommandText)
        {
            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }

            return new SqlDataAdapter(selectCommandText, Connection);
        }

        protected SqlCommand CreateCommand(string cmdText)
        {
            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }

            return new SqlCommand(cmdText, Connection);
        }
    }
}
