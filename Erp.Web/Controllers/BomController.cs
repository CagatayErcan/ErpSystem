using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erp.Data;
using Erp.Data.Entities;

namespace Erp.Web.Controllers
{
    [Authorize]
    public class BomController : Controller
    {
        private readonly ErpDbContext _context;

        public BomController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: Bom (Tüm ürünlerin BOM listesi)
        public async Task<IActionResult> Index()
        {
            var urunler = await _context.Urunler
                .Include(u => u.ErgitmeReceteleri)
                .Include(u => u.MacaReceteleri)
                .Include(u => u.IslemeReceteleri)
                .ToListAsync();

            return View(urunler);
        }

        // GET: Bom/Details/5 (Belirli bir ürünün BOM detayı)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var urun = await _context.Urunler
                .Include(u => u.ErgitmeReceteleri)
                .Include(u => u.MacaReceteleri)
                .Include(u => u.IslemeReceteleri)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (urun == null) return NotFound();

            // BOM revizyon bilgisini getir
            var bom = await _context.UrunBomlar
                .Where(b => b.UrunId == id)
                .OrderByDescending(b => b.RevizyonTarihi)
                .FirstOrDefaultAsync();

            ViewBag.Bom = bom;
            ViewBag.BomTarihce = await _context.UrunBomlar
                .Where(b => b.UrunId == id)
                .OrderByDescending(b => b.RevizyonTarihi)
                .ToListAsync();

            return View(urun);
        }

        // POST: Bom/CreateRevision (Yeni revizyon oluştur)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRevision(int urunId, string revizyonSebebi)
        {
            var urun = await _context.Urunler.FindAsync(urunId);
            if (urun == null) return NotFound();

            // Son revizyon numarasını bul
            var sonBom = await _context.UrunBomlar
                .Where(b => b.UrunId == urunId)
                .OrderByDescending(b => b.RevizyonTarihi)
                .FirstOrDefaultAsync();

            string yeniRevizyonNo;
            if (sonBom == null)
            {
                yeniRevizyonNo = "REV-001";
            }
            else
            {
                var sonNumara = int.Parse(sonBom.RevizyonNo.Split('-')[1]);
                yeniRevizyonNo = $"REV-{sonNumara + 1:D3}";
            }

            var yeniBom = new UrunBom
            {
                UrunId = urunId,
                RevizyonNo = yeniRevizyonNo,
                RevizyonTarihi = DateTime.Now,
                RevizyonSebebi = revizyonSebebi,
                Olusturan = User.Identity?.Name ?? "admin"
            };

            _context.UrunBomlar.Add(yeniBom);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = urunId });
        }
    }
}