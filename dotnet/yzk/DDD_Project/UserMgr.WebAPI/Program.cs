using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserMgr.Infrastructure;
using UserMgr.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer("Server=localhost;Database=UserMgrDb;Trusted_Connection=True;");
});

builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add<UnitOfWorkFilter>();
});

// Add controller support
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map controllers (uses controllers in Controllers/)
app.MapControllers();

app.Run();
