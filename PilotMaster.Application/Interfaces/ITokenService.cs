using PilotMaster.Domain.Entities;

namespace PilotMaster.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
    }
}

