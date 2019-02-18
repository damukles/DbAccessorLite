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
            if (table.Rows == null || table.Rows.Count < 1)
                throw new ArgumentOutOfRangeException(nameof(table.Rows), "Table contains no rows.");

            if (table.Columns == null || table.Columns.Count < 1)
                throw new ArgumentOutOfRangeException(nameof(table.Rows), "Table contains no Columns.");

            var typeManager = new PropertyTypeManager<TEntity>();

            foreach (DataRow row in table.Rows)
            {
                var entity = new TEntity();
                foreach (DataColumn col in table.Columns)
                {
                    var matchedProperty = typeManager.FindProperty(col.ColumnName);
                    if (matchedProperty != default(PropertyInfo))
                    {
                        var rawColValue = row[col.ColumnName];
                        if (rawColValue != null)
                        {
                            var colValue = typeManager.TidyUpValue(rawColValue, matchedProperty.PropertyType);
                            matchedProperty.SetValue(entity, colValue);
                        }
                    }
                }
                yield return entity;
            }
        }
    }
}