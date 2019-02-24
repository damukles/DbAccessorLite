using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace DbAccessorLite.Nano
{
    public class DbAccessorNano
    {
        private readonly string _connectionString;
        private readonly Func<string, DbConnection> _connectionFactory;

        public DbAccessorNano(string connectionString, Func<string, DbConnection> connectionFactory = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (connectionFactory == null)
                connectionFactory = DbConnectionFactory.Default;

            _connectionString = connectionString;
            _connectionFactory = connectionFactory;
        }

        public int Execute(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException(nameof(query));

            using (var conn = _connectionFactory.Invoke(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public async Task<int> ExecuteAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException(nameof(query));

            using (var conn = _connectionFactory.Invoke(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public DataTable Query(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException(nameof(query));

            using (var conn = _connectionFactory.Invoke(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable tbl = new DataTable();
                        tbl.Load(reader);
                        return tbl;
                    }
                }
            }
        }

        public async Task<DataTable> QueryAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException(nameof(query));

            using (var conn = _connectionFactory.Invoke(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        DataTable tbl = new DataTable();
                        tbl.Load(reader);
                        return tbl;
                    }
                }
            }
        }
    }
}