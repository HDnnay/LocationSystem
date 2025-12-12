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
            ValidatorName(name);
            Name = name;
            Id = Guid.NewGuid();
        }

        private static void ValidatorName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinessRuleException($"{nameof(name)}的为空");
            }
        }

        public void UpdateName(string name)
        {
            if(string.IsNullOrWhiteSpace(name)) { throw new BussinessRuleException("Name名字不能为空！"); }
            Name = name;
        }
    }
}
