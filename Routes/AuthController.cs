using BlogMinimalApi.Models;
using BlogMinimalApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogMinimalApi.Routes;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Register model)
    {
        var (Success, Message) = await _authService.Register(model);
        if (Success) return Ok(new { Message });
        return BadRequest(new { Message });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Login model)
    {
        var (Success, Message, Token) = await _authService.Login(model);
        if (Success) return Ok(new { Token });
        return BadRequest(new { Message });
    }

    [HttpPost("add-role")]
    public async Task<IActionResult> AddRole([FromBody] string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(role));
            if (result.Succeeded) return Ok(new { message = "Role added successfully" });
            return BadRequest(result.Errors);
        }
        return BadRequest("Role already exists");
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] UserRole model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null) return BadRequest("User not found");

        var result = await _userManager.AddToRoleAsync(user, model.Role);
        if (result.Succeeded) return Ok(new { message = "Role assigned successfully" });
        return BadRequest(result.Errors);
    }
}
