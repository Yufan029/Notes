using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserMgr.Domain.Entities;

namespace UserMgr.Infrastructure.Configs;

public class UserLoginHistoryConfig : IEntityTypeConfiguration<UserLoginHistory>
{
    public void Configure(EntityTypeBuilder<UserLoginHistory> builder)
    {
        builder.ToTable("T_UserLoginHistories");

        // UserLoginHistory 里有一个nullable的 UserId 字段, 不需要建立一对一或一对多关系
        // 因为 UserLoginHistory 和 User 是两个独立的聚合，没有强关系，有可能以后会拆分到不同的微服务，不同数据库

        // 聚合内 外键保留，聚合之间没有不需要导航属性（外键）

        builder.OwnsOne(x => x.PhoneNumber, nb =>
        {
            nb.Property(p => p.RegionNumber).HasMaxLength(5).IsUnicode(false);
            nb.Property(p => p.Number).HasMaxLength(20).IsUnicode(false);
        });
    }
}