using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;
using System;

namespace LocationSystem.Domain.Entities
{
    public class RegularUser : User
    {
        private RegularUser() { }
        public RegularUser(string name, Email email, UserType userType)
            : base(name, email, false)
        {
            UserType = userType;
        }
    }
}