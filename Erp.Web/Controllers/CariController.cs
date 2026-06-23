using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erp.Data;
using Erp.Data.Entities;

namespace Erp.Web.Controllers
{
    [Authorize]
    public class CariController : Controller
    {
        private readonly ErpDbContext _context;

        public CariController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: Cari
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cariler.ToListAsync());
        }

        // GET: Cari/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null) return NotFound();

            var cari = await _context.Cariler
                .FirstOrDefaultAsync(m => m.CariKodu == id);
            if (cari == null) return NotFound();

            return View(cari);
        }

        // GET: Cari/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cari/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CariKodu,CariUnvani,CariKisaAd,CariTipi,Durumu,VergiDairesi,VergiNo,TCKN,MersisNo,TicaretSicilNo,YetkiliKisi,Telefon,EPosta,WebSitesi,Ulke,Il,Ilce,PostaKodu,Adres,SatisTemsilcisi,Bolge,Sektor,DovizTuru,RiskLimiti,OlusturmaTarihi,GuncellemeTarihi")] Cari cari)
        {
            if (string.IsNullOrEmpty(cari.CariKodu))
            {
                var sonCari = _context.Cariler
                    .OrderByDescending(c => c.CariKodu)
                    .FirstOrDefault();

                int sonNumara = 0;
                if (sonCari != null && sonCari.CariKodu.StartsWith("C-"))
                {
                    var numaraStr = sonCari.CariKodu.Replace("C-", "");
                    int.TryParse(numaraStr, out sonNumara);
                }

                int yeniNumara = sonNumara + 1;
                cari.CariKodu = $"C-{yeniNumara:D4}";
            }

            ModelState.Remove("CariKodu");

            if (ModelState.IsValid)
            {
                _context.Add(cari);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cari);
        }

        // GET: Cari/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null) return NotFound();

            var cari = await _context.Cariler.FindAsync(id);
            if (cari == null) return NotFound();
            return View(cari);
        }

        // POST: Cari/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CariKodu,CariUnvani,CariKisaAd,CariTipi,Durumu,VergiDairesi,VergiNo,TCKN,MersisNo,TicaretSicilNo,YetkiliKisi,Telefon,EPosta,WebSitesi,Ulke,Il,Ilce,PostaKodu,Adres,SatisTemsilcisi,Bolge,Sektor,DovizTuru,RiskLimiti,OlusturmaTarihi,GuncellemeTarihi")] Cari cari)
        {
            if (id != cari.CariKodu) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    cari.GuncellemeTarihi = DateTime.Now;
                    _context.Update(cari);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CariExists(cari.CariKodu)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cari);
        }

        // GET: Cari/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null) return NotFound();

            var cari = await _context.Cariler
                .FirstOrDefaultAsync(m => m.CariKodu == id);
            if (cari == null) return NotFound();

            return View(cari);
        }

        // POST: Cari/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var cari = await _context.Cariler.FindAsync(id);
            if (cari != null) _context.Cariler.Remove(cari);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CariExists(string id)
        {
            return _context.Cariler.Any(e => e.CariKodu == id);
        }
    }
}