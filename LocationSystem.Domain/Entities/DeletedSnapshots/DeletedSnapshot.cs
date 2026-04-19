using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocationSystem.Domain.Entities.DeletedSnapshots
{
    /// <summary>
    /// 删除快照实体
    /// </summary>
    public class DeletedSnapshot
    {
        public DeletedSnapshot()
        {
            DeletedAt = DateTime.Now;
        }
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 实体类型名称（如 "Order", "User", "Product"）
        /// </summary>
        [MaxLength(100)]
        public string EntityType { get; set; } = string.Empty;

        /// <summary>
        /// 程序集限定类型名称（用于反射加载）
        /// </summary>
        [MaxLength(500)]
        public string AssemblyQualifiedTypeName { get; set; } = string.Empty;

        /// <summary>
        /// 被删除实体的ID（字符串化，便于查询）
        /// </summary>
        [MaxLength(100)]
        public string EntityId { get; set; } = string.Empty;

        /// <summary>
        /// 实体名称/标题（便于展示）
        /// </summary>
        [MaxLength(200)]
        public string? EntityDisplayName { get; set; }

        /// <summary>
        /// 快照数据（JSON格式）
        /// </summary>
        [Column(TypeName = "nvarchar(max)")]
        public string SnapshotDataJson { get; set; } = string.Empty;

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime DeletedAt { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        [MaxLength(100)]
        public string? DeletedBy { get; set; }

        /// <summary>
        /// 删除原因
        /// </summary>
        [MaxLength(500)]
        public string? DeleteReason { get; set; }

        /// <summary>
        /// 额外元数据（JSON格式，可选）
        /// </summary>
        [Column(TypeName = "nvarchar(max)")]
        public string? MetadataJson { get; set; }
    }
}
