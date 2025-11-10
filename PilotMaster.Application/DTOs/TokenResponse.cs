namespace PilotMaster.Application.DTOs
{
    // Objeto que a API retorna após um login bem-sucedido
    public class TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}