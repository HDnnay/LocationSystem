using LocationSystem.Domain.Entities.UserRolePermissions;
using LocationSystem.Domain.ValueObjects;

namespace LocationSystem.Domain.Strategies
{
    public interface IUserCreationStrategy
    {
        User CreateUser(string name, Email email, string password);
    }
}
