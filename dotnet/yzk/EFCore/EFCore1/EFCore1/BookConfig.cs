using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore1
{
    // 配置类，entity to table name mapping.
    // 可以不配置，EFCore会根据约定就命名数据库里的表为entity的名字
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("T_Books");
            builder.Property(book => book.Title).HasMaxLength(50).IsRequired();
            builder.Property(book => book.AuthorName).HasMaxLength(20).IsRequired();
            builder.Ignore(book => book.IgnoreTest);

            builder.Property(book => book.Name2).HasColumnName("NameTwo").HasColumnType("varchar(8)").HasMaxLength(20);

            builder.HasIndex(b => b.Title);
            builder.HasIndex(b => new { b.Name2, b.AuthorName });
        }
    }
}
