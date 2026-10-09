using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonProjetAirlplane.Data;

namespace MonProjetAirlplane.Controllers
{
    [Authorize]
    [Route("api/dashboard")]
    [ApiController]
    public class ApiDashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetQuickStats()
        {
            var totalFlights = await _context.Flights.CountAsync();
            var ongoingDelays = await _context.Delays.CountAsync(d => d.Status == "En cours");
            var activeContracts = await _context.Contracts.CountAsync(c => c.Status == "Actif");

            return Ok(new
            {
                totalFlights,
                ongoingDelays,
                activeContracts,
                lastUpdated = DateTime.Now.ToString("HH:mm:ss")
            });
        }
    }
}