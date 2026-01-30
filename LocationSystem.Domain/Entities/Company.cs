using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Domain.Entities
{
    public class Company
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

    }
}
