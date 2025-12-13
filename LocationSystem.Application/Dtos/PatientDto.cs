using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LocationSystem.Application.Dtos
{
    public class PatientDto
    {
        public Guid Id { get;set; }
        public string Name { get;set; } = null!;
        public string Email { get;set; } = null!;
        public PatientDto(Patient model)
        {
            Id = model.Id;
            Name = model.Name;
            Email = model.Email.Value;
        }
    }

}
