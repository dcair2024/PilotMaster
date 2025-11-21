// EM PilotMaster.Api/Controllers/ManeuverController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PilotMaster.Application.DTOs;
using System.Collections.Generic;

[Route("api/manobras")] // ⬅️ Rota base limpa
[ApiController]
[Authorize]
public class ManobrasController : ControllerBase
{
    public ManobrasController() { }

    // 🟢 BK-08: Endpoint: GET /api/manobras/recentes
    [HttpGet("recentes")]
    [ProducesResponseType(typeof(IEnumerable<ManobraRecenteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<ManobraRecenteDto>> GetRecentManeuvers()
    {
        var manobras = new List<ManobraRecenteDto>
        {
            new ManobraRecenteDto { NomeNavio = "Navio Alpha", Porto = "Porto A", Hora = "14:30", Status = "Concluída" },
            new ManobraRecenteDto { NomeNavio = "Navio Beta", Porto = "Porto B", Hora = "13:15", Status = "Em andamento" },
        };

        return Ok(manobras);
    }
}
