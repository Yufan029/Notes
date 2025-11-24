using System;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

// Class parameter added since it's being added in API project, programs.cs file, services.AddDbContext<AppDbContext>
public class AppDbContext(DbContextOptions opt) : DbContext(opt)
{
    public required DbSet<Activity> Activities { get; set; }
}
