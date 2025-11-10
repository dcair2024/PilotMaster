using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PilotMaster.Application.DTOs;
using PilotMaster.Application.Interfaces;
using PilotMaster.Infrastructure.Data;
using System.Security.Claims;

namespace PilotMaster.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        // Construtor para Injeção de Dependência
        public AuthController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
        {
            // 1. Busca o usuário pelo email
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null)
            {
                return Unauthorized(new { Message = "Email ou senha inválidos." });
            }

            // 2. Valida a senha (USANDO LÓGICA SIMPLES POR ENQUANTO)
            // IMPORTANTE: Em produção, você usará uma biblioteca de hashing (como BCrypt ou Argon2)
            // Por enquanto, vamos simular:
            if (usuario.SenhaHash != request.Senha)
            {
                return Unauthorized(new { Message = "Email ou senha inválidos." });
            }

            // 3. Gera os Tokens
            var accessToken = _tokenService.GenerateAccessToken(usuario);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // 4. Salvar o Refresh Token no banco (Lógica de segurança futura)
            // Por enquanto, apenas retornamos.

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(accessToken) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

            return Ok(new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserRole = usuario.Role,
                Expiration = securityToken!.ValidTo // Pega a data de expiração do token
            });
        }

        // 🎯 Endpoint: GET /api/auth/test (Rota Protegida) [cite: 35]
        [HttpGet("test")]
        [Authorize(Roles = "Agente,Supervisor")] // Apenas usuários logados com essas roles acessam
        public ActionResult<string> TestProtectedEndpoint()
        {
            // Acesso às Claims do usuário logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            return Ok($"Endpoint Protegido OK. Usuário {userId} ({userEmail}) logado com a Role: {userRole}.");
        }

        // 🎯 Endpoint: POST /api/auth/refresh (Para Implementação Posterior) [cite: 34]
        // Esta rota exige a validação do Refresh Token no banco, que é mais complexa.
        [HttpPost("refresh")]
        public ActionResult RefreshToken([FromBody] TokenResponse tokens)
        {
            // Retorna um placeholder, pois a lógica exige salvar o Refresh Token no banco (futuro)
            return StatusCode(501, "O endpoint /auth/refresh está temporariamente indisponível na Sprint 1 (Foco: Login/Auth Base).");
        }
    }
}