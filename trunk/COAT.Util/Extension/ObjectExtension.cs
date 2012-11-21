using System;
using System.Linq;
using System.Reflection;
using COAT.Util.Extension.ObjectParser;

namespace COAT.Util.Extension
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
                PropertyInfo prop = obj.GetType().GetProperty(property);
                IValueParser parser = new ValueParserFactory().GetValueParser(prop);

                if (parser == null)
                    return;

                object val = parser.Parse(value);
                prop.SetValue(obj, val, null);
            }
            catch
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
            bool isEntiryObj = IsEntityObject(thisObj);

            PropertyInfo[] propInfosA = thisObj.GetType().GetProperties();
            PropertyInfo[] propInfosB = obj.GetType().GetProperties();

            foreach (PropertyInfo prop in propInfosA)
            {
                if (isEntiryObj && !IsEntityScalarProperty(prop))
                    continue;

                if (excepts.Contains(prop.Name))
                    continue;

                if (propInfosB.Count(p => p.Name == prop.Name) > 0)
                {
                    object val = obj.GetPropertyValue(prop.Name);
                    thisObj.SetPropertyValue(prop.Name, val);
                }
            }
        }

        public static void UpdateInclude(this object thisObj, object obj, string[] includes)
        {
            bool isEntiryObj = IsEntityObject(thisObj);

            PropertyInfo[] propInfosA = thisObj.GetType().GetProperties();
            PropertyInfo[] propInfosB = obj.GetType().GetProperties();

            foreach (PropertyInfo prop in propInfosA)
            {
                if (isEntiryObj && !IsEntityScalarProperty(prop))
                    continue;

                if (!includes.Contains(prop.Name))
                    continue;

                if (propInfosB.Count(p => p.Name == prop.Name) > 0)
                {
                    object val = obj.GetPropertyValue(prop.Name);
                    thisObj.SetPropertyValue(prop.Name, val);
                }
            }
        }

        public static bool IsSimilarEqual(this object thisObj, object obj, string[] excepts = null)
        {
            bool isEntiryObj = IsEntityObject(thisObj);

            PropertyInfo[] propInfosA = thisObj.GetType().GetProperties();
            PropertyInfo[] propInfosB = obj.GetType().GetProperties();

            foreach (PropertyInfo prop in propInfosA)
            {
                if (isEntiryObj && !IsEntityScalarProperty(prop))
                    continue;

                if (excepts != null && excepts.Contains(prop.Name))
                    continue;

                if (propInfosB.Count(p => p.Name == prop.Name) <= 0)
                    continue;

                object val = obj.GetPropertyValue(prop.Name);
                object val2 = thisObj.GetPropertyValue(prop.Name);

                if (val != val2)
                    return false;
            }

            return true;
        }

        public static bool IsEntityScalarProperty(PropertyInfo prop)
        {
            const string attrName = "EdmScalarPropertyAttribute";
            return prop.GetCustomAttributes(false).Any(p => p.ToString().Contains(attrName));
        }

        public static bool IsEntityObject(object thisObj)
        {
            Type baseType = thisObj.GetType().BaseType;
            return baseType != null && baseType.Name.Contains("EntityObject");
        }
    }
}