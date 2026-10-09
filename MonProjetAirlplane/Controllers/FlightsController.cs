using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonProjetAirlplane.Data;
using MonProjetAirlplane.Models;
using Microsoft.AspNetCore.Authorization;
using MonProjetAirlplane.Services;

namespace MonProjetAirlplane.Controllers
{
    [Authorize]
    public class FlightsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PdfService _pdfService;

        public FlightsController(ApplicationDbContext context, PdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
        }
        public async Task<IActionResult> ExportPdf(string searchString)
        {
            var query = _context.MarketDetails
                .Include(m => m.Market)
                .AsQueryable();

            // Appliquer le filtre de recherche en toute sécurité
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(f => 
                    (f.FN_CARRIER != null && f.FN_CARRIER.Contains(searchString)) ||
                    f.FN_NUMBER.ToString().Contains(searchString) || // Conversion de l'int en string
                    (f.AC_REGISTRATION != null && f.AC_REGISTRATION.Contains(searchString)) ||
                    (f.ARR_AP_ACTUAL != null && f.ARR_AP_ACTUAL.Contains(searchString))
                );
            }

            var flights = await query.ToListAsync();
            var pdfBytes = _pdfService.GenerateFlightReport(flights);

            return File(pdfBytes, "application/pdf", $"Rapport_Vols_{DateTime.Now:yyyyMMdd}.pdf");
        }

        // Fonction dédiée pour exporter uniquement les derniers vols ajoutés
        public async Task<IActionResult> ExportRecentPdf()
        {
            // 1. Récupérer les vols enregistrés manuellement dans la table Flights
            var manualFlights = await _context.Flights
                .OrderByDescending(f => f.Id)
                .ToListAsync();

            // 2. Convertir chaque objet Flight en MarketDetails en gérant la conversion de type pour FN_NUMBER
            var marketDetailsList = manualFlights.Select(f => new MarketDetails {
                FN_CARRIER = f.FN_CARRIER,
                // Si FN_NUMBER est un string dans Flight et un int dans MarketDetails (ou inversement), ajustez ici :
                FN_NUMBER = int.TryParse(f.FN_NUMBER, out int num) ? num : 0, // Utilise int.TryParse si f.FN_NUMBER est un string
                AC_REGISTRATION = f.AC_REGISTRATION,
                DAY_OF_ORIGIN = f.DAY_OF_ORIGIN,
                ARR_AP_ACTUAL = f.ARR_AP_ACTUAL,
                PAX_FLOWN_C = f.PAX_FLOWN_C,
                PAX_FLOWN_Y = f.PAX_FLOWN_Y,
                AVG_FARE = f.AVG_FARE
            }).ToList();

            // 3. Générer le rapport PDF avec ces données converties
            var pdfBytes = _pdfService.GenerateFlightReport(marketDetailsList);

            return File(pdfBytes, "application/pdf", $"Rapport_Vols_Ajoutes_{DateTime.Now:yyyyMMdd}.pdf");
        }
       
        // GET: Flights
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 10;
            int pageIndex = pageNumber ?? 1;

            // On interroge uniquement la table des vols manuels (Flights)
            var query = _context.Flights.OrderByDescending(f => f.Id);

            int totalItems = await query.CountAsync();
            
            var flights = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int totalPages = totalItems > 0 ? (int)Math.Ceiling(totalItems / (double)pageSize) : 1;

            ViewData["CurrentPage"] = pageIndex;
            ViewData["TotalPages"] = totalPages;

            // On retourne directement la liste des "Flight" à la vue Index
            return View(flights);
        }
        // GET: Flights/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flights/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FN_CARRIER,FN_NUMBER,AC_REGISTRATION,DAY_OF_ORIGIN,ARR_AP_ACTUAL,PAX_FLOWN_C,PAX_FLOWN_Y,AVG_FARE")] Flight flight)
        {
            bool volExiste = await _context.Flights
                .AnyAsync(f => f.FN_CARRIER == flight.FN_CARRIER && f.FN_NUMBER == flight.FN_NUMBER);

            if (volExiste)
            {
                ModelState.AddModelError(string.Empty, "Ce numéro de vol (Transporteur + Numéro) existe déjà dans la base de données.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        // POST: Flights/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FN_CARRIER,FN_NUMBER,AC_REGISTRATION,DAY_OF_ORIGIN,ARR_AP_ACTUAL,PAX_FLOWN_C,PAX_FLOWN_Y,AVG_FARE")] Flight flight)
        {
            if (id != flight.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.Id == id);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        
        // GET: Flights/Dashboard
        public async Task<IActionResult> Dashboard(int? pageNumber)
        {
            // 1. On interroge UNIQUEMENT la table Flights (les saisies manuelles)
            var flightsQuery = _context.Flights.OrderByDescending(f => f.Id);

            int pageSize = 10;
            int pageIndex = pageNumber ?? 1;
            
            int totalItems = await flightsQuery.CountAsync();
            
            var pagedFlights = await flightsQuery
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 2. Récupérer tous les vols de la table Flights pour les calculs globaux
            var allFlights = await _context.Flights.ToListAsync();

            var viewModel = new DashboardViewModel
            {
                TotalFlights = allFlights.Count,
                TotalPaxFlown = allFlights.Sum(f => f.PAX_FLOWN_C + f.PAX_FLOWN_Y),
                TotalPaxBooked = allFlights.Sum(f => f.PAX_FLOWN_C + f.PAX_FLOWN_Y),
                AverageFareGlobal = allFlights.Any() ? (decimal)allFlights.Average(f => (double)f.AVG_FARE) : 0,
                TotalPaxC = allFlights.Sum(f => f.PAX_FLOWN_C),
                TotalPaxY = allFlights.Sum(f => f.PAX_FLOWN_Y),

                TopDestinations = allFlights
                    .Where(f => !string.IsNullOrEmpty(f.ARR_AP_ACTUAL))
                    .GroupBy(f => f.ARR_AP_ACTUAL!)
                    .ToDictionary(g => g.Key, g => g.Count()),

                AverageFarePerDestination = allFlights
                    .GroupBy(f => f.ARR_AP_ACTUAL ?? "Inconnue")
                    .ToDictionary(
                        g => g.Key, 
                        g => (decimal)g.Average(f => (double)f.AVG_FARE)
                    ),
                
                #nullable disable
                // 3. Conversion propre des vols manuels pour la vue
                RecentFlights = pagedFlights.Select(f => new MarketDetails {
                    ID = f.Id.ToString(),
                    FN_CARRIER = f.FN_CARRIER,
                    FN_NUMBER = int.TryParse(f.FN_NUMBER, out int num) ? num : 0,
                    AC_REGISTRATION = f.AC_REGISTRATION,
                    DAY_OF_ORIGIN = f.DAY_OF_ORIGIN,
                    ARR_AP_ACTUAL = f.ARR_AP_ACTUAL,
                    PAX_FLOWN_C = f.PAX_FLOWN_C,
                    PAX_FLOWN_Y = f.PAX_FLOWN_Y,
                    AVG_FARE = f.AVG_FARE
                }).ToList(),
                #nullable restore
            };

            int totalPages = totalItems > 0 ? (int)Math.Ceiling(totalItems / (double)pageSize) : 1;

            ViewData["CurrentPage"] = pageIndex;
            ViewData["TotalPages"] = totalPages;

            return View(viewModel);
        }
    }
}