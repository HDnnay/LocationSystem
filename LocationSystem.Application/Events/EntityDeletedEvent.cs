using System.Text.Json;

namespace LocationSystem.Application.Events
{
    // 非泛型基类
    public class EntityDeletedEvent
    {
        public string EntityType { get; set; } = string.Empty;
        public string AssemblyQualifiedTypeName { get; set; } = string.Empty;
        public object EntityId { get; set; }
        public string EntityJson { get; set; } = string.Empty;
        public DateTime DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeleteReason { get; set; }
    }

    // 泛型事件
    public class EntityDeletedEvent<T> : EntityDeletedEvent
    {
        public T? Entity { get; set; }

        public static EntityDeletedEvent<T> Create(T entity, object entityId, string? deletedBy = null, string? deleteReason = null)
        {
            return new EntityDeletedEvent<T>
            {
                EntityType = typeof(T).Name,
                AssemblyQualifiedTypeName = typeof(T).AssemblyQualifiedName!,
                EntityId = entityId,
                Entity = entity,
                EntityJson = JsonSerializer.Serialize(entity),
                DeletedAt = DateTime.Now,
                DeletedBy = deletedBy,
                DeleteReason = deleteReason
            };
        }
    }
}