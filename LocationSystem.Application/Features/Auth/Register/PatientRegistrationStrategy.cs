using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.Register
{
    public class PatientRegistrationStrategy : IUserRegistrationStrategy
    {
        public async Task RegisterUser(User user, LocationSystem.Application.Contrats.Repositories.IUserRepository userRepository)
        {
            // 具体的患者注册逻辑
            await userRepository.AddAsync(user);
        }
    }
}
