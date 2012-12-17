using System;
using System.Linq;
using System.Reflection;
using COAT.Util.Extension.ObjectParser;
using COAT.Util.Log;

namespace COAT.Util.Extension
{
    public static class ObjectExtension
    {
        private static readonly Logger Logger = new Logger();

        public static object GetPropertyValue(this object obj, string property)
        {
            return obj.GetType().GetProperty(property).GetValue(obj, null);
        }

        public static void SetPropertyValue(this object obj, string property, object value)
        {
            try
            {
                var prop = obj.GetType().GetProperty(property);
                var parser = new ValueParserFactory().GetValueParser(prop);

                if (parser == null)
                    return;

                var val = parser.Parse(value);
                prop.SetValue(obj, val, null);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }

        public static void Update(this object thisObj, object obj, string[] excepts)
        {
            UpdateExcept(thisObj, obj, excepts);
        }

        public static void UpdateExcept(this object thisObj, object obj, string[] excepts)
        {
            var isEntiryObj = IsEntityObject(thisObj);

            var propInfosA = thisObj.GetType().GetProperties();
            var propInfosB = obj.GetType().GetProperties();

            foreach (var prop in propInfosA)
            {
                if (isEntiryObj && !IsEntityScalarProperty(prop))
                    continue;

                if (excepts.Contains(prop.Name))
                    continue;

                if (propInfosB.Count(p => p.Name == prop.Name) <= 0) 
                    continue;
                
                var val = obj.GetPropertyValue(prop.Name);
                thisObj.SetPropertyValue(prop.Name, val);
            }
        }

        public static void UpdateInclude(this object thisObj, object obj, string[] includes)
        {
            var isEntiryObj = IsEntityObject(thisObj);

            var propInfosA = thisObj.GetType().GetProperties();
            var propInfosB = obj.GetType().GetProperties();

            foreach (var prop in propInfosA)
            {
                if (isEntiryObj && !IsEntityScalarProperty(prop))
                    continue;

                if (!includes.Contains(prop.Name))
                    continue;

                if (propInfosB.Count(p => p.Name == prop.Name) <= 0) 
                    continue;
                
                var val = obj.GetPropertyValue(prop.Name);
                thisObj.SetPropertyValue(prop.Name, val);
            }
        }

        public static bool IsSimilarEqual(this object thisObj, object obj, string[] excepts = null)
        {
            var isEntiryObj = IsEntityObject(thisObj);

            var propInfosA = thisObj.GetType().GetProperties();
            var propInfosB = obj.GetType().GetProperties();

// ReSharper disable LoopCanBeConvertedToQuery
            foreach (var prop in propInfosA)
// ReSharper restore LoopCanBeConvertedToQuery
            {
                if (isEntiryObj && !IsEntityScalarProperty(prop))
                    continue;

                if (excepts != null && excepts.Contains(prop.Name))
                    continue;

                if (propInfosB.Count(p => p.Name == prop.Name) <= 0)
                    continue;

                var val = obj.GetPropertyValue(prop.Name);
                var val2 = thisObj.GetPropertyValue(prop.Name);

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
            var baseType = thisObj.GetType().BaseType;
            return baseType != null && baseType.Name.Contains("EntityObject");
        }
    }
}