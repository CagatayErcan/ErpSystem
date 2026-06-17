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
                .Include(u => u.MacaReceteleri)
                .Include(u => u.IslemeReceteleri)
                .FirstOrDefaultAsync(m => m.Id == id);

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

        // GET: Urun/Create
        public IActionResult Create()
        {
            // Tüm stokları ViewBag'e gönder
            ViewBag.Stoklar = _context.Stoklar
                .OrderBy(s => s.StokAdi)
                .Select(s => new { s.Id, s.StokKodu, s.StokAdi, s.StokTipi })
                .ToList();
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
                                BirimElektrikTuketimi = i < ergitmeElektrik.Length ? ergitmeElektrik[i] : 0,
                                ErgitmeSuresi = i < ergitmeSure.Length ? ergitmeSure[i] : 0
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
                var macaKullanim = Request.Form["MacaReceteleri[0].MacaKullanimAdedi"]
                    .Select(x => int.TryParse(x, out int i) ? i : 0).ToArray();
                var macaCinsi = Request.Form["MacaReceteleri[0].MacaCinsi"].ToArray();
                var macaKum = Request.Form["MacaReceteleri[0].KumTuketimi"]
                    .Select(x => decimal.TryParse(x, out decimal d) ? d : 0).ToArray();
                var macaRecine = Request.Form["MacaReceteleri[0].RecineTuketimi"]
                    .Select(x => decimal.TryParse(x, out decimal d) ? d : 0).ToArray();
                var macaCo2 = Request.Form["MacaReceteleri[0].Co2Tuketimi"]
                    .Select(x => decimal.TryParse(x, out decimal d) ? d : 0).ToArray();
                var macaAmin = Request.Form["MacaReceteleri[0].AminGazi"]
                    .Select(x => decimal.TryParse(x, out decimal d) ? d : 0).ToArray();
                var macaBezir = Request.Form["MacaReceteleri[0].BezirYagiTuketimi"]
                    .Select(x => decimal.TryParse(x, out decimal d) ? d : 0).ToArray();

                System.Diagnostics.Debug.WriteLine($"Maça satır sayısı: {macaAdi.Length}");

                if (macaAdi.Length > 0 && !string.IsNullOrEmpty(macaAdi[0]))
                {
                    for (int i = 0; i < macaAdi.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(macaAdi[i]))
                        {
                            var recete = new UrunMacaRecete
                            {
                                UrunId = urun.Id,
                                MacaAdi = macaAdi[i],
                                MacaKodu = i < macaKodu.Length ? macaKodu[i] : "",
                                MacaKullanimAdedi = i < macaKullanim.Length ? macaKullanim[i] : 0,
                                MacaCinsi = i < macaCinsi.Length ? macaCinsi[i] : "",
                                KumTuketimi = i < macaKum.Length ? macaKum[i] : 0,
                                RecineTuketimi = i < macaRecine.Length ? macaRecine[i] : 0,
                                Co2Tuketimi = i < macaCo2.Length ? macaCo2[i] : 0,
                                AminGazi = i < macaAmin.Length ? macaAmin[i] : 0,
                                BezirYagiTuketimi = i < macaBezir.Length ? macaBezir[i] : 0
                            };
                            _context.UrunMacaReceteleri.Add(recete);
                        }
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

            var urun = await _context.Urunler
                .Include(u => u.ErgitmeReceteleri)
                .Include(u => u.MacaReceteleri)
                .Include(u => u.IslemeReceteleri)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (urun == null) return NotFound();

            ViewBag.Stoklar = _context.Stoklar
                .OrderBy(s => s.StokAdi)
                .Select(s => new { s.Id, s.StokKodu, s.StokAdi, s.StokTipi })
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
                    // 1. Ana ürün bilgilerini güncelle
                    urun.GuncellemeTarihi = DateTime.Now;
                    _context.Update(urun);

                    // 2. Eski reçeteleri TAMAMEN SİL
                    var eskiErgitme = _context.UrunErgitmeReceteleri.Where(r => r.UrunId == id);
                    _context.UrunErgitmeReceteleri.RemoveRange(eskiErgitme);

                    var eskiMaca = _context.UrunMacaReceteleri.Where(r => r.UrunId == id);
                    _context.UrunMacaReceteleri.RemoveRange(eskiMaca);

                    var eskiIsleme = _context.UrunIslemeReceteleri.Where(r => r.UrunId == id);
                    _context.UrunIslemeReceteleri.RemoveRange(eskiIsleme);

                    // 3. Tüm form anahtarlarını tara ve Ergitme reçetelerini topla
                    var ergitmeKeys = Request.Form.Keys
                        .Where(k => k.StartsWith("ErgitmeReceteleri[") && k.Contains("]."))
                        .Select(k => k.Split('[')[1].Split(']')[0])
                        .Distinct()
                        .OrderBy(k => int.Parse(k))
                        .ToList();

                    var ergitmeStokKodu = new List<string>();
                    var ergitmeStokTipi = new List<string>();
                    var ergitmeStokAdi = new List<string>();
                    var ergitmeMiktar = new List<decimal>();
                    var ergitmeBirim = new List<string>();
                    var ergitmeElektrik = new List<decimal>();
                    var ergitmeSure = new List<int>();

                    foreach (var index in ergitmeKeys)
                    {
                        var prefix = $"ErgitmeReceteleri[{index}]";
                        ergitmeStokKodu.Add(Request.Form[$"{prefix}.StokKodu"].ToString());
                        ergitmeStokTipi.Add(Request.Form[$"{prefix}.StokTipi"].ToString());
                        ergitmeStokAdi.Add(Request.Form[$"{prefix}.StokAdi"].ToString());

                        if (decimal.TryParse(Request.Form[$"{prefix}.Miktar"], out decimal miktar))
                            ergitmeMiktar.Add(miktar);
                        else
                            ergitmeMiktar.Add(0);

                        ergitmeBirim.Add(Request.Form[$"{prefix}.Birim"].ToString());

                        if (decimal.TryParse(Request.Form[$"{prefix}.BirimElektrikTuketimi"], out decimal elektrik))
                            ergitmeElektrik.Add(elektrik);
                        else
                            ergitmeElektrik.Add(0);

                        if (int.TryParse(Request.Form[$"{prefix}.ErgitmeSuresi"], out int sure))
                            ergitmeSure.Add(sure);
                        else
                            ergitmeSure.Add(0);
                    }

                    System.Diagnostics.Debug.WriteLine($"Ergitme satır sayısı: {ergitmeStokKodu.Count}");

                    for (int i = 0; i < ergitmeStokKodu.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(ergitmeStokKodu[i]))
                        {
                            var recete = new UrunErgitmeRecete
                            {
                                UrunId = urun.Id,
                                StokTipi = i < ergitmeStokTipi.Count ? ergitmeStokTipi[i] : "",
                                StokAdi = i < ergitmeStokAdi.Count ? ergitmeStokAdi[i] : "",
                                StokKodu = ergitmeStokKodu[i],
                                Miktar = i < ergitmeMiktar.Count ? ergitmeMiktar[i] : 0,
                                Birim = i < ergitmeBirim.Count ? ergitmeBirim[i] : "KG",
                                BirimElektrikTuketimi = i < ergitmeElektrik.Count ? ergitmeElektrik[i] : 0,
                                ErgitmeSuresi = i < ergitmeSure.Count ? ergitmeSure[i] : 0
                            };
                            _context.UrunErgitmeReceteleri.Add(recete);
                        }
                    }

                    // 4. Maça Reçeteleri
                    var macaKeys = Request.Form.Keys
                        .Where(k => k.StartsWith("MacaReceteleri[") && k.EndsWith("].MacaAdi"))
                        .OrderBy(k => k)
                        .ToList();

                    if (macaKeys.Any())
                    {
                        var macaAdi = macaKeys.Select(k => Request.Form[k].ToString()).ToList();
                        var macaKodu = macaKeys.Select(k => Request.Form[k.Replace("MacaAdi", "MacaKodu")].ToString()).ToList();
                        var macaKullanim = macaKeys.Select(k => int.TryParse(Request.Form[k.Replace("MacaAdi", "MacaKullanimAdedi")], out int i) ? i : 0).ToList();
                        var macaCinsi = macaKeys.Select(k => Request.Form[k.Replace("MacaAdi", "MacaCinsi")].ToString()).ToList();
                        var macaKum = macaKeys.Select(k => decimal.TryParse(Request.Form[k.Replace("MacaAdi", "KumTuketimi")], out decimal d) ? d : 0).ToList();
                        var macaRecine = macaKeys.Select(k => decimal.TryParse(Request.Form[k.Replace("MacaAdi", "RecineTuketimi")], out decimal d) ? d : 0).ToList();
                        var macaCo2 = macaKeys.Select(k => decimal.TryParse(Request.Form[k.Replace("MacaAdi", "Co2Tuketimi")], out decimal d) ? d : 0).ToList();
                        var macaAmin = macaKeys.Select(k => decimal.TryParse(Request.Form[k.Replace("MacaAdi", "AminGazi")], out decimal d) ? d : 0).ToList();
                        var macaBezir = macaKeys.Select(k => decimal.TryParse(Request.Form[k.Replace("MacaAdi", "BezirYagiTuketimi")], out decimal d) ? d : 0).ToList();

                        for (int i = 0; i < macaAdi.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(macaAdi[i]))
                            {
                                var recete = new UrunMacaRecete
                                {
                                    UrunId = urun.Id,
                                    MacaAdi = macaAdi[i],
                                    MacaKodu = i < macaKodu.Count ? macaKodu[i] : "",
                                    MacaKullanimAdedi = i < macaKullanim.Count ? macaKullanim[i] : 0,
                                    MacaCinsi = i < macaCinsi.Count ? macaCinsi[i] : "",
                                    KumTuketimi = i < macaKum.Count ? macaKum[i] : 0,
                                    RecineTuketimi = i < macaRecine.Count ? macaRecine[i] : 0,
                                    Co2Tuketimi = i < macaCo2.Count ? macaCo2[i] : 0,
                                    AminGazi = i < macaAmin.Count ? macaAmin[i] : 0,
                                    BezirYagiTuketimi = i < macaBezir.Count ? macaBezir[i] : 0
                                };
                                _context.UrunMacaReceteleri.Add(recete);
                            }
                        }
                    }

                    // 5. İşleme Reçeteleri
                    var islemeKeys = Request.Form.Keys
                        .Where(k => k.StartsWith("IslemeReceteleri[") && k.EndsWith("].OperasyonSırası"))
                        .OrderBy(k => k)
                        .ToList();

                    if (islemeKeys.Any())
                    {
                        var islemeSira = islemeKeys.Select(k => int.TryParse(Request.Form[k], out int i) ? i : 0).ToList();
                        var islemeKodu = islemeKeys.Select(k => Request.Form[k.Replace("OperasyonSırası", "OperasyonKodu")].ToString()).ToList();
                        var islemeAdi = islemeKeys.Select(k => Request.Form[k.Replace("OperasyonSırası", "OperasyonAdi")].ToString()).ToList();
                        var islemeMerkez = islemeKeys.Select(k => Request.Form[k.Replace("OperasyonSırası", "IsMerkezi")].ToString()).ToList();
                        var islemeTakim = islemeKeys.Select(k => decimal.TryParse(Request.Form[k.Replace("OperasyonSırası", "TakimTuketimi")], out decimal d) ? d : 0).ToList();
                        var islemeStokKodu = islemeKeys.Select(k => Request.Form[k.Replace("OperasyonSırası", "StokKodu")].ToString()).ToList();
                        var islemeStokAdi = islemeKeys.Select(k => Request.Form[k.Replace("OperasyonSırası", "StokAdi")].ToString()).ToList();
                        var islemeElektrik = islemeKeys.Select(k => decimal.TryParse(Request.Form[k.Replace("OperasyonSırası", "BirimElektrikTuketimi")], out decimal d) ? d : 0).ToList();

                        for (int i = 0; i < islemeSira.Count; i++)
                        {
                            if (islemeSira[i] > 0 || !string.IsNullOrEmpty(islemeKodu[i]))
                            {
                                var recete = new UrunIslemeRecete
                                {
                                    UrunId = urun.Id,
                                    OperasyonSırası = islemeSira[i],
                                    OperasyonKodu = i < islemeKodu.Count ? islemeKodu[i] : "",
                                    OperasyonAdi = i < islemeAdi.Count ? islemeAdi[i] : "",
                                    IsMerkezi = i < islemeMerkez.Count ? islemeMerkez[i] : "",
                                    TakimTuketimi = i < islemeTakim.Count ? islemeTakim[i] : 0,
                                    StokKodu = i < islemeStokKodu.Count ? islemeStokKodu[i] : "",
                                    StokAdi = i < islemeStokAdi.Count ? islemeStokAdi[i] : "",
                                    BirimElektrikTuketimi = i < islemeElektrik.Count ? islemeElektrik[i] : 0
                                };
                                _context.UrunIslemeReceteleri.Add(recete);
                            }
                        }
                    }

                    // 6. Değişiklikleri kaydet
                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine("Tüm değişiklikler kaydedildi.");

                    // 7. BOM revizyonu oluştur
                    var sonBom = await _context.UrunBomlar
                        .Where(b => b.UrunId == urun.Id)
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!UrunExists(urun.Id)) return NotFound();
                    throw;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + ex.Message);
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
    }
}