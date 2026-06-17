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
            return View();
        }

        // POST: MasrafMerkezi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kod,Ad,UstMasrafMerkezi,Durum,Aciklama,OlusturmaTarihi,GuncellemeTarihi")] MasrafMerkezi masrafMerkezi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(masrafMerkezi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(masrafMerkezi);
        }

        // GET: MasrafMerkezi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var masrafMerkezi = await _context.MasrafMerkezleri.FindAsync(id);
            if (masrafMerkezi == null)
            {
                return NotFound();
            }
            return View(masrafMerkezi);
        }

        // POST: MasrafMerkezi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kod,Ad,UstMasrafMerkezi,Durum,Aciklama,OlusturmaTarihi,GuncellemeTarihi")] MasrafMerkezi masrafMerkezi)
        {
            if (id != masrafMerkezi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(masrafMerkezi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MasrafMerkeziExists(masrafMerkezi.Id))
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
