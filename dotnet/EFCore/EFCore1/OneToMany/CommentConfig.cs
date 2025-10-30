using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneToMany
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("T_Comments");
            builder.Property(x => x.Message).IsUnicode().IsRequired();

            // 双向导航， Article有到Comment的导航属性，Comment也有到Article的导航属性
            
            // 也可以配置在Article中，配置在多的一端，或者一的一端，都一样
            builder.HasOne<Article>(comment => comment.Article)
                .WithMany(article => article.Comments)
                .HasForeignKey(c => c.ArticleId)
                .IsRequired();
        }
    }
}
