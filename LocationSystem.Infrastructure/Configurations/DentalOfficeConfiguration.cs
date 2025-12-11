using LocationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Configurations
{
    public class DentalOfficeConfiguration : IEntityTypeConfiguration<DentalOffice>
    {
        public void Configure(EntityTypeBuilder<DentalOffice> builder)
        {
            builder.Property(t => t.Name).HasMaxLength(150).IsRequired();
        }
    }
}
