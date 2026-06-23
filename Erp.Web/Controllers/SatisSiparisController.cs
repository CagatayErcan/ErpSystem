using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erp.Data;
using Erp.Data.Entities;

namespace Erp.Web.Controllers
{
    [Authorize]
    public class SatisSiparisController : Controller
    {
        private readonly ErpDbContext _context;

        public SatisSiparisController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: SatisSiparis
        public async Task<IActionResult> Index()
        {
            var siparisler = await _context.SatisSiparisleri
                .Include(s => s.Cari)
                .Include(s => s.Detaylar)
                .ThenInclude(d => d.Stok)
                .ToListAsync();
            return View(siparisler);
        }

        // GET: SatisSiparis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var siparis = await _context.SatisSiparisleri
                .Include(s => s.Cari)
                .Include(s => s.Detaylar)
                .ThenInclude(d => d.Stok)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (siparis == null) return NotFound();
            return View(siparis);
        }

        // GET: SatisSiparis/Create
        public IActionResult Create()
        {
            // Müşteri listesi (CariKisaAd ile)
            ViewBag.CariListesi = _context.Cariler
                .Where(c => c.Durumu == "Aktif" && (c.CariTipi == "Müşteri" || c.CariTipi == "Her İkisi"))
                .OrderBy(c => c.CariKisaAd)
                .Select(c => new { c.CariKodu, c.CariKisaAd })
                .ToList();

            // Ürün listesi
            ViewBag.Urunler = _context.Urunler
            .OrderBy(u => u.UrunAdi)
            .Select(u => new { u.Id, u.UrunKodu, u.UrunAdi, u.MusteriKisaAd })
            .ToList();

            // Otomatik Sipariş No
            ViewBag.YeniSiparisNo = GenerateSiparisNo();

            return View();
        }

        // POST: SatisSiparis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SatisSiparis satisSiparis)
        {
            if (string.IsNullOrEmpty(satisSiparis.CariKodu))
            {
                ModelState.AddModelError("", "Lütfen bir müşteri seçiniz.");
                return View(satisSiparis);
            }

            if (string.IsNullOrEmpty(satisSiparis.SiparisNo))
            {
                satisSiparis.SiparisNo = GenerateSiparisNo();
            }

            var detaylar = Request.Form["Detaylar[0].StokId"].ToArray();
            if (detaylar.Length == 0 || string.IsNullOrEmpty(detaylar[0]))
            {
                ModelState.AddModelError("", "En az bir ürün ekleyiniz.");
                return View(satisSiparis);
            }

            if (ModelState.IsValid)
            {
                satisSiparis.OlusturmaTarihi = DateTime.Now;
                satisSiparis.Durum = "Planlama Onayı Bekliyor";

                _context.Add(satisSiparis);
                await _context.SaveChangesAsync();

                // Detayları ekle
                await AddDetaylar(satisSiparis.Id);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Hata durumunda ViewBag'leri yeniden doldur
            ViewBag.CariListesi = _context.Cariler
                .Where(c => c.Durumu == "Aktif" && (c.CariTipi == "Müşteri" || c.CariTipi == "Her İkisi"))
                .OrderBy(c => c.CariKisaAd)
                .Select(c => new {c.CariKisaAd })
                .ToList();

            ViewBag.Urunler = _context.Urunler
            .OrderBy(u => u.UrunAdi)
            .Select(u => new { u.Id, u.UrunKodu, u.UrunAdi, SatisFiyati = 0 })
            .ToList();

            return View(satisSiparis);
        }

        // GET: SatisSiparis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var siparis = await _context.SatisSiparisleri
                .Include(s => s.Cari)
                .Include(s => s.Detaylar)
                .ThenInclude(d => d.Stok)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (siparis == null) return NotFound();

            ViewBag.CariListesi = _context.Cariler
                .Where(c => c.Durumu == "Aktif" && (c.CariTipi == "Müşteri" || c.CariTipi == "Her İkisi"))
                .OrderBy(c => c.CariKisaAd)
                .Select(c => new { c.CariKodu, c.CariKisaAd })
                .ToList();

            ViewBag.Urunler = _context.Urunler
                .OrderBy(u => u.UrunAdi)
                .Select(u => new { u.Id, u.UrunKodu, u.UrunAdi })
                .ToList();

            return View(siparis);
        }

