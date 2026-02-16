using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.Register.Commands
{
    public interface IUserRegistrationStrategy
    {
        Task RegisterUser(User user, IUserRepository userRepository);
    }
}
