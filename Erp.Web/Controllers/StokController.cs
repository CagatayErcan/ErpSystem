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
        public async Task<IActionResult> Create()
        {
            // Stok tiplerini ViewBag'e gönder
            ViewBag.StokTipleri = await _context.StokTipleri
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToListAsync();

            // Ana Grupları ViewBag'e gönder
            ViewBag.AnaGruplari = await _context.AnaGruplari
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToListAsync();

            // Alt Grupları ViewBag'e gönder
            ViewBag.AltGruplari = await _context.AltGruplari
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToListAsync();

            // Gider Kategorilerini ViewBag'e gönder
            ViewBag.GiderKategorileri = await _context.GiderKategorileri
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToListAsync();
            // Masraf Merkezleri listesi (Kod + Ad)
            ViewBag.MasrafMerkezleri = await _context.MasrafMerkezleri
                .Where(m => m.Durum == "Aktif")
                .OrderBy(m => m.Ad)
                .Select(m => new { m.Kod, m.Ad, m.AnaGrup })
                .ToListAsync();
            // =============================================
            // Hata ayıklama için
            System.Diagnostics.Debug.WriteLine($"StokTipleri: {ViewBag.StokTipleri.Count}");
            System.Diagnostics.Debug.WriteLine($"AnaGruplari: {ViewBag.AnaGruplari.Count}");
            System.Diagnostics.Debug.WriteLine($"AltGruplari: {ViewBag.AltGruplari.Count}");
            System.Diagnostics.Debug.WriteLine($"GiderKategorileri: {ViewBag.GiderKategorileri.Count}");

            return View();
        }

        // POST: Stok/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Stok stok)
        {
            // ========== STOK KODUNU ZORLA OLUŞTUR ==========
            // Stok tipine göre prefix belirle
            string prefix = stok.StokTipi switch
            {
                "Hammadde" => "HM",
                "Sarf" => "SRF",
                "Hizmet" => "HZM",
                "Demirbaş" => "DMB",
                "Yarı Mamul" => "YM",
                _ => "STK"
            };

            // Bu prefix ile başlayan son kaydı bul
            var sonKayit = _context.Stoklar
                .Where(s => s.StokTipi == stok.StokTipi && s.StokKodu.StartsWith(prefix))
                .OrderByDescending(s => s.StokKodu)
                .FirstOrDefault();

            int sonNumara = 0;
            if (sonKayit != null)
            {
                var numaraStr = sonKayit.StokKodu.Replace(prefix + "-", "");
                int.TryParse(numaraStr, out sonNumara);
            }

            int yeniNumara = sonNumara + 1;
            stok.StokKodu = $"{prefix}-{yeniNumara:D4}";
            // =============================================

            // ModelState'i temizle (StokKodu hatasını kaldır)
            ModelState.Remove("StokKodu");

            if (ModelState.IsValid)
            {
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

            // Tüm tanımlamaları ViewBag'e gönder
            ViewBag.StokTipleri = await _context.StokTipleri
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToListAsync();

            ViewBag.AnaGruplari = await _context.AnaGruplari
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToListAsync();

            ViewBag.AltGruplari = await _context.AltGruplari
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToListAsync();

            ViewBag.GiderKategorileri = await _context.GiderKategorileri
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToListAsync();
            // ========== MASRAF MERKEZLERİ (YENİ) ==========
            ViewBag.MasrafMerkezleri = await _context.MasrafMerkezleri
                .Where(m => m.Durum == "Aktif")
                .OrderBy(m => m.Ad)
                .Select(m => new { m.Kod, m.Ad, m.AnaGrup })
                .ToListAsync();
            // =============================================

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
                try
                {
                    // ========== MEVCUT KAYDI BUL ==========
                    var mevcut = await _context.Stoklar
                        .AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == id);

                    if (mevcut == null) return NotFound();

                    // ========== STOK TİPİ DEĞİŞTİYSE KODU GÜNCELLE ==========
                    // Eski tip ve yeni tip farklı ise
                    if (mevcut.StokTipi != stok.StokTipi)
                    {
                        string prefix = stok.StokTipi switch
                        {
                            "Hammadde" => "HM",
                            "Sarf" => "SRF",
                            "Hizmet" => "HZM",
                            "Demirbaş" => "DMB",
                            "Yarı Mamul" => "YM",
                            _ => "STK"
                        };

                        // Bu prefix ile başlayan son kaydı bul
                        var sonKayit = await _context.Stoklar
                            .Where(s => s.StokTipi == stok.StokTipi && s.StokKodu.StartsWith(prefix))
                            .OrderByDescending(s => s.StokKodu)
                            .FirstOrDefaultAsync();

                        int sonNumara = 0;
                        if (sonKayit != null)
                        {
                            var numaraStr = sonKayit.StokKodu.Replace(prefix + "-", "");
                            int.TryParse(numaraStr, out sonNumara);
                        }

                        int yeniNumara = sonNumara + 1;
                        stok.StokKodu = $"{prefix}-{yeniNumara:D4}";

                        System.Diagnostics.Debug.WriteLine($"Yeni Kod: {stok.StokKodu}");
                    }
                    else
                    {
                        // Tip değişmediyse eski kodu koru
                        stok.StokKodu = mevcut.StokKodu;
                    }

                    // ========== GÜNCELLEMEYİ KAYDET ==========
                    stok.GuncellemeTarihi = DateTime.Now;
                    _context.Update(stok);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Stok başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StokExists(stok.Id)) return NotFound();
                    throw;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + ex.Message);
                    return View(stok);
                }
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
        // ==========================================
        // STOK TİPLERİ TANIMLAMA
        // ==========================================
        public async Task<IActionResult> StokTipiTanimi()
        {
            var list = await _context.StokTipleri
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToListAsync();
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> StokTipiTanimi(string[] items)
        {
            // Bu metod, tanımlama sayfasından gelen verileri işler
            // Şimdilik boş bırakalım, ihtiyaç halinde doldurulur
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> StokTipiTanimiEkle(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                // Aynı isimde kayıt var mı kontrol et
                var exists = await _context.StokTipleri.AnyAsync(x => x.Ad == item);
                if (!exists)
                {
                    _context.StokTipleri.Add(new StokTipi { Ad = item });
                    await _context.SaveChangesAsync();
                }
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> StokTipiTanimiSil(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                // Stok kartlarında kullanılıyor mu kontrol et
                var kullaniliyor = await _context.Stoklar.AnyAsync(x => x.StokTipi == item);
                if (kullaniliyor)
                {
                    return BadRequest("Bu stok tipi kullanımda olduğu için silinemez.");
                }

                var entity = await _context.StokTipleri.FirstOrDefaultAsync(x => x.Ad == item);
                if (entity != null)
                {
                    _context.StokTipleri.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            return Ok();
        }

        // ==========================================
        // ANA GRUP TANIMLAMA
        // ==========================================
        public async Task<IActionResult> AnaGrupTanimi()
        {
            var list = await _context.AnaGruplari
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToListAsync();
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> AnaGrupTanimiEkle(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                var exists = await _context.AnaGruplari.AnyAsync(x => x.Ad == item);
                if (!exists)
                {
                    _context.AnaGruplari.Add(new AnaGrup { Ad = item });
                    await _context.SaveChangesAsync();
                }
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AnaGrupTanimiSil(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                var kullaniliyor = await _context.Stoklar.AnyAsync(x => x.AnaGrup == item);
                if (kullaniliyor)
                {
                    return BadRequest("Bu ana grup kullanımda olduğu için silinemez.");
                }

                var entity = await _context.AnaGruplari.FirstOrDefaultAsync(x => x.Ad == item);
                if (entity != null)
                {
                    _context.AnaGruplari.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            return Ok();
        }

        // ==========================================
        // ALT GRUP TANIMLAMA
        // ==========================================
        public async Task<IActionResult> AltGrupTanimi()
        {
            var list = await _context.AltGruplari
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToListAsync();
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> AltGrupTanimiEkle(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                var exists = await _context.AltGruplari.AnyAsync(x => x.Ad == item);
                if (!exists)
                {
                    _context.AltGruplari.Add(new AltGrup { Ad = item });
                    await _context.SaveChangesAsync();
                }
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AltGrupTanimiSil(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                var kullaniliyor = await _context.Stoklar.AnyAsync(x => x.AltGrup == item);
                if (kullaniliyor)
                {
                    return BadRequest("Bu alt grup kullanımda olduğu için silinemez.");
                }

                var entity = await _context.AltGruplari.FirstOrDefaultAsync(x => x.Ad == item);
                if (entity != null)
                {
                    _context.AltGruplari.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            return Ok();
        }

        // ==========================================
        // GİDER KATEGORİSİ TANIMLAMA
        // ==========================================
        public async Task<IActionResult> GiderKategoriTanimi()
        {
            var list = await _context.GiderKategorileri
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToListAsync();
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> GiderKategoriTanimiEkle(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                var exists = await _context.GiderKategorileri.AnyAsync(x => x.Ad == item);
                if (!exists)
                {
                    _context.GiderKategorileri.Add(new GiderKategorisi { Ad = item });
                    await _context.SaveChangesAsync();
                }
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GiderKategoriTanimiSil(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                var kullaniliyor = await _context.Stoklar.AnyAsync(x => x.GiderKategorisi == item);
                if (kullaniliyor)
                {
                    return BadRequest("Bu gider kategorisi kullanımda olduğu için silinemez.");
                }

                var entity = await _context.GiderKategorileri.FirstOrDefaultAsync(x => x.Ad == item);
                if (entity != null)
                {
                    _context.GiderKategorileri.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            return Ok();
        }
       
    }
}