namespace LocationSystem.Domain.Entities.Interfacies
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime CreateTiem { get; set; }
    }
}
