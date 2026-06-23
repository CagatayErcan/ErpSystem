using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Erp.Data;
using Erp.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Erp.Web.Controllers
{
    [Authorize]
    public class MasrafMerkeziController : Controller
    {
        private readonly ErpDbContext _context;

        public MasrafMerkeziController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: MasrafMerkezi
        public async Task<IActionResult> Index()
        {
            return View(await _context.MasrafMerkezleri.ToListAsync());
        }

        // GET: MasrafMerkezi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var masrafMerkezi = await _context.MasrafMerkezleri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (masrafMerkezi == null)
            {
                return NotFound();
            }

            return View(masrafMerkezi);
        }

        // GET: MasrafMerkezi/Create
        public IActionResult Create()
        {
            // ========== ANA GRUP LİSTESİ (Stok'tan) ==========
            ViewBag.AnaGruplari = _context.AnaGruplari
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToList();
            // ================================================

            // Son kodu bul ve yeni kod üret
            var sonKayit = _context.MasrafMerkezleri
                .OrderByDescending(x => x.Kod)
                .FirstOrDefault();

            int sonNumara = 0;
            if (sonKayit != null && sonKayit.Kod != null && sonKayit.Kod.StartsWith("MM-"))
            {
                var numaraStr = sonKayit.Kod.Replace("MM-", "");
                int.TryParse(numaraStr, out sonNumara);
            }

            ViewBag.YeniKod = $"MM-{(sonNumara + 1):D4}";
            return View();
        }

        // POST: MasrafMerkezi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Kod,Ad,AnaGrup,Durum,Aciklama")] MasrafMerkezi masrafMerkezi)
        {
            if (string.IsNullOrEmpty(masrafMerkezi.Kod))
            {
                var sonKayit = _context.MasrafMerkezleri
                    .OrderByDescending(x => x.Kod)
                    .FirstOrDefault();

                int sonNumara = 0;
                if (sonKayit != null && sonKayit.Kod != null && sonKayit.Kod.StartsWith("MM-"))
                {
                    var numaraStr = sonKayit.Kod.Replace("MM-", "");
                    int.TryParse(numaraStr, out sonNumara);
                }

                masrafMerkezi.Kod = $"MM-{(sonNumara + 1):D4}";
            }

            // ========== HATA AYIKLAMA ==========
            System.Diagnostics.Debug.WriteLine($"Kod: {masrafMerkezi.Kod}");
            System.Diagnostics.Debug.WriteLine($"Ad: {masrafMerkezi.Ad}");
            System.Diagnostics.Debug.WriteLine($"AnaGrup: {masrafMerkezi.AnaGrup}");
            System.Diagnostics.Debug.WriteLine($"Durum: {masrafMerkezi.Durum}");
            // ==================================

            if (ModelState.IsValid)
            {
                _context.Add(masrafMerkezi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AnaGruplari = _context.AnaGruplari
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToList();

            return View(masrafMerkezi);
        }

        // GET: MasrafMerkezi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var masrafMerkezi = await _context.MasrafMerkezleri.FindAsync(id);
            if (masrafMerkezi == null) return NotFound();

            // ========== ANA GRUP LİSTESİ (Stok'tan) ==========
            ViewBag.AnaGruplari = _context.AnaGruplari
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToList();
            // ================================================

            return View(masrafMerkezi);
        }

        // POST: MasrafMerkezi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kod,Ad,AnaGrup,Durum,Aciklama,OlusturmaTarihi")] MasrafMerkezi masrafMerkezi)
        {
            if (id != masrafMerkezi.Id) return NotFound();

            // ========== HATA AYIKLAMA ==========
            System.Diagnostics.Debug.WriteLine($"Id: {masrafMerkezi.Id}");
            System.Diagnostics.Debug.WriteLine($"Kod: {masrafMerkezi.Kod}");
            System.Diagnostics.Debug.WriteLine($"Ad: {masrafMerkezi.Ad}");
            System.Diagnostics.Debug.WriteLine($"AnaGrup: {masrafMerkezi.AnaGrup}");
            System.Diagnostics.Debug.WriteLine($"Durum: {masrafMerkezi.Durum}");
            // ==================================

            if (ModelState.IsValid)
            {
                try
                {
                    masrafMerkezi.GuncellemeTarihi = DateTime.Now;
                    _context.Update(masrafMerkezi);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MasrafMerkeziExists(masrafMerkezi.Id)) return NotFound();
                    throw;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + ex.Message);
                }
            }

            ViewBag.AnaGruplari = _context.AnaGruplari
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToList();

            return View(masrafMerkezi);
        }

        // GET: MasrafMerkezi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var masrafMerkezi = await _context.MasrafMerkezleri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (masrafMerkezi == null)
            {
                return NotFound();
            }

            return View(masrafMerkezi);
        }

        // POST: MasrafMerkezi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var masrafMerkezi = await _context.MasrafMerkezleri.FindAsync(id);
            if (masrafMerkezi != null)
            {
                _context.MasrafMerkezleri.Remove(masrafMerkezi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MasrafMerkeziExists(int id)
        {
            return _context.MasrafMerkezleri.Any(e => e.Id == id);
        }
    }
}
