using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;
using System;

namespace LocationSystem.Domain.Entities
{
    public class DefaultUser : User
    {
        private DefaultUser() { }
        public DefaultUser(string name, Email email, UserType userType)
            : base(name, email)
        {
            UserType = userType;
        }
    }
}
