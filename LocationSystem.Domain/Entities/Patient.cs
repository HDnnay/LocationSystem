using LocationSystem.Domain.Exceptions;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Domain.Entities
{
    public class Patient
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        private Patient() { }
        public Patient(string name, Email email)
        {
            ValidatorName(name);

            Name = name;
            Email = email;
            Id = Guid.NewGuid();
        }

        private static void ValidatorName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinessRuleException($"{nameof(name)}的为空");
            }
        }
        private static void ValidatorEmail(Email email)
        {
            if (email is null)
            {
                throw new BussinessRuleException($"{nameof(email)}的为空");
            }
        }

        public void UpdateName(string name) 
        {
            ValidatorName(name);
            Name = name;
        }
        public void UpdateEmail(Email email) 
        {
            ValidatorEmail(email);
            Email = email;
        }
    }
}
