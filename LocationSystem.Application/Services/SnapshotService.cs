using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.ISevices;
using LocationSystem.Domain.Entities.DeletedSnapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LocationSystem.Application.Services
{
    public class SnapshotService : ISnapshotService
    {
        private readonly IDeletedSnapshotRepository _snapshotRepository;
        private readonly JsonSerializerOptions _jsonOptions;

        public SnapshotService(
            IDeletedSnapshotRepository snapshotRepository)
        {
            _snapshotRepository = snapshotRepository;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNameCaseInsensitive = true,
                WriteIndented = false,
                MaxDepth = 64
            };
        }

        /// <summary>
        /// 删除实体并创建快照
        /// </summary>
        public async Task<DeletedSnapshot> DeleteWithSnapshotAsync<T>(T entity, Func<T, Task> deleteAction, string? reason = null) where T : class
        {
            var entityType = typeof(T);
            var entityId = GetEntityId(entity);
            var displayName = GetEntityDisplayName(entity);

            // 创建快照
            var snapshot = new DeletedSnapshot
            {
                EntityType = entityType.Name,
                AssemblyQualifiedTypeName = entityType.AssemblyQualifiedName ?? entityType.FullName!,
                EntityId = entityId.ToString(),
                EntityDisplayName = displayName,
                SnapshotDataJson = JsonSerializer.Serialize(entity, _jsonOptions),
                DeletedAt = DateTime.UtcNow,
                DeletedBy = GetCurrentUser(),
                DeleteReason = reason,
                MetadataJson = JsonSerializer.Serialize(new
                {
                    DeletedFrom = "SnapshotService",
                    OriginalState = "Deleted"
                }, _jsonOptions)
            };

            // 保存快照
            await _snapshotRepository.AddAsync(snapshot);

            // 删除原实体
            await deleteAction(entity);

            return snapshot;
        }

        /// <summary>
        /// 批量删除并创建快照
        /// </summary>
        public async Task<List<DeletedSnapshot>> DeleteBatchWithSnapshotAsync<T>(IEnumerable<T> entities, Func<T, Task> deleteAction, string? reason = null) where T : class
        {
            var snapshots = new List<DeletedSnapshot>();

            foreach (var entity in entities)
            {
                var snapshot = await DeleteWithSnapshotAsync(entity, deleteAction, reason);
                snapshots.Add(snapshot);
            }

            return snapshots;
        }

        /// <summary>
        /// 从快照恢复实体
        /// </summary>
        public async Task<T?> RestoreFromSnapshotAsync<T>(int snapshotId, Func<T, Task> addAction) where T : class
        {
            var snapshot = await _snapshotRepository.GetByIdAsync(snapshotId);
            if (snapshot == null)
                throw new ArgumentException($"Snapshot with ID {snapshotId} not found");

            // 检查类型匹配
            var targetType = typeof(T);
            if (snapshot.EntityType != targetType.Name)
                throw new InvalidOperationException($"Snapshot type {snapshot.EntityType} does not match requested type {targetType.Name}");

            // 反序列化实体
            var restoredEntity = JsonSerializer.Deserialize<T>(snapshot.SnapshotDataJson, _jsonOptions);
            if (restoredEntity == null)
                throw new InvalidOperationException("Failed to deserialize snapshot data");

            // 添加到数据库
            await addAction(restoredEntity);

            return restoredEntity;
        }

        /// <summary>
        /// 获取实体的删除历史
        /// </summary>
        public async Task<List<DeletedSnapshot>> GetDeleteHistoryAsync<T>(object entityId) where T : class
        {
            Expression<Func<DeletedSnapshot, bool>> predicate = s => 
                s.EntityType == typeof(T).Name && s.EntityId == entityId.ToString();
            
            return await _snapshotRepository.FindAsync(predicate);
        }

        /// <summary>
        /// 加载快照数据为实体
        /// </summary>
        public T? LoadSnapshotAs<T>(DeletedSnapshot snapshot) where T : class
        {
            return JsonSerializer.Deserialize<T>(snapshot.SnapshotDataJson, _jsonOptions);
        }

        /// <summary>
        /// 动态加载快照数据
        /// </summary>
        public object? LoadSnapshotDynamic(DeletedSnapshot snapshot)
        {
            var targetType = Type.GetType(snapshot.AssemblyQualifiedTypeName);
            if (targetType == null)
                throw new InvalidOperationException($"Cannot load type: {snapshot.AssemblyQualifiedTypeName}");

            return JsonSerializer.Deserialize(snapshot.SnapshotDataJson, targetType, _jsonOptions);
        }

        /// <summary>
        /// 获取实体ID（支持不同类型的主键）
        /// </summary>
        private object GetEntityId<T>(T entity) where T : class
        {
            // 尝试获取 Id 属性
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null)
                return idProperty.GetValue(entity) ?? 0;

            // 尝试获取其他常见主键名
            var keyProperty = typeof(T).GetProperty("Id") ??
                             typeof(T).GetProperty($"{typeof(T).Name}Id") ??
                             typeof(T).GetProperties().FirstOrDefault(p => p.Name.EndsWith("Id"));

            if (keyProperty != null)
                return keyProperty.GetValue(entity) ?? 0;

            return entity.GetHashCode(); // fallback
        }

        /// <summary>
        /// 获取实体显示名称
        /// </summary>
        private string? GetEntityDisplayName<T>(T entity) where T : class
        {
            // 尝试获取 Name 属性
            var nameProperty = typeof(T).GetProperty("Name") ??
                              typeof(T).GetProperty("Title") ??
                              typeof(T).GetProperty("DisplayName") ??
                              typeof(T).GetProperty("FullName");

            if (nameProperty != null)
                return nameProperty.GetValue(entity)?.ToString();

            return null;
        }

        /// <summary>
        /// 获取当前用户（可根据实际情况实现）
        /// </summary>
        private string GetCurrentUser()
        {
            // 这里可以从 HttpContext、IHttpContextAccessor 或 Thread.CurrentPrincipal 获取
            return System.Environment.UserName ?? "System";
        }
    }

}
