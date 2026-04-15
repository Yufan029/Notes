using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoyalVilla.DTO;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;

namespace RoyalVilla_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly JwtService jwtService;

        public AuthService(ApplicationDbContext dbContext, IConfiguration configuration, IMapper mapper, JwtService jwtService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.jwtService = jwtService;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await dbContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower());
                if (user == null || user.Password != loginRequestDTO.Password)
                {
                    return null;
                }

                var token = jwtService.GenerateToken(user);

                return new LoginResponseDTO
                {
                    UserDTO = this.mapper.Map<UserDTO>(user),
                    Token = token
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while logging in: {ex.Message}", ex);
            }
        }

        public async Task<UserDTO?> RegisterAsync(RegisterationRequestDTO registerationRequestDTO)
        {
            try
            {
                if (await this.IsEmailExistsAsync(registerationRequestDTO.Email))
                {
                    throw new InvalidOperationException($"User with email '{registerationRequestDTO.Email}' already exists.");
                }

                User user = new()
                {
                    Email = registerationRequestDTO.Email,
                    Name = registerationRequestDTO.Name,
                    Password = registerationRequestDTO.Password,
                    Role = string.IsNullOrEmpty(registerationRequestDTO.Role) ? "Customer" : registerationRequestDTO.Role,
                    CreateDate = DateTime.UtcNow,
                };

                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();

                return this.mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while registering the user: {ex.Message}", ex);
            }
        }
    }
}
