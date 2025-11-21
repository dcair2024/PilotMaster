// EM PilotMaster.Api/Controllers/ShipController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PilotMaster.Application.DTOs;
using System.Collections.Generic;

[Route("api/navios")] // ⬅️ Rota base limpa
[ApiController]
[Authorize]
public class NaviosController : ControllerBase
{
    public NaviosController() { }

    // 🟢 BK-09: Endpoint: GET /api/navios/ativos
    [HttpGet("ativos")]
    [ProducesResponseType(typeof(IEnumerable<NavioAtivoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<NavioAtivoDto>> GetActiveShips()
    {
        var navios = new List<NavioAtivoDto>
        {
            new NavioAtivoDto { Nome = "Navio Alpha (IMO: 987)", PortoAtual = "Porto A", TempoNoPorto = "12h 45m" },
            new NavioAtivoDto { Nome = "Navio Beta (IMO: 123)", PortoAtual = "Em Manobra", TempoNoPorto = "2h 10m" },
        };

        return Ok(navios);
    }
}