using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PilotMaster.Domain.Entities;
using PilotMaster.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string email, string password)
    {
        var user = new ApplicationUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("Usuário criado!");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return Unauthorized();

        var valid = await _userManager.CheckPasswordAsync(user, password);
        if (!valid) return Unauthorized();

        var token = _tokenService.GenerateToken(user);

        return Ok(new { token });
    }
}
