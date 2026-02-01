using LocationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Configurations
{
    public class DentistConfiguration : IEntityTypeConfiguration<Dentist>
    {
        public void Configure(EntityTypeBuilder<Dentist> builder)
        {
            //builder.Property(t=>t.Name).HasMaxLength(50).IsRequired();
            //builder.ComplexProperty(t => t.Email, action =>
            //{
            //    action.Property(e=>e.MonthlyRent).HasColumnName("Email").HasMaxLength(50);
            //});
        }
    }
}
