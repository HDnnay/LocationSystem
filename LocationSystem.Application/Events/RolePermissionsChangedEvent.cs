using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Events
{
    public class RolePermissionsChangedEvent
    {
        public Guid RoleId { get; set; }
    }
}