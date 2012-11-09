using System;
using System.Linq;
using System.Linq.Expressions;

namespace COAT.Util.Extension
{
    public static class OrderedQueryableExtension
    {
        public static IOrderedQueryable<TSource> OrderByDirection<TSource, TKey>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector,
            bool descending = false)
        {
            if (descending)
                return source.OrderByDescending(keySelector);

            return source.OrderBy(keySelector);
        }
    }
}