using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonProjetAirlplane.Services;

namespace MonProjetAirlplane.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly DashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(DashboardService dashboardService, ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            try
            {
                // 1. Récupère les données globales et paginées du dashboard via le service
                var viewModel = await _dashboardService.GetDashboardDataAsync(pageNumber, 10);
                
                ViewData["CurrentPage"] = viewModel.CurrentPage;
                ViewData["TotalPages"] = viewModel.TotalPages;

                // 2. Renvoie le ViewModel complet à la vue
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement du tableau de bord.");
                // Vous pouvez rediriger vers une vue d'erreur personnalisée ou afficher un message vide
                return View(new MonProjetAirlplane.Models.DashboardViewModel());
            }
        }
    }
}