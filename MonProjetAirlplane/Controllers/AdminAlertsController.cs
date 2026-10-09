using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonProjetAirlplane.Data;

namespace MonProjetAirlplane.Controllers
{
    [Authorize]
    public class AdminAlertsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminAlertsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminAlerts
        public async Task<IActionResult> Index()
        {
            // Récupère uniquement les alertes non résolues
            var activeAlerts = await _context.Alerts
                .Where(a => !a.IsResolved)
                .ToListAsync();

            return View(activeAlerts);
        }

        [HttpPost]
        public async Task<IActionResult> Resolve(string id)
        {
            var alert = await _context.Alerts.FindAsync(id);
            if (alert != null)
            {
                alert.IsResolved = true;
                _context.Update(alert);
                await _context.SaveChangesAsync();
            }
            // Vous pouvez rediriger vers "Index" de ce contrôleur ou vers le Dashboard
            return RedirectToAction(nameof(Index));
        }
    }
}