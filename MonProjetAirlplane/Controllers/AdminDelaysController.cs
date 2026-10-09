using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonProjetAirlplane.Data;
using MonProjetAirlplane.Models;

namespace MonProjetAirlplane.Controllers
{
    public class AdminDelaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminDelaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminDelays
        public async Task<IActionResult> Index()
        {
            var delays = await _context.Delays.ToListAsync();
            return View(delays);
        }

        // GET: AdminDelays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminDelays/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DelayViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
                _context.Delays.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AdminDelays/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var delay = await _context.Delays.FindAsync(id);
            if (delay == null) return NotFound();

            return View(delay);
        }

        // POST: AdminDelays/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, DelayViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Delays.Any(e => e.Id == model.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AdminDelays/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var delay = await _context.Delays.FindAsync(id);
            if (delay != null)
            {
                _context.Delays.Remove(delay);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}