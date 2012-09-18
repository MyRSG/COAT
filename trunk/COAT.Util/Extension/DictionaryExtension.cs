using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COAT.Extension
{
    public static class DictionaryExtension
    {
        public static void Update<Tkey, TValue>(this Dictionary<Tkey, TValue> dic, Tkey key, TValue value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
        }

        public static TValue SafeGet<Tkey, TValue>(this Dictionary<Tkey, TValue> dic, Tkey key, TValue defaultValue)
        {
            if (!dic.ContainsKey(key))
                return defaultValue;

            return dic[key];
        }

    }
}