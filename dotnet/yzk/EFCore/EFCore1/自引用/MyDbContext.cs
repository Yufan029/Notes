using Microsoft.EntityFrameworkCore;
using 自引用;

namespace OneToMany
{
    public class MyDbContext : DbContext
    {
        public DbSet<OrgUnit> OrgUnits { get; set; }
     
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.;Database=demo2;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True");
            //optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
