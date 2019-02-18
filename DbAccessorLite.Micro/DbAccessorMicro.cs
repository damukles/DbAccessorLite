using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using DbAccessorLite.Nano;

namespace DbAccessorLite.Micro
{
    public class DbAccessorMicro
    {
        private readonly DbAccessorNano _db;
        public DbAccessorMicro(string connectionString, Func<string, DbConnection> connectionFactory = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _db = new DbAccessorNano(connectionString, connectionFactory);
        }

        public int Execute(string query) => _db.Execute(query);

        public Task<int> ExecuteAsync(string query) => _db.ExecuteAsync(query);

        public IEnumerable<TEntity> Query<TEntity>(string query) where TEntity : new()
        {
            var table = _db.Query(query);
            return table.ToEntities<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string query) where TEntity : new()
        {
            var table = await _db.QueryAsync(query);
            return table.ToEntities<TEntity>();
        }
    }
}