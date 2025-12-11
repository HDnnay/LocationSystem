using LocationSystem.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Domain.Entities
{
    public class DentalOffice
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public DentalOffice(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new BussinessRuleException($"{nameof(name)}的为空");
            }
            Name = name;
            Id = Guid.NewGuid();
        }
    }
}
