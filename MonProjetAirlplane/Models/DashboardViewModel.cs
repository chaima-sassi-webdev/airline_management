using System.Collections.Generic;

namespace MonProjetAirlplane.Models
{
    public class DashboardViewModel
    {
        public int TotalFlights { get; set; }
        public double TotalPaxFlown { get; set; }
        public double TotalPaxBooked { get; set; }
        public decimal AverageFareGlobal { get; set; }
        public double TotalPaxC { get; set; }
        public double TotalPaxY { get; set; }
        public int TotalUsers { get; set; }
        public int TotalSuppliers { get; set; }
        public int ActiveContracts { get; set; }
        public int OngoingDelays { get; set; }
        public int AvailableArticles { get; set; }

        // Utilise directement MarketDetails ici
        public List<MarketDetails> RecentFlights { get; set; } = new List<MarketDetails>();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public Dictionary<string, int> TopDestinations { get; set; } = new();
        public Dictionary<string, decimal> AverageFarePerDestination { get; set; } = new();
        public List<DelayViewModel> RecentDelays { get; set; } = new();
        public List<Alert> RecentAlerts { get; set; } = new();
    }
}