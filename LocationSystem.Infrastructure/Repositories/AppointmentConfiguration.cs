using LocationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Repositories
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ComplexProperty(t => t.TimeInterval, action =>
            {
                action.Property(t=>t.Start).HasColumnName("StartDate");
                action.Property(t=>t.End).HasColumnName("EndDate");
            });
        }
    }
}
