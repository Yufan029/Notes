using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserMgr.Domain.Entities;

namespace UserMgr.Infrastructure.Configs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("T_Users");

        // PhoneNumber is a Value Object(值对象)， so we use OwnsOne to configure it.
        builder.OwnsOne(x => x.PhoneNumber, nb =>
        {
            nb.Property(p => p.RegionNumber).HasMaxLength(5).IsUnicode(false);
            nb.Property(p => p.Number).HasMaxLength(20).IsUnicode(false);
        });

        // 一对一关系配置
        builder.HasOne(b => b.UserAccessFail)
            .WithOne(uf => uf.User)
            .HasForeignKey<UserAccessFail>(uf => uf.UserId);

        // 私有成员变量，EF Core 如何实现充血模型
        builder.Property("passwordHash").HasMaxLength(100).IsUnicode(false);
    }
}