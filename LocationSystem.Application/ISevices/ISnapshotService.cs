using LocationSystem.Application.Dtos.DeletedSnapshots;
using LocationSystem.Domain.Entities.DeletedSnapshots;
using System.Linq.Expressions;

namespace LocationSystem.Application.ISevices
{
    /// <summary>
    /// 快照服务接口
    /// </summary>
    public interface ISnapshotService
    {
        /// <summary>
        /// 删除实体并创建快照
        /// </summary>
        Task<DeletedSnapshot> DeleteWithSnapshotAsync<T>(T entity, Func<T, Task> deleteAction, string? reason = null) where T : class;

        /// <summary>
        /// 批量删除并创建快照
        /// </summary>
        Task<List<DeletedSnapshot>> DeleteBatchWithSnapshotAsync<T>(IEnumerable<T> entities, Func<T, Task> deleteAction, string? reason = null) where T : class;

        /// <summary>
        /// 从快照恢复实体
        /// </summary>
        Task<T?> RestoreFromSnapshotAsync<T>(int snapshotId, Func<T, Task> addAction) where T : class;

        /// <summary>
        /// 获取实体的删除历史
        /// </summary>
        Task<List<DeletedSnapshot>> GetDeleteHistoryAsync<T>(object entityId) where T : class;

        /// <summary>
        /// 加载快照数据为实体
        /// </summary>
        T? LoadSnapshotAs<T>(DeletedSnapshot snapshot) where T : class;

        /// <summary>
        /// 动态加载快照数据（不知道具体类型）
        /// </summary>
        object? LoadSnapshotDynamic(DeletedSnapshot snapshot);

        /// <summary>
        /// 获取所有删除快照（分页）
        /// </summary>
        Task<(int, IEnumerable<DeletedSnapshotDto>)> GetAllSnapshotsAsync(int page = 1, int pageSize = 10, Expression<Func<DeletedSnapshot, bool>>? predicate = null);
    }
}
