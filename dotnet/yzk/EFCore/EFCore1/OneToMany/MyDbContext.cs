using Microsoft.EntityFrameworkCore;

namespace OneToMany
{
    public class MyDbContext : DbContext
    {
        // 双向导航， Article有到Comment的导航属性，Comment也有到Article的导航属性
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }

        // User和leave是单向导航的例子，Leave表里有User的导航属性, 不止一个（Request和Approver)，
        // 但是User表里没有任何导航属性。
        // 在设置Leave的时候，要明确表示 WithMany() 没有参数
        public DbSet<User> Users { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.;Database=demo1;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True");
            optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
