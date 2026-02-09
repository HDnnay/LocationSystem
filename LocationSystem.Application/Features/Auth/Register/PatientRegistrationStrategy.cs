using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Auth.Login;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.Register
{
    public class PatientRegistrationStrategy : IUserRegistrationStrategy
    {
        public async Task RegisterUser(User user, IUserRepository userRepository)
        {
            if (user is Patient patient)
            {
                await userRepository.AddPatientAsync(patient);
            }
            else
            {
                throw new Exception("Invalid user type for patient registration");
            }
        }
    }
}
