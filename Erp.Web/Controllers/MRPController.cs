using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erp.Data;
using Erp.Data.Entities;

namespace Erp.Web.Controllers
{
    [Authorize]
    public class MRPController : Controller
    {
        private readonly ErpDbContext _context;

        public MRPController(ErpDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // ANA SAYFA
        // ==========================================
        public async Task<IActionResult> Index()
        {
            ViewBag.AlasimOranlari = await _context.AlasimHammaddeOranlari.ToListAsync();
            ViewBag.KalipSarfOranlari = await _context.KalipSarfOranlari.ToListAsync();
            ViewBag.MacaSarfOranlari = await _context.MacaSarfOranlari.ToListAsync();
            ViewBag.EnerjiStandartlari = await _context.EnerjiStandartlari.ToListAsync();
            return View();
        }

        // ==========================================
        // ALAŞIM ORANLARI (CRUD)
        // ==========================================
        public async Task<IActionResult> AlasimOranlari()
        {
            return View(await _context.AlasimHammaddeOranlari.ToListAsync());
        }

        public IActionResult AlasimOranlariCreate()
        {
            ViewBag.StokTipleri = _context.StokTipleri
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlasimOranlariCreate(string AlasimSinifi, string[] StokTipi, string[] HammaddeAdi, string[] HammaddeKodu, decimal[] Oran)
        {
            if (string.IsNullOrEmpty(AlasimSinifi))
            {
                ModelState.AddModelError("", "Lütfen bir alaşım sınıfı seçiniz.");
                ViewBag.StokTipleri = _context.StokTipleri.OrderBy(x => x.Ad).Select(x => x.Ad).ToList();
                return View();
            }

            if (HammaddeKodu == null || HammaddeKodu.Length == 0 || string.IsNullOrEmpty(HammaddeKodu[0]))
            {
                ModelState.AddModelError("", "Lütfen en az bir satır ekleyiniz.");
                ViewBag.StokTipleri = _context.StokTipleri.OrderBy(x => x.Ad).Select(x => x.Ad).ToList();
                return View();
            }

            for (int i = 0; i < HammaddeKodu.Length; i++)
            {
                if (!string.IsNullOrEmpty(HammaddeKodu[i]))
                {
                    var model = new AlasimHammaddeOranlari
                    {
                        AlasimSinifi = AlasimSinifi,
                        StokTipi = (StokTipi != null && i < StokTipi.Length) ? StokTipi[i] : "",
                        HammaddeKodu = HammaddeKodu[i],
                        HammaddeAdi = (HammaddeAdi != null && i < HammaddeAdi.Length) ? HammaddeAdi[i] : "",
                        Oran = (Oran != null && i < Oran.Length) ? Oran[i] : 0
                    };
                    _context.AlasimHammaddeOranlari.Add(model);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AlasimOranlari));
        }

        public async Task<IActionResult> AlasimOranlariEdit(int? id)
        {
            if (id == null) return NotFound();

            var mevcut = await _context.AlasimHammaddeOranlari
                .FirstOrDefaultAsync(x => x.Id == id);

            if (mevcut == null) return NotFound();

            var tumKayitlar = await _context.AlasimHammaddeOranlari
                .Where(x => x.AlasimSinifi == mevcut.AlasimSinifi)
                .ToListAsync();

            ViewBag.AlasimSinifi = mevcut.AlasimSinifi;
            ViewBag.StokTipleri = _context.StokTipleri
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToList();

            return View(tumKayitlar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlasimOranlariEdit(string AlasimSinifi, List<AlasimHammaddeOranlari> kayitlar)
        {
            if (string.IsNullOrEmpty(AlasimSinifi))
            {
                ModelState.AddModelError("", "Alaşım sınıfı bulunamadı.");
                return View(kayitlar);
            }

            if (kayitlar == null || kayitlar.Count == 0)
            {
                ModelState.AddModelError("", "En az bir satır bulunmalıdır.");
                return View(kayitlar);
            }

            try
            {
                var eskiKayitlar = _context.AlasimHammaddeOranlari
                    .Where(x => x.AlasimSinifi == AlasimSinifi);
                _context.AlasimHammaddeOranlari.RemoveRange(eskiKayitlar);

                foreach (var item in kayitlar)
                {
                    if (!string.IsNullOrEmpty(item.HammaddeKodu))
                    {
                        item.AlasimSinifi = AlasimSinifi;
                        item.OlusturmaTarihi = DateTime.Now;
                        item.GuncellemeTarihi = DateTime.Now;
                        _context.AlasimHammaddeOranlari.Add(item);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Alaşım oranları başarıyla güncellendi.";
                return RedirectToAction(nameof(AlasimOranlari));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + ex.Message);
                return View(kayitlar);
            }
        }

        [HttpPost, ActionName("AlasimOranlariDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlasimOranlariDeleteConfirmed(int id)
        {
            var model = await _context.AlasimHammaddeOranlari.FindAsync(id);
            if (model != null) _context.AlasimHammaddeOranlari.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AlasimOranlari));
        }

        // ==========================================
        // KALIP SARF ORANLARI (CRUD)
        // ==========================================
        public async Task<IActionResult> KalipSarfOranlari()
        {
            return View(await _context.KalipSarfOranlari.ToListAsync());
        }

        public IActionResult KalipSarfOranlariCreate()
        {
            ViewBag.StokTipleri = _context.StokTipleri
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KalipSarfOranlariCreate(string KalipCinsi, string[] StokTipi, string[] SarfAdi, string[] SarfKodu, string[] Birim, decimal[] Miktar)
        {
            if (string.IsNullOrEmpty(KalipCinsi))
            {
                ModelState.AddModelError("", "Lütfen bir kalıp cinsi seçiniz.");
                ViewBag.StokTipleri = _context.StokTipleri.OrderBy(x => x.Ad).Select(x => x.Ad).ToList();
                return View();
            }

            if (SarfKodu == null || SarfKodu.Length == 0 || string.IsNullOrEmpty(SarfKodu[0]))
            {
                ModelState.AddModelError("", "Lütfen en az bir satır ekleyiniz.");
                ViewBag.StokTipleri = _context.StokTipleri.OrderBy(x => x.Ad).Select(x => x.Ad).ToList();
                return View();
            }

            for (int i = 0; i < SarfKodu.Length; i++)
            {
                if (!string.IsNullOrEmpty(SarfKodu[i]))
                {
                    var model = new KalipSarfOranlari
                    {
                        KalipCinsi = KalipCinsi,
                        StokTipi = (StokTipi != null && i < StokTipi.Length) ? StokTipi[i] : "",
                        SarfKodu = SarfKodu[i],
                        SarfAdi = (SarfAdi != null && i < SarfAdi.Length) ? SarfAdi[i] : "",
                        Birim = (Birim != null && i < Birim.Length) ? Birim[i] : "KG",
                        Miktar = (Miktar != null && i < Miktar.Length) ? Miktar[i] : 0
                    };
                    _context.KalipSarfOranlari.Add(model);
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Kalıp sarf oranları başarıyla kaydedildi.";
            return RedirectToAction(nameof(KalipSarfOranlari));
        }

        public async Task<IActionResult> KalipSarfOranlariEdit(int? id)
        {
            if (id == null) return NotFound();

            var mevcut = await _context.KalipSarfOranlari
                .FirstOrDefaultAsync(x => x.Id == id);

            if (mevcut == null) return NotFound();

            var tumKayitlar = await _context.KalipSarfOranlari
                .Where(x => x.KalipCinsi == mevcut.KalipCinsi)
                .ToListAsync();

            ViewBag.KalipCinsi = mevcut.KalipCinsi;
            ViewBag.StokTipleri = _context.StokTipleri
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToList();

            return View(tumKayitlar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KalipSarfOranlariEdit(string KalipCinsi, List<KalipSarfOranlari> kayitlar)
        {
            if (string.IsNullOrEmpty(KalipCinsi))
            {
                ModelState.AddModelError("", "Kalıp cinsi bulunamadı.");
                return View(kayitlar);
            }

            if (kayitlar == null || kayitlar.Count == 0)
            {
                ModelState.AddModelError("", "En az bir satır bulunmalıdır.");
                return View(kayitlar);
            }

            try
            {
                var eskiKayitlar = _context.KalipSarfOranlari
                    .Where(x => x.KalipCinsi == KalipCinsi);
                _context.KalipSarfOranlari.RemoveRange(eskiKayitlar);

                foreach (var item in kayitlar)
                {
                    if (!string.IsNullOrEmpty(item.SarfKodu))
                    {
                        item.KalipCinsi = KalipCinsi;
                        item.OlusturmaTarihi = DateTime.Now;
                        item.GuncellemeTarihi = DateTime.Now;
                        _context.KalipSarfOranlari.Add(item);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Kalıp sarf oranları başarıyla güncellendi.";
                return RedirectToAction(nameof(KalipSarfOranlari));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + ex.Message);
                return View(kayitlar);
            }
        }

        [HttpPost, ActionName("KalipSarfOranlariDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KalipSarfOranlariDeleteConfirmed(int id)
        {
            var model = await _context.KalipSarfOranlari.FindAsync(id);
            if (model != null) _context.KalipSarfOranlari.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(KalipSarfOranlari));
        }

        // ==========================================
        // MAÇA SARF ORANLARI (CRUD)
        // ==========================================
        public async Task<IActionResult> MacaSarfOranlari()
        {
            return View(await _context.MacaSarfOranlari.ToListAsync());
        }

        public IActionResult MacaSarfOranlariCreate()
        {
            ViewBag.StokTipleri = _context.StokTipleri
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MacaSarfOranlariCreate(string MacaCinsi, string[] StokTipi, string[] SarfAdi, string[] SarfKodu, string[] Birim, decimal[] Miktar)
        {
            if (string.IsNullOrEmpty(MacaCinsi))
            {
                ModelState.AddModelError("", "Lütfen bir maça cinsi seçiniz.");
                ViewBag.StokTipleri = _context.StokTipleri.OrderBy(x => x.Ad).Select(x => x.Ad).ToList();
                return View();
            }

            if (SarfKodu == null || SarfKodu.Length == 0 || string.IsNullOrEmpty(SarfKodu[0]))
            {
                ModelState.AddModelError("", "Lütfen en az bir satır ekleyiniz.");
                ViewBag.StokTipleri = _context.StokTipleri.OrderBy(x => x.Ad).Select(x => x.Ad).ToList();
                return View();
            }

            for (int i = 0; i < SarfKodu.Length; i++)
            {
                if (!string.IsNullOrEmpty(SarfKodu[i]))
                {
                    var model = new MacaSarfOranlari
                    {
                        MacaCinsi = MacaCinsi,
                        StokTipi = (StokTipi != null && i < StokTipi.Length) ? StokTipi[i] : "",
                        SarfKodu = SarfKodu[i],
                        SarfAdi = (SarfAdi != null && i < SarfAdi.Length) ? SarfAdi[i] : "",
                        Birim = (Birim != null && i < Birim.Length) ? Birim[i] : "KG",
                        Miktar = (Miktar != null && i < Miktar.Length) ? Miktar[i] : 0
                    };
                    _context.MacaSarfOranlari.Add(model);
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Maça sarf oranları başarıyla kaydedildi.";
            return RedirectToAction(nameof(MacaSarfOranlari));
        }

        public async Task<IActionResult> MacaSarfOranlariEdit(int? id)
        {
            if (id == null) return NotFound();

            var mevcut = await _context.MacaSarfOranlari
                .FirstOrDefaultAsync(x => x.Id == id);

            if (mevcut == null) return NotFound();

            var tumKayitlar = await _context.MacaSarfOranlari
                .Where(x => x.MacaCinsi == mevcut.MacaCinsi)
                .ToListAsync();

            ViewBag.MacaCinsi = mevcut.MacaCinsi;
            ViewBag.StokTipleri = _context.StokTipleri
                .OrderBy(x => x.Ad)
                .Select(x => x.Ad)
                .ToList();

            return View(tumKayitlar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MacaSarfOranlariEdit(string MacaCinsi, List<MacaSarfOranlari> kayitlar)
        {
            if (string.IsNullOrEmpty(MacaCinsi))
            {
                ModelState.AddModelError("", "Maça cinsi bulunamadı.");
                return View(kayitlar);
            }

            if (kayitlar == null || kayitlar.Count == 0)
            {
                ModelState.AddModelError("", "En az bir satır bulunmalıdır.");
                return View(kayitlar);
            }

            try
            {
                var eskiKayitlar = _context.MacaSarfOranlari
                    .Where(x => x.MacaCinsi == MacaCinsi);
                _context.MacaSarfOranlari.RemoveRange(eskiKayitlar);

                foreach (var item in kayitlar)
                {
                    if (!string.IsNullOrEmpty(item.SarfKodu))
                    {
                        item.MacaCinsi = MacaCinsi;
                        item.OlusturmaTarihi = DateTime.Now;
                        item.GuncellemeTarihi = DateTime.Now;
                        _context.MacaSarfOranlari.Add(item);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Maça sarf oranları başarıyla güncellendi.";
                return RedirectToAction(nameof(MacaSarfOranlari));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + ex.Message);
                return View(kayitlar);
            }
        }

        [HttpPost, ActionName("MacaSarfOranlariDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MacaSarfOranlariDeleteConfirmed(int id)
        {
            var model = await _context.MacaSarfOranlari.FindAsync(id);
            if (model != null) _context.MacaSarfOranlari.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MacaSarfOranlari));
        }

        // ==========================================
        // ENERJİ STANDARTLARI (CRUD)
        // ==========================================
        public async Task<IActionResult> EnerjiStandartlari()
        {
            return View(await _context.EnerjiStandartlari.ToListAsync());
        }

        public IActionResult EnerjiStandartlariCreate() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnerjiStandartlariCreate(EnerjiStandartlari model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EnerjiStandartlari));
            }
            return View(model);
        }

        public async Task<IActionResult> EnerjiStandartlariEdit(int? id)
        {
            if (id == null) return NotFound();
            var model = await _context.EnerjiStandartlari.FindAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnerjiStandartlariEdit(int id, EnerjiStandartlari model)
        {
            if (id != model.Id) return NotFound();
            if (ModelState.IsValid)
            {
                model.GuncellemeTarihi = DateTime.Now;
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EnerjiStandartlari));
            }
            return View(model);
        }

        [HttpPost, ActionName("EnerjiStandartlariDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnerjiStandartlariDeleteConfirmed(int id)
        {
            var model = await _context.EnerjiStandartlari.FindAsync(id);
            if (model != null) _context.EnerjiStandartlari.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EnerjiStandartlari));
        }

        // ==========================================
        // GET STOK GETİR
        // ==========================================
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
        [HttpGet]
        public async Task<IActionResult> GetSonAlasimOranlari(string alasimSinifi)
        {
            if (string.IsNullOrEmpty(alasimSinifi))
                return Json(new List<object>());

            var sonKayitlar = await _context.AlasimHammaddeOranlari
                .Where(x => x.AlasimSinifi == alasimSinifi)
                .OrderByDescending(x => x.Id)
                .Take(10) // Son 10 kaydı al
                .Select(x => new { x.StokTipi, x.HammaddeAdi, x.HammaddeKodu, x.Oran })
                .ToListAsync();

            return Json(sonKayitlar);
        }
        [HttpGet]
        public async Task<IActionResult> GetSonKalipSarfOranlari(string kalipCinsi)
        {
            if (string.IsNullOrEmpty(kalipCinsi))
                return Json(new List<object>());

            var sonKayitlar = await _context.KalipSarfOranlari
                .Where(x => x.KalipCinsi == kalipCinsi)
                .OrderByDescending(x => x.Id)
                .Take(10)
                .Select(x => new { x.StokTipi, x.SarfAdi, x.SarfKodu, x.Birim, x.Miktar })
                .ToListAsync();

            return Json(sonKayitlar);
        }
        [HttpGet]
        public async Task<IActionResult> GetSonMacaSarfOranlari(string macaCinsi)
        {
            if (string.IsNullOrEmpty(macaCinsi))
                return Json(new List<object>());

            var sonKayitlar = await _context.MacaSarfOranlari
                .Where(x => x.MacaCinsi == macaCinsi)
                .OrderByDescending(x => x.Id)
                .Take(10)
                .Select(x => new { x.StokTipi, x.SarfAdi, x.SarfKodu, x.Birim, x.Miktar })
                .ToListAsync();

            return Json(sonKayitlar);
        }
        [HttpGet]
        public async Task<IActionResult> GetBesleyiciBilgisi(string besleyiciKodu)
        {
            if (string.IsNullOrEmpty(besleyiciKodu))
                return Json(new { });

            var besleyici = await _context.Stoklar
                .Where(s => s.StokKodu == besleyiciKodu)
                .Select(s => new { s.StokKodu, s.StokAdi, s.SonAlisFiyati, s.Aciklama })
                .FirstOrDefaultAsync();

            return Json(besleyici);
        }
    }
}