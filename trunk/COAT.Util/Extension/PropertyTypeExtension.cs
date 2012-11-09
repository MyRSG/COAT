using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace COAT.Util.Extension
{
    public static class PropertyTypeExtension
    {
        public static bool IsInteger(this PropertyInfo prop)
        {
            return ContainTypeName(prop, new[] {"System.Int32", "System.Int64"});
        }

        public static bool IsDouble(this PropertyInfo prop)
        {
            return ContainTypeName(prop, new[] {"System.Double"});
        }

        public static bool IsDateTime(this PropertyInfo prop)
        {
            return ContainTypeName(prop, new[] {"System.DateTime"});
        }

        public static bool IsEntity(this PropertyInfo prop)
        {
            return ContainTypeName(prop, new[] {"EntityReference", "EntityCollection"});
        }

        private static bool ContainTypeName(PropertyInfo prop, IEnumerable<string> types)
        {
            string fullname = prop.PropertyType.FullName;
            return types.Any(t => fullname != null && fullname.Contains(t));
        }
    }
}