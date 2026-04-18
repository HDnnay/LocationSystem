using LocationSystem.Domain.Entities.DeletedSnapshots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocationSystem.Infrastructure.Configurations
{
    public class DeletedSnapshotConfigration : IEntityTypeConfiguration<DeletedSnapshot>
    {
        public void Configure(EntityTypeBuilder<DeletedSnapshot> builder)
        {
            // 配置 DeletedSnapshot 索引
            // 配置 DeletedSnapshot 索引
            builder.HasIndex(e => e.EntityType)
                .HasDatabaseName("IX_Snapshot_EntityType");

            builder.HasIndex(e => new { e.EntityType, e.EntityId })
                .HasDatabaseName("IX_Snapshot_EntityType_Id");

            builder.HasIndex(e => e.DeletedAt)
                .HasDatabaseName("IX_Snapshot_DeletedAt");
        }
    }
}
