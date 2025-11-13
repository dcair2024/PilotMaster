using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PilotMaster.Application.DTOs;
using PilotMaster.Application.Interfaces;
using PilotMaster.Infrastructure.Data;
using System.Security.Claims;
using System.Security.Cryptography; // Para usar SHA256
using System.Text;                  // Para usar Encoding.UTF8

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
                // Mensagem genérica para segurança
                return Unauthorized(new { Message = "Email ou senha inválidos." });
            }

            // 2. 🔑 VALIDAÇÃO DA SENHA CORRIGIDA (AGORA COM SHA256)
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.Senha));

            // Converte o array de bytes em string HASH (MAIÚSCULA, sem hífens)
            var senhaHash = BitConverter.ToString(hash).Replace("-", "");

            // Compara o HASH da senha de entrada com o HASH salvo no banco
            if (usuario.SenhaHash != senhaHash)
            {
                return Unauthorized(new { Message = "Email ou senha inválidos." });
            }
            // 🔑 FIM DA VALIDAÇÃO CORRIGIDA

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
                Expiration = securityToken!.ValidTo
            });
        }

        // ... (Os endpoints /test e /refresh continuam os mesmos)
    }
}