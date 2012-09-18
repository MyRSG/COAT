using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace COAT.Extension
{
    public static class PropertyTypeExtension
    {
        public static bool IsInteger(this PropertyInfo prop)
        {
            return ContainTypeName(prop, new string[] { "System.Int32", "System.Int64" });
        }

        public static bool IsDouble(this PropertyInfo prop)
        {
            return ContainTypeName(prop, new string[] { "System.Double" });
        }

        public static bool IsDateTime(this PropertyInfo prop)
        {
            return ContainTypeName(prop, new string[] { "System.DateTime" });
        }

        public static bool IsEntity(this PropertyInfo prop)
        {
            return ContainTypeName(prop, new string[] { "EntityReference", "EntityCollection" });
        }

        static bool ContainTypeName(PropertyInfo prop, string[] types)
        {
            var fullname = prop.PropertyType.FullName;
            foreach (var t in types)
            {
                if (fullname.Contains(t))
                    return true;
            }

            return false;
        }
    }
}
