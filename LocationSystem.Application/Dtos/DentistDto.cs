using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Dtos
{
    public class DentistDto
    {
        public DentistDto(Dentist dentist)
        {
            Id = dentist.Id;
            Name = dentist.Name;
            Email = dentist.Email.Value;
        }

        public Guid Id { get;set; }
        public string Name { get;set; } = null!;
        public string Email { get;set; } = null!;
    }
}
