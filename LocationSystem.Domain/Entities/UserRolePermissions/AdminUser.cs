using LocationSystem.Domain.ValueObjects;
using System;

namespace LocationSystem.Domain.Entities.UserRolePermissions
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
