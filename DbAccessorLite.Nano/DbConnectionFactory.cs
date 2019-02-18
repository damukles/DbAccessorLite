using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace DbAccessorLite.Nano
{
    internal class DbConnectionFactory
    {
        internal static Func<string, DbConnection> Default = (connectionString) => new SqlConnection(connectionString);
    }
}