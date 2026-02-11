using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.Register
{
    public interface IUserRegistrationStrategy
    {
        Task RegisterUser(User user, LocationSystem.Application.Contrats.Repositories.IUserRepository userRepository);
    }
}