        // POST: SatisSiparis/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SatisSiparis satisSiparis)
        {
            if (id != satisSiparis.Id) return NotFound();

            if (string.IsNullOrEmpty(satisSiparis.CariKodu))
            {
                ModelState.AddModelError("", "Lütfen bir müşteri seçiniz.");
                return View(satisSiparis);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    satisSiparis.GuncellemeTarihi = DateTime.Now;
                    _context.Update(satisSiparis);
                    await _context.SaveChangesAsync();

                    // Detayları güncelle (sil + yeniden ekle)
                    var eskiDetaylar = _context.SatisSiparisDetaylari.Where(d => d.SatisSiparisId == id);
                    _context.SatisSiparisDetaylari.RemoveRange(eskiDetaylar);

                    await AddDetaylar(id);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SatisSiparisExists(satisSiparis.Id)) return NotFound();
                    throw;
                }
            }

            ViewBag.CariListesi = _context.Cariler
                .Where(c => c.Durumu == "Aktif" && (c.CariTipi == "Müşteri" || c.CariTipi == "Her İkisi"))
                .OrderBy(c => c.CariKisaAd)
                .Select(c => new { c.CariKodu, c.CariKisaAd })
                .ToList();

            ViewBag.Urunler = _context.Urunler
            .OrderBy(u => u.UrunAdi)
            .Select(u => new { u.Id, u.UrunKodu, u.UrunAdi, SatisFiyati = 0 })
            .ToList();

            return View(satisSiparis);
        }

        // GET: SatisSiparis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var siparis = await _context.SatisSiparisleri
                .Include(s => s.Cari)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (siparis == null) return NotFound();
            return View(siparis);
        }

        // POST: SatisSiparis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detaylar = _context.SatisSiparisDetaylari.Where(d => d.SatisSiparisId == id);
            _context.SatisSiparisDetaylari.RemoveRange(detaylar);

            var siparis = await _context.SatisSiparisleri.FindAsync(id);
            if (siparis != null) _context.SatisSiparisleri.Remove(siparis);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // PRIVATE METHODS
        // ==========================================
        private string GenerateSiparisNo()
        {
            var sonSiparis = _context.SatisSiparisleri
                .OrderByDescending(s => s.SiparisNo)
                .FirstOrDefault();

            int sonNumara = 0;
            if (sonSiparis != null && sonSiparis.SiparisNo.StartsWith("SS-"))
            {
                var numaraStr = sonSiparis.SiparisNo.Replace("SS-", "");
                int.TryParse(numaraStr, out sonNumara);
            }

            return $"SS-{(sonNumara + 1):D4}";
        }

        private async Task AddDetaylar(int siparisId)
        {
            var stokIdList = Request.Form["Detaylar[0].StokId"].ToArray();
            var miktarList = Request.Form["Detaylar[0].Miktar"].Select(x => decimal.TryParse(x, out decimal d) ? d : 0).ToArray();
            var birimList = Request.Form["Detaylar[0].Birim"].ToArray();
            var birimFiyatList = Request.Form["Detaylar[0].BirimFiyat"].Select(x => decimal.TryParse(x, out decimal d) ? d : 0).ToArray();
            var iskontoList = Request.Form["Detaylar[0].IskontoOrani"].Select(x => decimal.TryParse(x, out decimal d) ? d : 0).ToArray();
            var kdvList = Request.Form["Detaylar[0].KdvOrani"].Select(x => decimal.TryParse(x, out decimal d) ? d : 0).ToArray();
            var satirToplamList = Request.Form["Detaylar[0].SatirToplami"].Select(x => decimal.TryParse(x, out decimal d) ? d : 0).ToArray();

            for (int i = 0; i < stokIdList.Length; i++)
            {
                if (!string.IsNullOrEmpty(stokIdList[i]) && int.TryParse(stokIdList[i], out int stokId))
                {
                    var detay = new SatisSiparisDetay
                    {
                        SatisSiparisId = siparisId,
                        StokId = stokId,
                        Miktar = i < miktarList.Length ? miktarList[i] : 0,
                        Birim = i < birimList.Length ? birimList[i] : "ADET",
                        BirimFiyat = i < birimFiyatList.Length ? birimFiyatList[i] : 0,
                        IskontoOrani = i < iskontoList.Length ? iskontoList[i] : 0,
                        KdvOrani = i < kdvList.Length ? kdvList[i] : 20,
                        SatirToplami = i < satirToplamList.Length ? satirToplamList[i] : 0
                    };
                    _context.SatisSiparisDetaylari.Add(detay);
                }
            }
        }

        private bool SatisSiparisExists(int id)
        {
            return _context.SatisSiparisleri.Any(e => e.Id == id);
        }
    }
}