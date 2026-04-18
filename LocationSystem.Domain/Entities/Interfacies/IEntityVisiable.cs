namespace LocationSystem.Domain.Entities.Interfacies
{
    public interface IEntityVisiable : IEntity
    {
        bool IsDisabled { get; set; }
    }
}
