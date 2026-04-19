namespace LocationSystem.Application.Dtos.DeletedSnapshots
{
    public class DeletedSnapshotDto
    {
        public Guid Id { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string? EntityDisplayName { get; set; }
        public DateTime DeletedAt { get; set; }
        public string? SnapshotDataJson { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeleteReason { get; set; }
    }
}