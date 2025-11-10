using System.ComponentModel.DataAnnotations;

namespace PilotMaster.Application.DTOs
{
    // Objeto que a API recebe na requisição de login
    public class LoginRequest
    {
        [Required(ErrorMessage = "O Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de Email inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha é obrigatória.")]
        public string Senha { get; set; } = string.Empty;
    }
}