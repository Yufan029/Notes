using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneToMany
{
    public class LeaveConfig : IEntityTypeConfiguration<Leave>
    {
        public void Configure(EntityTypeBuilder<Leave> builder)
        {
            // User和leave是单项导航的例子，Leave表里有User的导航属性, 不止一个（Request和Approver)，
            // 但是User表里没有任何导航属性。
            // 在设置Leave的时候，要明确表示 WithMany() 没有参数

            builder.ToTable("T_Leaves");
            builder.HasOne<User>(leave => leave.Request).WithMany().IsRequired();
            builder.HasOne<User>(leave => leave.Approver).WithMany();
        }
    }
}
