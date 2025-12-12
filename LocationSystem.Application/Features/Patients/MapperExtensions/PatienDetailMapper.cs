using LocationSystem.Application.Features.Patients.Queries.GetPatienDetail;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.MapperExtensions
{
    public static class PatienDetailMapper
    {
        public static PatienDetailDto MapToPatienDetailDto(this Patient patien)
        {
            return new PatienDetailDto()
            {
                Id = patien.Id,
                Name = patien.Name,
                Email = patien.Email.Value
            };
        }
    }
}
