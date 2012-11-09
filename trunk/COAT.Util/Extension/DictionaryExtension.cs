using System.Collections.Generic;

namespace COAT.Util.Extension
{
    public static class DictionaryExtension
    {
        public static void Update<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
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

        public static TValue SafeGet<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue defaultValue)
        {
            if (!dic.ContainsKey(key))
                return defaultValue;

            return dic[key];
        }
    }
}