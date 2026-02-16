using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.Register
{
    public class UserRegistrationStrategyFactory
    {
        private readonly Dictionary<UserType, IUserRegistrationStrategy> _strategies;

        public UserRegistrationStrategyFactory()
        {
            _strategies = new Dictionary<UserType, IUserRegistrationStrategy>
            {
                
            };
        }

        public IUserRegistrationStrategy GetStrategy(UserType userType)
        {
            if (_strategies.TryGetValue(userType, out var strategy))
            {
                return strategy;
            }
            throw new Exception("Unsupported user type");
        }
    }
}
