using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using COAT.Util.Extension;
using COAT.Util.Extension.ObjectParser;

namespace COAT.Extension
{
    public static class ObjectExtension
    {
        public static object GetPropertyValue(this object obj, string property)
        {
            return obj.GetType().GetProperty(property).GetValue(obj, null);
        }

        public static void SetPropertyValue(this object obj, string property, object value)
        {
            try
            {
                var prop = obj.GetType().GetProperty(property);
                IValueParser parser = new ValueParserFactory().GetValueParser(prop);

                if (parser == null)
                    return;

                var val = parser.Parse(value);
                prop.SetValue(obj, val, null);
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        public static void Update(this object thisObj, object obj, string[] excepts)
        {
            UpdateExcept(thisObj, obj, excepts);
        }

        public static void UpdateExcept(this object thisObj, object obj, string[] excepts)
        {
            var isEntiryObj = thisObj.GetType().BaseType.Name.Contains("EntityObject");

            var propInfosA = thisObj.GetType().GetProperties();
            var propInfosB = obj.GetType().GetProperties();

            foreach (var prop in propInfosA)
            {
                if (isEntiryObj && !IsEntityScalarProperty(prop))
                    continue;

                if (excepts.Contains(prop.Name))
                    continue;

                if (propInfosB.Count(p => p.Name == prop.Name) > 0)
                {
                    var val = obj.GetPropertyValue(prop.Name);
                    thisObj.SetPropertyValue(prop.Name, val);
                }
            }

        }

        public static void UpdateInclude(this object thisObj, object obj, string[] includes)
        {
            var isEntiryObj = thisObj.GetType().BaseType.Name.Contains("EntityObject");

            var propInfosA = thisObj.GetType().GetProperties();
            var propInfosB = obj.GetType().GetProperties();

            foreach (var prop in propInfosA)
            {
                if (isEntiryObj && !IsEntityScalarProperty(prop))
                    continue;

                if (!includes.Contains(prop.Name))
                    continue;

                if (propInfosB.Count(p => p.Name == prop.Name) > 0)
                {
                    var val = obj.GetPropertyValue(prop.Name);
                    thisObj.SetPropertyValue(prop.Name, val);
                }
            }
        }

        public static bool IsEntityScalarProperty(PropertyInfo prop)
        {
            var attrName = "EdmScalarPropertyAttribute";
            var rslt = prop.GetCustomAttributes(false).Any(p => p.ToString().Contains(attrName));
            return rslt;
        }

        public static bool IsSimilarEqual(this object thisObj, object obj, string[] excepts = null)
        {
            var isEntiryObj = thisObj.GetType().BaseType.Name.Contains("EntityObject");

            var propInfosA = thisObj.GetType().GetProperties();
            var propInfosB = obj.GetType().GetProperties();

            foreach (var prop in propInfosA)
            {
                if (isEntiryObj && !IsEntityScalarProperty(prop))
                    continue;

                if (excepts != null && excepts.Contains(prop.Name))
                    continue;

                if (propInfosB.Count(p => p.Name == prop.Name) > 0)
                {
                    var val = obj.GetPropertyValue(prop.Name);
                    var val2 = thisObj.GetPropertyValue(prop.Name);

                    if (val != val2)
                        return false;
                }
            }

            return true;

        }
    }
}