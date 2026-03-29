namespace LocationSystem.Domain.Entities.Interfacies
{
    public interface IEntity
    {
        Guid Id { get; }
        DateTime CreateTiem { get; }
    }
}
