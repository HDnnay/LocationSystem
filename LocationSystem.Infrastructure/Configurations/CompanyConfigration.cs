using LocationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Configurations
{
    public class CompanyConfigration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(p => p.Province)
                 .IsConcurrencyToken();
        }
    }
}
