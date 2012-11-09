using System;
using System.Collections.Generic;

namespace COAT.Util.Extension
{
    public static class EmurableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
    }
}