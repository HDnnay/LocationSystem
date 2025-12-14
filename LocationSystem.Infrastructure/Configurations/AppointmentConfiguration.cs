using LocationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Configurations
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
            // 修改 Patient 关系的删除行为
            builder.HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.NoAction); // 改为 NoAction

            // 修改 Dentist 关系的删除行为
            builder.HasOne(a => a.Dentist)
                .WithMany()
                .HasForeignKey(a => a.DentistId)
                .OnDelete(DeleteBehavior.NoAction); // 改为 NoAction
            builder.HasOne(a => a.DentalOffice)
            .WithMany()
            .HasForeignKey(a => a.DentalOfficeId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
