using System.ComponentModel.DataAnnotations;
// No PilotMaster.Application.DTOs/LoginRequest.cs

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // ESSENCIAL!

namespace PilotMaster.Application.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "O Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de Email inválido.")]
        [JsonPropertyName("email")] // Mapeia o JSON 'email' para o C# 'Email'
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha é obrigatória.")]
        [JsonPropertyName("password")] // Mapeia o JSON 'password' para o C# 'Senha'
        public string Senha { get; set; } = string.Empty;
    }
}