namespace LocationSystem.Domain.Entities.Interfacies
{
    public interface IEntity
    {
        Guid Id { get; }
        DateTime CreateTime { get; }
    }
}
