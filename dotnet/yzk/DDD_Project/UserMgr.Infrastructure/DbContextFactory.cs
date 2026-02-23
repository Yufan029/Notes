using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using UserMgr.Infrastructure;

public class DbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
        optionsBuilder.UseSqlServer("Server=.;Database=UserMgr;Trusted_Connection=True;TrustServerCertificate=True");

        return new UserDbContext(optionsBuilder.Options);
    }
}