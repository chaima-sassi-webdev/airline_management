using Microsoft.EntityFrameworkCore;
using MonProjetAirlplane.Data;
using MonProjetAirlplane.Models;

namespace MonProjetAirlplane.Services
{
    public class DashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync(int pageNumber = 1, int pageSize = 10)
        {
            // On charge tous les éléments pour garder vos calculs globaux intacts (KPIs, graphiques, etc.)
            var marketDetails = await _context.MarketDetails
                .Include(m => m.Market) // Pour récupérer la Zone et le Pays sans "N/A"
                .ToListAsync();

            // Calcul de la pagination sur la liste triée
            var orderedFlights = marketDetails.OrderByDescending(f => f.DAY_OF_ORIGIN).ToList();
            int totalItems = orderedFlights.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Extraction de la sous-liste pour la page actuelle
            var pagedFlights = orderedFlights
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // 🚨 Chargement des alertes actives (non résolues)
            var activeAlerts = await _context.Alerts
                .Where(a => !a.IsResolved)
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .ToListAsync();

            var viewModel = new DashboardViewModel
            {
                TotalFlights = marketDetails.Count,
                TotalPaxFlown = marketDetails.Sum(f => f.PAX_FLOWN_C + f.PAX_FLOWN_Y),
                TotalPaxBooked = marketDetails.Sum(f => f.PAX_FLOWN_C + f.PAX_FLOWN_Y),
                
                AverageFareGlobal = marketDetails.Any() ? (decimal)marketDetails.Average(f => (double)f.AVG_FARE) : 0,
                
                TotalPaxC = marketDetails.Sum(f => f.PAX_FLOWN_C),
                TotalPaxY = marketDetails.Sum(f => f.PAX_FLOWN_Y),
                
                // Éléments paginés pour le tableau
                RecentFlights = pagedFlights,

                // 🚨 Assignation des alertes au ViewModel
                RecentAlerts = activeAlerts,

                TopDestinations = marketDetails
                    .Where(f => !string.IsNullOrEmpty(f.ARR_AP_ACTUAL))
                    .GroupBy(f => f.ARR_AP_ACTUAL!)
                    .ToDictionary(g => g.Key, g => g.Count()),
                    
                AverageFarePerDestination = marketDetails
                    .GroupBy(f => f.ARR_AP_ACTUAL ?? "Inconnue")
                    .ToDictionary(
                        g => g.Key, 
                        g => (decimal)g.Average(f => (double)f.AVG_FARE)
                    ),

                // Paramètres de pagination pour la vue
                CurrentPage = pageNumber,
                TotalPages = totalPages > 0 ? totalPages : 1
            };

            return viewModel;
        }
    }
}