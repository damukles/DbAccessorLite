using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DbAccessorLite.Nano
{
    public static class DataTableExtensions
    {
        public static IEnumerable<string> GetColumnValues(this DataTable table, string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentNullException(nameof(columnName));

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn col in table.Columns)
                {
                    if (col.ColumnName == columnName)
                    {
                        yield return row[col.ColumnName]?.ToString().Trim();
                    }
                }
            }
        }

        public static IEnumerable<Row> GetRows(this DataTable table, string[] fields)
        {
            if (fields == null || fields.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(fields));

            foreach (DataRow row in table.Rows)
            {
                yield return new Row(table.GetRowCols(row, fields));
            }
        }

        public static IEnumerable<Column> GetRowCols(this DataTable table, DataRow row, string[] fields)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            if (fields == null || fields.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(fields));

            foreach (DataColumn col in table.Columns)
            {
                if (fields.Contains(col.ColumnName))
                {
                    yield return new Column() { Name = col.ColumnName, Value = row[col.ColumnName]?.ToString().Trim() };
                }
            }
        }
    }
}