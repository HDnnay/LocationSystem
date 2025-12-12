using LocationSystem.Application.Features.Patients.Queries.GetPatienList;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LocationSystem.Application.Features.Patients.MapperExtensions
{
    public static class PatienListMapper
    {
        public static PatienListDto MapToPatienListDto(this Patient patient)
        {
            return new PatienListDto()
            {
                Id = patient.Id,
                Name = patient.Name,
                Email = patient.Email.Value
            };
        }
    }
}
