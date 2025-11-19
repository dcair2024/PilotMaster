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
    // Implementação da interface ITokenService
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

        // Gera o Token de Acesso (Access Token)
        public string GenerateAccessToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role) // Adiciona a ROLE (Agente/Supervisor) [cite: 29]
            };

            var credentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256);

            // Define o tempo de expiração do Access Token
            var expires = DateTime.UtcNow.AddMinutes(
                double.Parse(_config["Jwt:ExpiryMinutes"]!));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = credentials,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        // Gera o Refresh Token (apenas uma string aleatória por enquanto, para ser salva no banco)
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}