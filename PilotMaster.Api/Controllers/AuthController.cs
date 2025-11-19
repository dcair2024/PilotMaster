using Microsoft.AspNetCore.Mvc;
using PilotMaster.Application.DTOs;
using PilotMaster.Application.Interfaces;
using PilotMaster.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace PilotMaster.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Unauthorized(new { Message = "Email ou senha inválidos." });

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                request.Senha,
                lockoutOnFailure: false);

            if (!result.Succeeded)
                return Unauthorized(new { Message = "Email ou senha inválidos." });

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Agente";

            var accessToken = _tokenService.GenerateAccessToken(user, role);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;

            return Ok(new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserRole = role,
                Expiration = securityToken!.ValidTo
            });
        }
    }
}
