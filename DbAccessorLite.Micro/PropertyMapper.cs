using System.Data;
using System.Reflection;

namespace DbAccessorLite.Micro
{
    internal class PropertyMapper<TEntity>
    {
        private readonly PropertyTypeManager<TEntity> _typeManager;

        internal PropertyMapper()
        {
            _typeManager = new PropertyTypeManager<TEntity>();
        }

        internal void Map(DataRow row, DataColumn col, TEntity entity)
        {
            var matchedProperty = _typeManager.FindProperty(col.ColumnName);
            if (matchedProperty == default(PropertyInfo))
                return;

            var rawColValue = row[col.ColumnName];
            if (rawColValue == null)
                return;

            if (rawColValue.GetType() == typeof(System.DBNull))
            {
                if (!_typeManager.CanBeAssignedNull(matchedProperty))
                {
                    throw new NoNullAllowedException($"Property {matchedProperty.Name} does not allow null.");
                }
                return;
            }

            var colValue = _typeManager.TidyUpValue(rawColValue, matchedProperty.PropertyType);
            matchedProperty.SetValue(entity, colValue);
        }
    }
}