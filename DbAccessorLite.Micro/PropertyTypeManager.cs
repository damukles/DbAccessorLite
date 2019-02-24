using System;
using System.Linq;
using System.Reflection;

namespace DbAccessorLite.Micro
{
    internal class PropertyTypeManager<T>
    {
        private readonly PropertyInfo[] _properties;

        internal PropertyTypeManager()
        {
            _properties = typeof(T).GetProperties();
        }
        internal PropertyInfo FindProperty(string name)
        {
            return _properties.SingleOrDefault(p => p.Name == name);
        }

        internal object TidyUpValue(object colValue, Type type)
        {
            switch (type.Name)
            {
                case "String":
                case "string":
                    return colValue.ToString().Trim();

                default:
                    return colValue;
            }
        }

        internal bool CanBeAssignedNull(PropertyInfo matchedProperty)
        {
            return matchedProperty.PropertyType == typeof(string)
                || !matchedProperty.PropertyType.IsValueType
                || Nullable.GetUnderlyingType(matchedProperty.PropertyType) != null;
        }
    }
}