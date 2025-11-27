using IdentityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDataProtection();   // 数据保护
builder.Services.AddIdentityCore<MyUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;
});

var identityBuilder = new IdentityBuilder(typeof(MyUser), typeof(MyRole), builder.Services);
identityBuilder.AddEntityFrameworkStores<MyDbContext>()
            .AddDefaultTokenProviders()
            .AddUserManager<UserManager<MyUser>>()
            .AddRoleManager<RoleManager<MyRole>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
