
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityFramework;

[ApiController]
[Route("api/[controller]/[action]")]
public class AddUserAndRoleController : ControllerBase
{
    private readonly UserManager<MyUser> userManager;
    private readonly RoleManager<MyRole> roleManager;

    public AddUserAndRoleController(UserManager<MyUser> userManager, RoleManager<MyRole> roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    [HttpPost]
    public async Task<IActionResult> AddUserAndRole()
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var role = new MyRole { Name = "Admin" };
            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to create role");
            }
        }

        var userZyf = await userManager.FindByNameAsync("zyf");
        if (userZyf == null)
        {
            var user = new MyUser { UserName = "zyf", Email = "zyf@example.com" };
            var result = await userManager.CreateAsync(user, "password");
            if (!result.Succeeded)
            {
                return BadRequest("Failed to create user");
            }

            await userManager.AddToRoleAsync(user, "Admin");
        }

        return Ok("User and role added successfully");
    }
}