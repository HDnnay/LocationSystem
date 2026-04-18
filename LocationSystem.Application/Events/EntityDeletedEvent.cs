using System;

namespace LocationSystem.Application.Events
{
    public class EntityDeletedEvent
    {
        public string EntityType { get; set; } = string.Empty;
        public Guid EntityId { get; set; }
        public string EntityJson { get; set; } = string.Empty;
        public DateTime DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeleteReason { get; set; }
    }
}