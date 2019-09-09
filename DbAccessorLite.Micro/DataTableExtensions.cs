using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DbAccessorLite.Micro
{
    public static class DataTableExtensions
    {
        public static IEnumerable<TEntity> ToEntities<TEntity>(this DataTable table) where TEntity : new()
        {
            if (table.Columns == null || table.Columns.Count < 1)
                throw new ArgumentOutOfRangeException(nameof(table.Rows), "Table contains no Columns.");

            var mapper = new PropertyMapper<TEntity>();

            foreach (DataRow row in table.Rows)
            {
                var entity = new TEntity();
                foreach (DataColumn col in table.Columns)
                {
                    mapper.Map(row, col, entity);
                }
                yield return entity;
            }
        }
    }
}