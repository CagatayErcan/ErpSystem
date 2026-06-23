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
    public class UrunController : Controller
    {
        private readonly ErpDbContext _context;

        public UrunController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: Urun
        public async Task<IActionResult> Index()
        {
            return View(await _context.Urunler.ToListAsync());
        }

        // GET: Urun/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var urun = await _context.Urunler
                .Include(u => u.ErgitmeReceteleri)
                .Include(u => u.KalipReceteleri)
                .Include(u => u.MacaReceteleri)
                .Include(u => u.IslemeReceteleri)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (urun == null) return NotFound();

            // ========== YENİ: Çift Kayıt Temizleme ==========
            // Ergitme Reçeteleri
            if (urun.ErgitmeReceteleri != null && urun.ErgitmeReceteleri.Any())
            {
                var tekrarEdenler = urun.ErgitmeReceteleri
                    .GroupBy(r => r.StokKodu)
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g.OrderByDescending(r => r.Id).Skip(1))
                    .ToList();

                foreach (var item in tekrarEdenler)
                {
                    _context.UrunErgitmeReceteleri.Remove(item);
                }
                await _context.SaveChangesAsync();
            }

            // Maça Reçeteleri (YENİ Model - MacaRecete)
            if (urun.MacaReceteleri != null && urun.MacaReceteleri.Any())
            {
                var tekrarEdenler = urun.MacaReceteleri
                    .GroupBy(r => r.MacaKodu)
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g.OrderByDescending(r => r.Id).Skip(1))
                    .ToList();

                foreach (var item in tekrarEdenler)
                {
                    _context.MacaReceteleri.Remove(item);
                }
                await _context.SaveChangesAsync();
            }

            // İşleme Reçeteleri
            if (urun.IslemeReceteleri != null && urun.IslemeReceteleri.Any())
            {
                var tekrarEdenler = urun.IslemeReceteleri
                    .GroupBy(r => r.StokKodu)
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g.OrderByDescending(r => r.Id).Skip(1))
                    .ToList();

                foreach (var item in tekrarEdenler)
                {
                    _context.UrunIslemeReceteleri.Remove(item);
                }
                await _context.SaveChangesAsync();
            }
            // Besleyici detayını getir
            if (!string.IsNullOrEmpty(urun.BesleyiciStokKodu))
            {
                ViewBag.BesleyiciDetay = await _context.Stoklar
                    .FirstOrDefaultAsync(s => s.StokKodu == urun.BesleyiciStokKodu);
            }
            // ========== KONTROL SONU ==========

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

        // GET: Urun/Create
        public IActionResult Create()
        {
            // Create metodunun başına ekleyin
            var liste = _context.Cariler
                .Where(c => c.Durumu == "Aktif" && (c.CariTipi == "Müşteri" || c.CariTipi == "Her İkisi"))
                .Select(c => new { c.CariKisaAd, c.CariUnvani })
                .ToList();

            foreach (var item in liste)
            {
                System.Diagnostics.Debug.WriteLine($"KisaAd: {item.CariKisaAd}, Unvan: {item.CariUnvani}");
            }
            // Stok listesi
            ViewBag.Stoklar = _context.Stoklar
                .OrderBy(s => s.StokAdi)
                .Select(s => new { s.Id, s.StokKodu, s.StokAdi, s.StokTipi })
                .ToList();
            // Besleyici listesi (Sarf tipi ve BesleyiciMi true olanlar)
            ViewBag.BesleyiciListesi = _context.Stoklar
                .Where(s => s.BesleyiciMi == true && s.StokTipi == "Sarf")
                .OrderBy(s => s.StokAdi)
                .Select(s => new { s.StokKodu, s.StokAdi, s.Aciklama })
                .ToList();

            // ========== MÜŞTERİ LİSTESİ (YENİ) ==========
            ViewBag.MusteriListesi = _context.Cariler
            .Where(c => c.Durumu == "Aktif" && (c.CariTipi == "Müşteri" || c.CariTipi == "Her İkisi"))
            .OrderBy(c => c.CariKisaAd)
            .Select(c => new { c.CariKisaAd })
            .ToList();
            // ===========================================

            return View();
        }

        // POST: Urun/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Urun urun)
        {
            // ========== MODELSTATE HATALARINI YAKALA ==========
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine("ModelState Hatası: " + error.ErrorMessage);
                }
                // Stok listesini tekrar ViewBag'e yükle
                ViewBag.Stoklar = _context.Stoklar
                    .OrderBy(s => s.StokAdi)
                    .Select(s => new { s.Id, s.StokKodu, s.StokAdi, s.StokTipi })
                    .ToList();
                return View(urun);
            }

            try
            {
                // 1. Ürünü ekle
                urun.OlusturmaTarihi = DateTime.Now;
                _context.Urunler.Add(urun);
                await _context.SaveChangesAsync();

                // ==========================================
                // 2. ERGİTME REÇETELERİ
                // ==========================================
                var ergitmeStokTipi = Request.Form["ErgitmeReceteleri[0].StokTipi"].ToArray();
                var ergitmeStokAdi = Request.Form["ErgitmeReceteleri[0].StokAdi"].ToArray();
                var ergitmeStokKodu = Request.Form["ErgitmeReceteleri[0].StokKodu"].ToArray();
                var ergitmeMiktar = Request.Form["ErgitmeReceteleri[0].Miktar"]
                    .Select(x => decimal.TryParse(x, out decimal d) ? d : 0).ToArray();
                var ergitmeBirim = Request.Form["ErgitmeReceteleri[0].Birim"].ToArray();
                var ergitmeElektrik = Request.Form["ErgitmeReceteleri[0].BirimElektrikTuketimi"]
                    .Select(x => string.IsNullOrEmpty(x) ? 0 : (decimal.TryParse(x, out decimal d) ? d : 0)).ToArray();
                var ergitmeSure = Request.Form["ErgitmeReceteleri[0].ErgitmeSuresi"]
                    .Select(x => string.IsNullOrEmpty(x) ? 0 : (int.TryParse(x, out int i) ? i : 0)).ToArray();

                System.Diagnostics.Debug.WriteLine($"Ergitme satır sayısı: {ergitmeStokKodu.Length}");

                // Eğer veri varsa ekle
                if (ergitmeStokKodu.Length > 0 && !string.IsNullOrEmpty(ergitmeStokKodu[0]))
                {
                    for (int i = 0; i < ergitmeStokKodu.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(ergitmeStokKodu[i]))
                        {
                            var recete = new UrunErgitmeRecete
                            {
                                UrunId = urun.Id,
                                StokTipi = i < ergitmeStokTipi.Length ? ergitmeStokTipi[i] : "",
                                StokAdi = i < ergitmeStokAdi.Length ? ergitmeStokAdi[i] : "",
                                StokKodu = ergitmeStokKodu[i],
                                Miktar = i < ergitmeMiktar.Length ? ergitmeMiktar[i] : 0,
                                Birim = i < ergitmeBirim.Length ? ergitmeBirim[i] : "KG",
                            };
                            _context.UrunErgitmeReceteleri.Add(recete);
                        }
                    }
                }

                // ==========================================
                // 3. MAÇA REÇETELERİ
                // ==========================================
                var macaAdi = Request.Form["MacaReceteleri[0].MacaAdi"].ToArray();
                var macaKodu = Request.Form["MacaReceteleri[0].MacaKodu"].ToArray();
                var macaKullanim = Request.Form["MacaReceteleri[0].KullanimAdedi"]
                    .Select(x => int.TryParse(x, out int i) ? i : 0).ToArray();
                var macaCinsi = Request.Form["MacaReceteleri[0].MacaCinsi"].ToArray();
                var macaCevrimSuresi = Request.Form["MacaReceteleri[0].MacaCevrimSuresi"]
                    .Select(x => int.TryParse(x, out int i) ? i : 0).ToArray();

                for (int i = 0; i < macaAdi.Length; i++)
                {
                    if (!string.IsNullOrEmpty(macaAdi[i]))
                    {
                        var recete = new MacaRecete
                        {
                            UrunId = urun.Id,
                            MacaAdi = macaAdi[i],
                            MacaKodu = i < macaKodu.Length ? macaKodu[i] : "",
                            KullanimAdedi = i < macaKullanim.Length ? macaKullanim[i] : 0,
                            MacaCinsi = i < macaCinsi.Length ? macaCinsi[i] : "",
                            MacaCevrimSuresi = i < macaCevrimSuresi.Length ? macaCevrimSuresi[i] : 0
                        };
                        _context.MacaReceteleri.Add(recete);
                    }
                }

                // ==========================================
                // 4. İŞLEME REÇETELERİ
                // ==========================================
                var islemeSira = Request.Form["IslemeReceteleri[0].OperasyonSırası"]
                    .Select(x => int.TryParse(x, out int i) ? i : 0).ToArray();
                var islemeKodu = Request.Form["IslemeReceteleri[0].OperasyonKodu"].ToArray();
                var islemeAdi = Request.Form["IslemeReceteleri[0].OperasyonAdi"].ToArray();
                var islemeMerkez = Request.Form["IslemeReceteleri[0].IsMerkezi"].ToArray();
                var islemeTakim = Request.Form["IslemeReceteleri[0].TakimTuketimi"]
                    .Select(x => decimal.TryParse(x, out decimal d) ? d : 0).ToArray();
                var islemeStokKodu = Request.Form["IslemeReceteleri[0].StokKodu"].ToArray();
                var islemeStokAdi = Request.Form["IslemeReceteleri[0].StokAdi"].ToArray();
                var islemeElektrik = Request.Form["IslemeReceteleri[0].BirimElektrikTuketimi"]
                    .Select(x => decimal.TryParse(x, out decimal d) ? d : 0).ToArray();

                System.Diagnostics.Debug.WriteLine($"İşleme satır sayısı: {islemeSira.Length}");

                if (islemeSira.Length > 0 && islemeSira[0] > 0)
                {
                    for (int i = 0; i < islemeSira.Length; i++)
                    {
                        if (islemeSira[i] > 0 || !string.IsNullOrEmpty(islemeKodu[i]))
                        {
                            var recete = new UrunIslemeRecete
                            {
                                UrunId = urun.Id,
                                OperasyonSırası = islemeSira[i],
                                OperasyonKodu = i < islemeKodu.Length ? islemeKodu[i] : "",
                                OperasyonAdi = i < islemeAdi.Length ? islemeAdi[i] : "",
                                IsMerkezi = i < islemeMerkez.Length ? islemeMerkez[i] : "",
                                TakimTuketimi = i < islemeTakim.Length ? islemeTakim[i] : 0,
                                StokKodu = i < islemeStokKodu.Length ? islemeStokKodu[i] : "",
                                StokAdi = i < islemeStokAdi.Length ? islemeStokAdi[i] : "",
                                BirimElektrikTuketimi = i < islemeElektrik.Length ? islemeElektrik[i] : 0
                            };
                            _context.UrunIslemeReceteleri.Add(recete);
                        }
                    }
                }

                await _context.SaveChangesAsync();

                // ==========================================
                // 5. BOM REVİZYONU OLUŞTUR
                // ==========================================
                var bom = new UrunBom
                {
                    UrunId = urun.Id,
                    RevizyonNo = "REV-001",
                    RevizyonTarihi = DateTime.Now,
                    RevizyonSebebi = "İlk oluşturma",
                    Olusturan = User.Identity?.Name ?? "admin"
                };
                _context.UrunBomlar.Add(bom);
                await _context.SaveChangesAsync();

                // ==========================================
                // 6. ÜRÜN KARTINA YÖNLENDİR
                // ==========================================
                return RedirectToAction(nameof(Details), new { id = urun.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
                ViewBag.Stoklar = _context.Stoklar
                    .OrderBy(s => s.StokAdi)
                    .Select(s => new { s.Id, s.StokKodu, s.StokAdi, s.StokTipi })
                    .ToList();
                return View(urun);
            }
        }

        // GET: Urun/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // ========== INCLUDE İLE REÇETELERİ GETİR ==========
            var urun = await _context.Urunler
                .Include(u => u.ErgitmeReceteleri)   // ← ERGİTME REÇETELERİ
                .Include(u => u.KalipReceteleri)     // ← KALIP REÇETELERİ
                .Include(u => u.MacaReceteleri)      // ← MAÇA REÇETELERİ
                .Include(u => u.IslemeReceteleri)    // ← İŞLEME REÇETELERİ
                .FirstOrDefaultAsync(m => m.Id == id);
            // ================================================

            if (urun == null) return NotFound();

            // Stok listesi
            ViewBag.Stoklar = _context.Stoklar
                .OrderBy(s => s.StokAdi)
                .Select(s => new { s.Id, s.StokKodu, s.StokAdi, s.StokTipi })
                .ToList();

            // Besleyici listesi (Sarf tipi ve BesleyiciMi true olanlar)
            ViewBag.BesleyiciListesi = _context.Stoklar
                .Where(s => s.BesleyiciMi == true && s.StokTipi == "Sarf")
                .OrderBy(s => s.StokAdi)
                .Select(s => new { s.StokKodu, s.StokAdi, s.Aciklama })
                .ToList();

            // Müşteri listesi
            ViewBag.MusteriListesi = _context.Cariler
                .Where(c => c.Durumu == "Aktif" && (c.CariTipi == "Müşteri" || c.CariTipi == "Her İkisi"))
                .OrderBy(c => c.CariUnvani)
                .Select(c => new { c.CariKisaAd, c.CariUnvani })
                .ToList();

            return View(urun);
        }

        // ==========================================
        // POST EDIT
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Urun urun)
        {
            if (id != urun.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Ana ürünü güncelle
                    urun.GuncellemeTarihi = DateTime.Now;
                    _context.Update(urun);

                    // 2. Eski reçeteleri TEMİZLE (RemoveRange ile)
                    var eskiErgitme = _context.UrunErgitmeReceteleri.Where(r => r.UrunId == id);
                    _context.UrunErgitmeReceteleri.RemoveRange(eskiErgitme);

                    var eskiMaca = _context.MacaReceteleri.Where(r => r.UrunId == id);
                    _context.MacaReceteleri.RemoveRange(eskiMaca);

                    var eskiIsleme = _context.UrunIslemeReceteleri.Where(r => r.UrunId == id);
                    _context.UrunIslemeReceteleri.RemoveRange(eskiIsleme);

                    // 3. Tüm form anahtarlarını tara ve Ergitme reçetelerini topla
                    var allKeys = Request.Form.Keys;
                    var ergitmeIndices = new HashSet<int>();

                    // ========== KALIP REÇETELERİNİ SİL ==========
                    var eskiKalip = _context.KalipReceteleri.Where(r => r.UrunId == id);
                    _context.KalipReceteleri.RemoveRange(eskiKalip);
                    // ===========================================

                    foreach (var key in allKeys)
                    {
                        if (key.StartsWith("ErgitmeReceteleri[") && key.Contains("]."))
                        {
                            var indexStr = key.Split('[')[1].Split(']')[0];
                            if (int.TryParse(indexStr, out int idx))
                            {
                                ergitmeIndices.Add(idx);
                            }
                        }
                    }

                    // 4. Her indeks için verileri oku ve EKLE
                    foreach (var index in ergitmeIndices.OrderBy(i => i))
                    {
                        var prefix = $"ErgitmeReceteleri[{index}]";
                        var stokKodu = Request.Form[$"{prefix}.StokKodu"].ToString();

                        if (!string.IsNullOrEmpty(stokKodu))
                        {
                            var recete = new UrunErgitmeRecete
                            {
                                UrunId = urun.Id,
                                StokTipi = Request.Form[$"{prefix}.StokTipi"].ToString(),
                                StokAdi = Request.Form[$"{prefix}.StokAdi"].ToString(),
                                StokKodu = stokKodu,
                                Miktar = decimal.TryParse(Request.Form[$"{prefix}.Miktar"], out decimal m) ? m : 0,
                                Birim = Request.Form[$"{prefix}.Birim"].ToString(),
                            };
                            _context.UrunErgitmeReceteleri.Add(recete);
                        }
                    }

                    // Maça Reçeteleri - Edit (YENİ Model)
                    var macaAdi = Request.Form["MacaReceteleri[0].MacaAdi"].ToArray();
                    var macaKodu = Request.Form["MacaReceteleri[0].MacaKodu"].ToArray();
                    var macaKullanim = Request.Form["MacaReceteleri[0].KullanimAdedi"]
                        .Select(x => int.TryParse(x, out int i) ? i : 0).ToArray();
                    var macaCinsi = Request.Form["MacaReceteleri[0].MacaCinsi"].ToArray();
                    var macaCevrimSuresi = Request.Form["MacaReceteleri[0].MacaCevrimSuresi"]
                        .Select(x => int.TryParse(x, out int i) ? i : 0).ToArray();

                    for (int i = 0; i < macaAdi.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(macaAdi[i]))
                        {
                            var recete = new MacaRecete
                            {
                                UrunId = urun.Id,
                                MacaAdi = macaAdi[i],
                                MacaKodu = i < macaKodu.Length ? macaKodu[i] : "",
                                KullanimAdedi = i < macaKullanim.Length ? macaKullanim[i] : 0,
                                MacaCinsi = i < macaCinsi.Length ? macaCinsi[i] : "",
                                MacaCevrimSuresi = i < macaCevrimSuresi.Length ? macaCevrimSuresi[i] : 0
                            };
                            _context.MacaReceteleri.Add(recete);
                        }
                    }

                    // 6. İşleme Reçeteleri (aynı mantık)
                    var islemeIndices = new HashSet<int>();
                    foreach (var key in allKeys)
                    {
                        if (key.StartsWith("IslemeReceteleri[") && key.Contains("]."))
                        {
                            var indexStr = key.Split('[')[1].Split(']')[0];
                            if (int.TryParse(indexStr, out int idx))
                            {
                                islemeIndices.Add(idx);
                            }
                        }
                    }

                    foreach (var index in islemeIndices.OrderBy(i => i))
                    {
                        var prefix = $"IslemeReceteleri[{index}]";
                        var opSira = Request.Form[$"{prefix}.OperasyonSırası"].ToString();

                        if (!string.IsNullOrEmpty(opSira) && int.TryParse(opSira, out int sira) && sira > 0)
                        {
                            var recete = new UrunIslemeRecete
                            {
                                UrunId = urun.Id,
                                OperasyonSırası = sira,
                                OperasyonKodu = Request.Form[$"{prefix}.OperasyonKodu"].ToString(),
                                OperasyonAdi = Request.Form[$"{prefix}.OperasyonAdi"].ToString(),
                                IsMerkezi = Request.Form[$"{prefix}.IsMerkezi"].ToString(),
                                TakimTuketimi = decimal.TryParse(Request.Form[$"{prefix}.TakimTuketimi"], out decimal t) ? t : 0,
                                StokKodu = Request.Form[$"{prefix}.StokKodu"].ToString(),
                                StokAdi = Request.Form[$"{prefix}.StokAdi"].ToString(),
                                BirimElektrikTuketimi = decimal.TryParse(Request.Form[$"{prefix}.BirimElektrikTuketimi"], out decimal e) ? e : 0
                            };
                            _context.UrunIslemeReceteleri.Add(recete);
                        }
                    }

                    // 7. TÜM değişiklikleri KAYDET
                    await _context.SaveChangesAsync();

                    // 8. BOM revizyonu oluştur
                    var sonBom = await _context.UrunBomlar
                        .Where(b => b.UrunId == urun.Id)
                        .OrderByDescending(b => b.RevizyonTarihi)
                        .FirstOrDefaultAsync();

                    string yeniRevizyonNo = sonBom == null ? "REV-001" : $"REV-{int.Parse(sonBom.RevizyonNo.Split('-')[1]) + 1:D3}";

                    var yeniBom = new UrunBom
                    {
                        UrunId = urun.Id,
                        RevizyonNo = yeniRevizyonNo,
                        RevizyonTarihi = DateTime.Now,
                        RevizyonSebebi = "Ürün düzenlendi",
                        Olusturan = User.Identity?.Name ?? "admin"
                    };
                    _context.UrunBomlar.Add(yeniBom);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Details), new { id = urun.Id });
                }
                catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true)
                {
                    ModelState.AddModelError("", "Aynı stok kodu birden fazla satırda kullanılamaz. Lütfen tekrar eden kayıtları kontrol edin.");
                    return View(urun);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UrunExists(urun.Id)) return NotFound();
                    throw;
                }
                catch (Exception ex)
                {
                    // Inner exception'ı göster
                    var innerMessage = ex.InnerException?.Message ?? ex.Message;
                    ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + innerMessage);
                    return View(urun);
                }
            }

            return View(urun);
        }

        // POST: Urun/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var urun = await _context.Urunler.FindAsync(id);
            if (urun != null)
            {
                _context.Urunler.Remove(urun);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UrunExists(int id)
        {
            return _context.Urunler.Any(e => e.Id == id);
        }
        [HttpGet]
        public async Task<IActionResult> GetStoklarByTip(string stokTipi)
        {
            if (string.IsNullOrEmpty(stokTipi))
                return Json(new List<object>());

            var stoklar = await _context.Stoklar
                .Where(s => s.StokTipi == stokTipi)
                .OrderBy(s => s.StokAdi)
                .Select(s => new { s.StokKodu, s.StokAdi })
                .ToListAsync();

            return Json(stoklar);
        }
        // GET: Urun/GetAlasimOranlari
        [HttpGet]
        public async Task<IActionResult> GetAlasimOranlari(string alasimSinifi)
        {
            if (string.IsNullOrEmpty(alasimSinifi))
                return Json(new List<object>());

            var oranlar = await _context.AlasimHammaddeOranlari
                .Where(x => x.AlasimSinifi == alasimSinifi)
                .Select(x => new { x.StokTipi, x.HammaddeKodu, x.HammaddeAdi, x.Oran })
                .ToListAsync();

            return Json(oranlar);
        }
        // GET: Urun/GetKalipOranlari
        [HttpGet]
        public async Task<IActionResult> GetKalipOranlari(string kalipCinsi)
        {
            if (string.IsNullOrEmpty(kalipCinsi))
                return Json(new List<object>());

            var oranlar = await _context.KalipSarfOranlari
                .Where(x => x.KalipCinsi == kalipCinsi)
                .Select(x => new { x.StokTipi, x.SarfKodu, x.SarfAdi, x.Miktar })
                .ToListAsync();

            return Json(oranlar);
        }

        // GET: Urun/GetKalipReceteData
        [HttpGet]
        public async Task<IActionResult> GetKalipReceteData(string kalipCinsi, string besleyiciStokKodu, int? besleyiciAdeti, int? parcaAdeti)
        {
            var result = new List<object>();

            // 1. Kalıp Sarf Oranları
            if (!string.IsNullOrEmpty(kalipCinsi))
            {
                var kalipOranlari = await _context.KalipSarfOranlari
                    .Where(x => x.KalipCinsi == kalipCinsi)
                    .Select(x => new
                    {
                        StokTipi = x.StokTipi,
                        StokAdi = x.SarfAdi,
                        StokKodu = x.SarfKodu,
                        Miktar = x.Miktar, // % olarak
                        Birim = x.Birim,
                        Kaynak = "Kalip"
                    })
                    .ToListAsync();

                result.AddRange(kalipOranlari);
            }

            // 2. Besleyici Bilgisi
            if (!string.IsNullOrEmpty(besleyiciStokKodu) && besleyiciAdeti.HasValue && parcaAdeti.HasValue && parcaAdeti.Value > 0)
            {
                var besleyici = await _context.Stoklar
                    .Where(s => s.StokKodu == besleyiciStokKodu)
                    .Select(s => new
                    {
                        StokTipi = s.StokTipi,
                        StokAdi = s.StokAdi,
                        StokKodu = s.StokKodu,
                        Miktar = (decimal)besleyiciAdeti.Value / parcaAdeti.Value, // Besleyici Adeti / Parça Adeti
                        Birim = "ADET",
                        Kaynak = "Besleyici"
                    })
                    .FirstOrDefaultAsync();

                if (besleyici != null)
                {
                    result.Add(besleyici);
                }
            }

            return Json(result);
        }
    }
}