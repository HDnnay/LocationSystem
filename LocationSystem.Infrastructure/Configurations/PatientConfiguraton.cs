using LocationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Configurations
{
    public class PatientConfiguraton : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.Property(p=>p.Name).HasMaxLength(20).IsRequired();
            builder.ComplexProperty(p => p.Email, action =>
            {
                action.Property(e=>e.Value).HasColumnName("Email").HasMaxLength(254);
            });
        }
    }
}
