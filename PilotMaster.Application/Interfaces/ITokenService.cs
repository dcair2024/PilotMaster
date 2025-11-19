using PilotMaster.Domain.Entities;

namespace PilotMaster.Application.Interfaces
{
    public interface ITokenService
    {
        // Versão antiga (se você ainda usar Usuario)
        string GenerateAccessToken(Usuario usuario);

        // Versão para Identity / ApplicationUser
        string GenerateAccessToken(ApplicationUser user, string role);

        // Refresh token
        string GenerateRefreshToken();
    }
}
