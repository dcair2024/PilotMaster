using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PilotMaster.Application.Interfaces;
using PilotMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PilotMaster.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _jwtKey;

        public TokenService(IConfiguration config)
        {
            _config = config;
            var key = _config["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key not configured");
            _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        // Versão antiga (mantive para compatibilidade)
        public string GenerateAccessToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, usuario.Role ?? string.Empty)
            };

            return CreateToken(claims);
        }

        // Versão para ApplicationUser usado pelo Identity
        public string GenerateAccessToken(ApplicationUser user, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, role ?? string.Empty)
            };

            return CreateToken(claims);
        }

        private string CreateToken(List<Claim> claims)
        {
            var credentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256);

            var expiryMinutes = _config["Jwt:ExpiryMinutes"];
            var minutes = !string.IsNullOrWhiteSpace(expiryMinutes) && double.TryParse(expiryMinutes, out var m)
                ? m
                : 60.0; // fallback

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(minutes),
                SigningCredentials = credentials,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
