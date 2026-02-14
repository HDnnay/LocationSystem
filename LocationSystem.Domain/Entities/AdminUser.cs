using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;
using System;

namespace LocationSystem.Domain.Entities
{
    public class AdminUser : User
    {
        private AdminUser() { }
        public AdminUser(string name, Email email, UserType userType)
            : base(name, email, userType == UserType.Admin)
        {
            UserType = userType;
        }

        public AdminUser(string name, Email email, UserType userType, bool isSuperAdmin)
            : base(name, email, isSuperAdmin)
        {
            UserType = userType;
        }
    }
}
