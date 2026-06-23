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
    public class IsMerkeziController : Controller
    {
        private readonly ErpDbContext _context;

        public IsMerkeziController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: IsMerkezi
        public async Task<IActionResult> Index()
        {
            await SeedIsMerkezleri();
            return View(await _context.IsMerkezleri.ToListAsync());
        }

        // GET: IsMerkezi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var isMerkezi = await _context.IsMerkezleri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (isMerkezi == null)
            {
                return NotFound();
            }

            return View(isMerkezi);
        }

        // GET: IsMerkezi/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IsMerkezi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kod,Ad,Aciklama,OlusturmaTarihi,GuncellemeTarihi")] IsMerkezi isMerkezi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(isMerkezi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(isMerkezi);
        }

        // GET: IsMerkezi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var isMerkezi = await _context.IsMerkezleri.FindAsync(id);
            if (isMerkezi == null)
            {
                return NotFound();
            }
            return View(isMerkezi);
        }

        // POST: IsMerkezi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kod,Ad,Aciklama,OlusturmaTarihi,GuncellemeTarihi")] IsMerkezi isMerkezi)
        {
            if (id != isMerkezi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(isMerkezi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IsMerkeziExists(isMerkezi.Id))
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
            return View(isMerkezi);
        }

        // GET: IsMerkezi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var isMerkezi = await _context.IsMerkezleri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (isMerkezi == null)
            {
                return NotFound();
            }

            return View(isMerkezi);
        }

        // POST: IsMerkezi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isMerkezi = await _context.IsMerkezleri.FindAsync(id);
            if (isMerkezi != null)
            {
                _context.IsMerkezleri.Remove(isMerkezi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IsMerkeziExists(int id)
        {
            return _context.IsMerkezleri.Any(e => e.Id == id);
        }
        private async Task SeedIsMerkezleri()
        {
            if (!_context.IsMerkezleri.Any())
            {
                var list = new List<IsMerkezi>
        {
            new IsMerkezi { Kod = "ERG-001", Ad = "Ergitme" },
            new IsMerkezi { Kod = "MAC-001", Ad = "Maçahane" },
            new IsMerkezi { Kod = "KAL-001", Ad = "Kalıphane" },
            new IsMerkezi { Kod = "KUM-001", Ad = "Kumlama" },
            new IsMerkezi { Kod = "TAS-001", Ad = "Taşlama" },
            new IsMerkezi { Kod = "DT-001", Ad = "Dikey Torna" },
            new IsMerkezi { Kod = "YT-001", Ad = "Yatay Torna" },
            new IsMerkezi { Kod = "DI-001", Ad = "Dikey İşlem" },
            new IsMerkezi { Kod = "YI-001", Ad = "Yatay İşlem" },
            new IsMerkezi { Kod = "KK-001", Ad = "Kalite Kontrol" },
            new IsMerkezi { Kod = "BOY-001", Ad = "Boya" },
            new IsMerkezi { Kod = "PAK-001", Ad = "Paketleme" }
        };

                _context.IsMerkezleri.AddRange(list);
                await _context.SaveChangesAsync();
            }
        }
    }
}
