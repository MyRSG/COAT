using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace COAT.Extension
{
    public static class IOrderedQueryableExtension
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
