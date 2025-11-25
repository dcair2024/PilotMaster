using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PilotMaster.Application.Interfaces;
using PilotMaster.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PilotMaster.Application.Services
{
    // A classe agora implementa todos os métodos de ITokenService
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _jwtKey;

        public TokenService(IConfiguration config)
        {
            _config = config;
            // Cria a chave criptográfica a partir da string no appsettings.json
            _jwtKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        }

        // 🔑 1. OBRIGATÓRIO: Implementação para Login (ApplicationUser)
        // Este método será usado pelo AuthController.
        public string GenerateAccessToken(ApplicationUser user, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role)
            };

            // Reutiliza a lógica comum de criação de token
            return CreateToken(claims);
        }

        // 🔑 2. OBRIGATÓRIO: Implementação para o tipo Usuario (versão antiga)
        // Se você não usa mais a classe Usuario, pode deixar esta implementação simples
        // para satisfazer a interface.
        public string GenerateAccessToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role)
            };

            return CreateToken(claims);
        }

        // 🔑 3. OBRIGATÓRIO: Implementação do Refresh Token
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        // 🔑 Método Auxiliar para criar o JWT (reúne a lógica de expiração, etc.)
        private string CreateToken(List<Claim> claims)
        {
            var credentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256);

            // Tenta obter o tempo de expiração de Jwt:ExpiryMinutes, se existir
            if (!double.TryParse(_config["Jwt:ExpiryMinutes"], out double expiryMinutes))
            {
                expiryMinutes = 120; // Padrão de 2 horas se a chave não for encontrada
            }

            var expires = DateTime.UtcNow.AddHours(8);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = credentials,
                // Assumindo que Issuer/Audience estão configurados no appsettings.json
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}