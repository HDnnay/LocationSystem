namespace LocationSystem.Domain.Entities.Interfacies
{
    public interface ISoftDeleteEntity : IEntity, IEntityVisiable
    {
        bool IsDelete { get; set; }
        Guid DeleteUserId { get; set; }
        DateTime DeleteTime { get; set; }
    }
}
