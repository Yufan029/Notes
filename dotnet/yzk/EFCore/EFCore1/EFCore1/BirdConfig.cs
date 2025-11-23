using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore1
{
    public class BirdConfig : IEntityTypeConfiguration<Bird>
    {
        public void Configure(EntityTypeBuilder<Bird> builder)
        {
            builder.HasKey(x => x.Number);
            builder.Property(b => b.Name).HasDefaultValue("xBird");    // recommend, type-safe
            builder.Property("Name").HasDefaultValue("yBird");
        }
    }
}
