
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
}