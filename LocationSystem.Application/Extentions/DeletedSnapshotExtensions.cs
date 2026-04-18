using LocationSystem.Domain.Entities.DeletedSnapshots;

namespace LocationSystem.Application.Extentions
{
    /// <summary>
    /// DeletedSnapshot 查询规格类
    /// </summary>
    public static class DeletedSnapshotExtensions
    {
        /// <summary>
        /// 查询指定类型的所有删除快照
        /// </summary>
        public static IQueryable<DeletedSnapshot> OfType<T>(this IQueryable<DeletedSnapshot> query) where T : class
        {
            return query.Where(s => s.EntityType == typeof(T).Name);
        }

        /// <summary>
        /// 查询指定实体ID的删除快照
        /// </summary>
        public static IQueryable<DeletedSnapshot> OfEntity<T>(this IQueryable<DeletedSnapshot> query, object entityId) where T : class
        {
            return query.Where(s => s.EntityType == typeof(T).Name && s.EntityId == entityId.ToString());
        }

        /// <summary>
        /// 获取时间范围内的删除记录
        /// </summary>
        public static IQueryable<DeletedSnapshot> DeletedBetween(this IQueryable<DeletedSnapshot> query, DateTime start, DateTime end)
        {
            return query.Where(s => s.DeletedAt >= start && s.DeletedAt <= end);
        }

        /// <summary>
        /// 按删除人分组统计
        /// </summary>
        public static Dictionary<string, int> GroupByDeleter(this IEnumerable<DeletedSnapshot> snapshots)
        {
            return snapshots
                .Where(s => !string.IsNullOrEmpty(s.DeletedBy))
                .GroupBy(s => s.DeletedBy!)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// 查询指定时间之后的删除记录
        /// </summary>
        public static IQueryable<DeletedSnapshot> DeletedAfter(this IQueryable<DeletedSnapshot> query, DateTime dateTime)
        {
            return query.Where(s => s.DeletedAt >= dateTime);
        }

        /// <summary>
        /// 查询指定时间之前的删除记录
        /// </summary>
        public static IQueryable<DeletedSnapshot> DeletedBefore(this IQueryable<DeletedSnapshot> query, DateTime dateTime)
        {
            return query.Where(s => s.DeletedAt <= dateTime);
        }

        /// <summary>
        /// 查询包含指定关键字的删除记录
        /// </summary>
        public static IQueryable<DeletedSnapshot> ContainsInDisplayName(this IQueryable<DeletedSnapshot> query, string keyword)
        {
            return query.Where(s => s.EntityDisplayName != null && s.EntityDisplayName.Contains(keyword));
        }

        /// <summary>
        /// 查询指定删除人的删除记录
        /// </summary>
        public static IQueryable<DeletedSnapshot> DeletedByUser(this IQueryable<DeletedSnapshot> query, string userName)
        {
            return query.Where(s => s.DeletedBy == userName);
        }

        /// <summary>
        /// 查询指定删除原因的删除记录
        /// </summary>
        public static IQueryable<DeletedSnapshot> WithReason(this IQueryable<DeletedSnapshot> query, string reason)
        {
            return query.Where(s => s.DeleteReason == reason);
        }
    }
}