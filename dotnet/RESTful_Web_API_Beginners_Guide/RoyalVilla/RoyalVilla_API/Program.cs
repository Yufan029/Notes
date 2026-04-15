using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using RoyalVilla.DTO;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using RoyalVilla_API.Services;
using Scalar.AspNetCore;
using System.Text;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAutoMapper(o =>
        {
            o.CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            o.CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
            o.CreateMap<Villa, VillaDTO>().ReverseMap();
            o.CreateMap<VillaUpdateDTO, VillaDTO>().ReverseMap();
            o.CreateMap<User, UserDTO>().ReverseMap();
            o.CreateMap<VillaAmenities, VillaAmenitiesDTO>()
                .ForMember(dest => dest.VillaName,
                           opt => opt.MapFrom(src => src.Villa != null ? src.Villa.Name : null));

            o.CreateMap<VillaAmenitiesDTO, VillaAmenities>()
                .ForMember(dest => dest.VillaId,
                           opt => opt.MapFrom(src => src.VillaId));

            o.CreateMap<VillaAmenities, VillaAmenitiesCreateDTO>().ReverseMap();
            o.CreateMap<VillaAmenities, VillaAmenitiesUpdateDTO>().ReverseMap();
            o.CreateMap<VillaAmenitiesDTO, VillaAmenitiesUpdateDTO>().ReverseMap();
        });

        builder.Services.AddScoped<JwtService>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                    )
                };
            });

        builder.Services.AddCors();
        builder.Services.AddAuthorization();

        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cacellationToken) =>
            {
                document.Components ??= new();
                document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                {
                    ["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "Enter JWT Bearer token"
                    }
                };

                document.Security =
                [
                    new OpenApiSecurityRequirement
                    {
                        { new OpenApiSecuritySchemeReference("Bearer"), new List<string>() }
                    }
                ];

                return Task.CompletedTask;
            });
        });

        var app = builder.Build();

        await SeedDataAsync(app);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }
        app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("*"));
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();


        static async Task SeedDataAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();
        }
    }
}