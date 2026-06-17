using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erp.Data;
using Erp.Data.Entities;

namespace Erp.Web.Controllers
{
    [Authorize]
    public class StokController : Controller
    {
        private readonly ErpDbContext _context;

        public StokController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: Stok
        public async Task<IActionResult> Index()
        {
            var stoklar = await _context.Stoklar.ToListAsync();
            return View(stoklar);
        }

        // GET: Stok/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var stok = await _context.Stoklar.FirstOrDefaultAsync(m => m.Id == id);
            if (stok == null) return NotFound();
            return View(stok);
        }

        // GET: Stok/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stok/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Stok stok)
        {
            if (ModelState.IsValid)
            {
                stok.OlusturmaTarihi = DateTime.Now;
                _context.Add(stok);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stok);
        }

        // GET: Stok/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var stok = await _context.Stoklar.FindAsync(id);
            if (stok == null) return NotFound();
            return View(stok);
        }

        // POST: Stok/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Stok stok)
        {
            if (id != stok.Id) return NotFound();
            if (ModelState.IsValid)
            {
                stok.GuncellemeTarihi = DateTime.Now;
                _context.Update(stok);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stok);
        }

        // GET: Stok/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var stok = await _context.Stoklar.FirstOrDefaultAsync(m => m.Id == id);
            if (stok == null) return NotFound();
            return View(stok);
        }

        // POST: Stok/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stok = await _context.Stoklar.FindAsync(id);
            if (stok != null) _context.Stoklar.Remove(stok);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StokExists(int id)
        {
            return _context.Stoklar.Any(e => e.Id == id);
        }
        public IActionResult Test()
        {
            var count = _context.Stoklar.Count();
            return Content($"Stok tablosunda {count} kayıt var!");
        }
    }
}