using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Utilities
{
    internal static class IQueryableExtensions
    {
        internal static IQueryable<T> Paginate<T>(this IQueryable<T> query,int page,int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

    }
}
