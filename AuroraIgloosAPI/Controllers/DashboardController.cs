using AuroraIgloosAPI.BussinessLogic;
using AuroraIgloosAPI.DTOs;
using AuroraIgloosAPI.Models.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace AuroraIgloosAPI.Controllers
{
    public class DashboardController : ControllerBase
    {
        private readonly CompanyContext _context;

        public DashboardController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public ActionResult<DashboardStatsDTO> GetDashboardStats([FromQuery] int days = 30)
        {
            var to = DateOnly.FromDateTime(DateTime.Now);
            var from = to.AddDays(-days + 1);

            var logic = new DashboardLogic(_context);
            var stats = logic.GetDashboardStats(from, to);

            return Ok(stats);
        }
    }
}
