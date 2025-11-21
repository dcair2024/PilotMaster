using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PilotMaster.Application.DTOs; // 🔑 NECESSÁRIO para LoginRequest/TokenResponse
using PilotMaster.Application.Interfaces;
using PilotMaster.Domain.Entities;
using System.IdentityModel.Tokens.Jwt; // 🔑 NECESSÁRIO para ler o token
using System.Security.Claims;
using LoginRequest = Microsoft.AspNetCore.Identity.Data.LoginRequest; // 🔑 NECESSÁRIO para o Token

// Define a rota base como api/auth, e não api/AuthController
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager; // 🔑 ADICIONADO para login
    private readonly ITokenService _tokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, // 🔑 ADICIONADO
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager; // 🔑 ADICIONADO
        _tokenService = tokenService;
    }

    // ----------------------------------------------------
    // ROTAS DE LOGIN
    // ----------------------------------------------------

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
    {
        // 1. Busca o usuário
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return Unauthorized(new { Message = "Email ou senha inválidos." });

        // 2. Validação da senha usando SignInManager (mais adequado)
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
            return Unauthorized(new { Message = "Email ou senha inválidos." });

        // 🔑 3. OBTÉM A ROLE (Resolve o erro CS0103 'role')
        var roles = await _userManager.GetRolesAsync(user);
        string role = roles.FirstOrDefault() ?? "Agente";

        // 4. GERA OS TOKENS
        var accessToken = _tokenService.GenerateAccessToken(user, role);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // 5. LÊ O TOKEN PARA OBTER DATA DE EXPIRAÇÃO
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;

        // 6. Retorna o TokenResponse (Resolve o erro CS0103 'token' ou 'accessToken' no retorno)
        return Ok(new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UserRole = role,
            Expiration = securityToken!.ValidTo
        });
    }

    // ----------------------------------------------------
    // ROTAS DE REGISTRO (Ajustada para usar DTO)
    // ----------------------------------------------------

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("Usuário criado!");
    }
}