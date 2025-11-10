using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PilotMaster.Domain.Entities;

namespace PilotMaster.Application.Interfaces
{
    // Define o contrato para o serviço de Token
    public interface ITokenService
    {
        // Gera o token de acesso principal
        string GenerateAccessToken(Usuario usuario);

        // Gera o token de refresh (para obter novos Access Tokens)
        string GenerateRefreshToken();
    }
}
