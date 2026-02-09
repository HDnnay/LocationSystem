using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Auth.Login;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.Register
{
    public interface IUserRegistrationStrategy
    {
        Task RegisterUser(User user, IUserRepository userRepository);
    }
}
