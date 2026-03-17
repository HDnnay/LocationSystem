using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public bool IsDisabled { get; set; }
        public List<RoleDto> Roles { get; set; }
    }
}
