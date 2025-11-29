
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace IdentityFramework;

[ApiController]
[Route("api/[controller]/[action]")]
public class AddUserAndRoleController(
    UserManager<MyUser> userManager,
    RoleManager<MyRole> roleManager,
    IWebHostEnvironment env) : ControllerBase
{
    private readonly UserManager<MyUser> userManager = userManager;
    private readonly RoleManager<MyRole> roleManager = roleManager;
    private readonly IWebHostEnvironment env = env;
    private static Dictionary<string, (string identityToken, DateTime expiry)> resetTokens = new();

    [HttpPost]
    public async Task<IActionResult> AddUserAndRole()
    {
        if (!await roleManager.RoleExistsAsync("admin"))
        {
            var role = new MyRole { Name = "admin" };
            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to create role");
            }
        }

        var useryzk = await userManager.FindByNameAsync("yzk");
        if (useryzk == null)
        {
            var user = new MyUser { UserName = "yzk", Email = "yzk@example.com" };
            var result = await userManager.CreateAsync(user, "password");
            if (!result.Succeeded)
            {
                return BadRequest("Failed to create user");
            }

            await userManager.AddToRoleAsync(user, "admin");
        }

        return Ok("User and role added successfully");
    }

    [HttpPost]
    public async Task<IActionResult> CheckUserNameAndPassword(string userName, string password)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return Unauthorized("Invalid username or password");
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            return Unauthorized($"User is locked out, till time: {user.LockoutEnd}");
        }
        
        if (await userManager.CheckPasswordAsync(user, password))
        {
            if (env.IsDevelopment())
            {
                Console.WriteLine($"User {userName} logged in successfully.");
            }
            await userManager.ResetAccessFailedCountAsync(user);
            return Ok("Username and password are correct");
        }
        else
        {
            await userManager.AccessFailedAsync(user);
            if (env.IsDevelopment())
            {
                Console.WriteLine($"Invalid password for user {userName}. Failed attempts: {await userManager.GetAccessFailedCountAsync(user)}");
            }
        }

        return Unauthorized("Invalid username or password");
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string userName)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        // Generate Identity token for actual password reset
        var identityToken = await userManager.GeneratePasswordResetTokenAsync(user);
        System.Console.WriteLine($"Identity token for {userName}: {identityToken}");
        
        // Generate a secure 6-digit token for user convenience
        //var simpleToken = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
        var tokenExpiry = DateTime.UtcNow.AddMinutes(10); // Token valid for 10 minutes
        
        // Store both tokens in dictionary (in production, use database)
        resetTokens[userName] = (identityToken, tokenExpiry);
        
        if (env.IsDevelopment())
        {
            Console.WriteLine($"Password reset token for {userName}: {identityToken}");
        }

        return Ok(new { token = identityToken, message = "Simple 6-digit token generated" });
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(string userName, string token, string newPassword)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        // Verify token exists and is not expired
        if (!resetTokens.ContainsKey(userName))
        {
            return BadRequest("Invalid or expired token");
        }

        var (identityToken, expiry) = resetTokens[userName];
        if (identityToken != token || DateTime.UtcNow > expiry)
        {
            resetTokens.Remove(userName);
            return BadRequest("Invalid or expired token");
        }

        // Use Identity's ResetPasswordAsync with the stored identity token
        var result = await userManager.ResetPasswordAsync(user, identityToken, newPassword);
        if (result.Succeeded)
        {
            resetTokens.Remove(userName); // Remove used token
            return Ok("Password has been reset successfully");
        }

        return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
    }
}