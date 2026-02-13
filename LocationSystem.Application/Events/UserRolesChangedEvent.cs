using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Events
{
    public class UserRolesChangedEvent
    {
        public Guid UserId { get; set; }
    }
}