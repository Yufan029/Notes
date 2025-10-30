using Microsoft.EntityFrameworkCore;

namespace OneToMany
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new MyDbContext())
            {
                /*
                Article article = new Article();
                article.Title = "first article";
                article.Message = "new article, first article, article message";

                Comment c1 = new Comment() { Message = "comment 1" };
                Comment c2 = new Comment() { Message = "comment 2" };
                
                article.Comments.Add(c1);
                article.Comments.Add(c2);

                ctx.Articles.Add(article);
                ctx.SaveChanges();
                */

                var article = ctx.Articles.Include(a => a.Comments).Single(a => a.Id == 1);
                foreach (var c in article.Comments)
                {
                    Console.WriteLine($"{c.Id}: {c.Message}");
                }
            }
        }
    }
}
