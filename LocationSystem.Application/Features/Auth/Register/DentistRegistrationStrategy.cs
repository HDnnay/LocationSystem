using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Auth.Login;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.Register
{
    public class DentistRegistrationStrategy : IUserRegistrationStrategy
    {
        public async Task RegisterUser(User user, IUserRepository userRepository)
        {
            if (user is Dentist dentist)
            {
                await userRepository.AddDentistAsync(dentist);
            }
            else
            {
                throw new Exception("Invalid user type for dentist registration");
            }
        }
    }
}
