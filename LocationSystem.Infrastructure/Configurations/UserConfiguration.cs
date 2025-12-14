using LocationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
            builder.ComplexProperty(t => t.Email, action =>
            {
                action.Property(e => e.Value).HasColumnName("Email").HasMaxLength(50);
            });
        }
    }
}
