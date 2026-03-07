using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Events
{
    public class MenuChangedEvent
    {
        public ActionType ActionType { get; set; }
        public Guid? UserId { get; set; }
    }
    public enum ActionType
    {
        Create,
        Update,
        Delete
    }
}
