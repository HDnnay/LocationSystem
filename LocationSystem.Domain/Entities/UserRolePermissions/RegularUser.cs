using LocationSystem.Domain.ValueObjects;

namespace LocationSystem.Domain.Entities.UserRolePermissions
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