using LocationSystem.Domain.Entities.Interfacies;
using System.Linq.Expressions;

namespace LocationSystem.Application.Extentions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// 过滤掉已删除的实体
        /// </summary>
        /// <typeparam name="T">实现了 ISoftDeleteEntity 接口的实体类型</typeparam>
        /// <param name="query">查询对象</param>
        /// <returns>过滤后的查询对象</returns>
        public static IQueryable<T> WhereNotDeleted<T>(this IQueryable<T> query) where T : ISoftDeleteEntity
        {
            return query.Where(x => !x.IsDelete);
        }

        /// <summary>
        /// 过滤掉已删除的实体，并应用额外的过滤条件
        /// </summary>
        /// <typeparam name="T">实现了 ISoftDeleteEntity 接口的实体类型</typeparam>
        /// <param name="query">查询对象</param>
        /// <param name="predicate">额外的过滤表达式</param>
        /// <returns>过滤后的查询对象</returns>
        public static IQueryable<T> WhereNotDeleted<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate) where T : ISoftDeleteEntity
        {
            return query.Where(x => !x.IsDelete).Where(predicate);
        }
    }
}