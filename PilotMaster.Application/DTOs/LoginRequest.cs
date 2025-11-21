using System.ComponentModel.DataAnnotations;
// No PilotMaster.Application.DTOs/LoginRequest.cs

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // ESSENCIAL!

namespace PilotMaster.Application.DTOs
{
    public class LoginRequest
    {
        public string Email { get; set; }
        // 🔑 O NOME AQUI DEVE SER CONSISTENTE COM O QUE VOCÊ ENVIA!
        public string Senha { get; set; }
        // Se você está enviando "password", mude a propriedade para "Password"
    }
}