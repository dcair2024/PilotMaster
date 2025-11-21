using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PilotMaster.Domain.Entities; // 🔑 CORRIGIDO: Necessário para encontrar a classe 'Usuario'

namespace PilotMaster.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(Usuario usuario);
        string GenerateRefreshToken();
        // 🔑 Use ApplicationUser para ser consistente com o AddIdentity
        string GenerateAccessToken(ApplicationUser user, string role); 
    }
}