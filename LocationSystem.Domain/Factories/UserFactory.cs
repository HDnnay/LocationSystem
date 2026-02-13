using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;
using System;

namespace LocationSystem.Domain.Factories
{
    public class UserFactory
    {
        public User CreateUser(UserType userType, string name, string email, string password)
        {
            var emailValue = new Email(email);
            var user = new DefaultUser(name, emailValue, userType);
            user.SetPasswordHash(password);
            return user;
        }
    }
}
