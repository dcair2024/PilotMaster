using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PilotMaster.Application.DTOs;
using System.Collections.Generic; // Necessário para IEnumerable

namespace PilotMaster.Api.Controllers
{
    // Rota base: /api/dashboard
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        // Construtor (assumo que você tem o construtor vazio ou com DI aqui)

        // 🟢 BK-07: Endpoint: GET /api/dashboard
        /// <summary>
        /// Retorna as métricas principais do Dashboard (contagem de Navios, Manobras e Agentes). (Tarefa BK-07)
        /// </summary>
        /// <returns>Retorna um objeto com as métricas gerais do sistema.</returns>
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

        // 🟢 BK-08: Endpoint: GET /api/dashboard/manobras/recentes
        /// <summary>
        /// Retorna uma lista simulada das últimas manobras para a seção "Manobras Recentes" do Dashboard. (Tarefa BK-08)
        /// </summary>
        /// <returns>Retorna uma lista de ManobraRecenteDto.</returns>
        [HttpGet("manobras/recentes")]
        [ProducesResponseType(typeof(IEnumerable<ManobraRecenteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<ManobraRecenteDto>> GetRecentManeuvers()
        {
            var manobras = new List<ManobraRecenteDto>
            {
                new ManobraRecenteDto { NomeNavio = "Navio Alpha", Porto = "Porto A", Hora = "14:30", Status = "Concluída" },
                new ManobraRecenteDto { NomeNavio = "Navio Beta", Porto = "Porto B", Hora = "13:15", Status = "Em andamento" },
                new ManobraRecenteDto { NomeNavio = "Navio Gamma", Porto = "Porto C", Hora = "11:45", Status = "Agendada" },
                new ManobraRecenteDto { NomeNavio = "Navio Delta", Porto = "Porto A", Hora = "10:20", Status = "Concluída" }
            };

            return Ok(manobras);
        }

        // 🟢 BK-09: Endpoint: GET /api/dashboard/navios/ativos
        /// <summary>
        /// Retorna uma lista simulada dos navios que estão atualmente em operação no porto. (Tarefa BK-09)
        /// </summary>
        /// <returns>Retorna uma lista de NavioAtivoDto.</returns>
        [HttpGet("navios/ativos")]
        [ProducesResponseType(typeof(IEnumerable<NavioAtivoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<NavioAtivoDto>> GetActiveShips()
        {
            var navios = new List<NavioAtivoDto>
            {
                new NavioAtivoDto { Nome = "Navio Alpha (IMO: 9876543)", PortoAtual = "Porto A", TempoNoPorto = "12h 45m" },
                new NavioAtivoDto { Nome = "Navio Beta (IMO: 1234567)", PortoAtual = "Em Manobra", TempoNoPorto = "2h 10m" },
                new NavioAtivoDto { Nome = "Navio Gamma (IMO: 1122334)", PortoAtual = "Porto C", TempoNoPorto = "2d 01h" }
            };

            return Ok(navios);
        }
    }
}