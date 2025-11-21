// EM PilotMaster.Api/Controllers/DashboardController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PilotMaster.Application.DTOs;
using System.Collections.Generic;

namespace PilotMaster.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        // 🔑 ADICIONE O CONSTRUTOR: Se não houver DI, um construtor vazio é necessário para evitar erros de runtime
        public DashboardController() { }

        // 🟢 BK-07: Endpoint: GET /api/dashboard
        [HttpGet]
        [ProducesResponseType(typeof(DashboardMetricsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<DashboardMetricsDto> GetDashboardMetrics()
        {
            var metrics = new DashboardMetricsDto
            {
                Navios = 12,
                Manobras = 8,
                Agentes = 5,
            };

            return Ok(metrics);
        }
    }
}