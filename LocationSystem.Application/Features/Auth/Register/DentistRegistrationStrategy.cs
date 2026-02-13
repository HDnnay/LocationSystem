using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.Register
{
    public class DentistRegistrationStrategy : IUserRegistrationStrategy
    {
        public async Task RegisterUser(User user, LocationSystem.Application.Contrats.Repositories.IUserRepository userRepository)
        {
            // 具体的牙医注册逻辑
            await userRepository.AddAsync(user);
        }
    }
}
