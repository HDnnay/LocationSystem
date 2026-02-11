using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;

namespace LocationSystem.Domain.Strategies
{
    public interface IUserCreationStrategy
    {
        User CreateUser(string name, Email email, string password);
    }
}
