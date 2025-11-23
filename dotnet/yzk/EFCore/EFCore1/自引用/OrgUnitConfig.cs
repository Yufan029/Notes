using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 自引用
{
    public class OrgUnitConfig : IEntityTypeConfiguration<OrgUnit>
    {
        public void Configure(EntityTypeBuilder<OrgUnit> builder)
        {
            builder.ToTable("T_OrgUnits");
            builder.Property(o => o.Name).IsUnicode().IsRequired().HasMaxLength(50);
            builder.HasOne<OrgUnit>(o => o.Parent).WithMany(o => o.Children); // root node can be null.
        }
    }
}
