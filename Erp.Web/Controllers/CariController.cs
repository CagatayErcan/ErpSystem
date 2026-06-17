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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cari = await _context.Cariler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cari == null)
            {
                return NotFound();
            }

            return View(cari);
        }

        // GET: Cari/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cari/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CariKodu,CariUnvani,CariKisaAd,CariTipi,Durumu,VergiDairesi,VergiNo,TCKN,MersisNo,TicaretSicilNo,YetkiliKisi,Telefon,EPosta,WebSitesi,Ulke,Il,Ilce,PostaKodu,Adres,SatisTemsilcisi,Bolge,Sektor,DovizTuru,RiskLimiti,OlusturmaTarihi,GuncellemeTarihi")] Cari cari)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cari);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cari);
        }

        // GET: Cari/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cari = await _context.Cariler.FindAsync(id);
            if (cari == null)
            {
                return NotFound();
            }
            return View(cari);
        }

        // POST: Cari/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CariKodu,CariUnvani,CariKisaAd,CariTipi,Durumu,VergiDairesi,VergiNo,TCKN,MersisNo,TicaretSicilNo,YetkiliKisi,Telefon,EPosta,WebSitesi,Ulke,Il,Ilce,PostaKodu,Adres,SatisTemsilcisi,Bolge,Sektor,DovizTuru,RiskLimiti,OlusturmaTarihi,GuncellemeTarihi")] Cari cari)
        {
            if (id != cari.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cari);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CariExists(cari.Id))
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
            return View(cari);
        }

        // GET: Cari/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cari = await _context.Cariler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cari == null)
            {
                return NotFound();
            }

            return View(cari);
        }

        // POST: Cari/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cari = await _context.Cariler.FindAsync(id);
            if (cari != null)
            {
                _context.Cariler.Remove(cari);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CariExists(int id)
        {
            return _context.Cariler.Any(e => e.Id == id);
        }
        public IActionResult Test()
        {
            var count = _context.Cariler.Count();
            return Content($"Cari tablosunda {count} kayıt var!");
        }
    }
}
