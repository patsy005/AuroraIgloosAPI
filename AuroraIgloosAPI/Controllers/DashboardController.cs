using AuroraIgloosAPI.BussinessLogic;
using AuroraIgloosAPI.DTOs;
using AuroraIgloosAPI.Models.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuroraIgloosAPI.Controllers
{
    [Authorize(Roles = "Admin,Staff,ReadOnly")]
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly CompanyContext _context;

        public DashboardController(CompanyContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Staff,ReadOnly")]
        // GET: /api/Dashboard/stats?days=30
        [HttpGet("stats")]
        public ActionResult<DashboardStatsDTO> GetStats([FromQuery] int days = 30)
        {
            if (days <= 0) return BadRequest("days must be > 0");

            var to = DateOnly.FromDateTime(DateTime.Now);
            var from = to.AddDays(-days + 1);

            var logic = new DashboardLogic(_context);
            var stats = logic.GetDashboardStats(from, to);

            return Ok(stats);
        }
        
        [Authorize(Roles = "Admin,Staff,ReadOnly")]
        [HttpGet("sales")]
        public ActionResult<List<DashboardSalesPointDTO>> GetSales([FromQuery] int months = 12)
        {
            var to = DateOnly.FromDateTime(DateTime.Now);
            var from = new DateOnly(to.Year, to.Month, 1).AddMonths(-(months - 1));

            var logic = new DashboardLogic(_context);
            var series = logic.GetSalesSeries(from, to);

            return Ok(series);
        }
    }
}
