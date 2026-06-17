using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Erp.Data;
using Erp.Data.Entities;

namespace Erp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ErpDbContext _context;

        public AuthController(ErpDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Eğer kullanıcı zaten giriş yapmışsa ana sayfaya yönlendir
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Kullanıcıyı veritabanında kontrol et
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);

            if (user == null)
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
                return View();
            }

            // Kullanıcı pasif ise giriş yapmasın
            if (!user.IsActive)
            {
                ViewBag.Error = "Bu kullanıcı hesabı pasif durumdadır!";
                return View();
            }

            // Cookie için claim'ler oluştur
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.GivenName, user.FullName ?? ""),
                new Claim("FullName", user.FullName ?? "")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Cookie ayarları - SlidingExpiration burada değil, Program.cs'de tanımlı
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,           // Kalıcı çerez
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)  // 8 saat geçerli
                // SlidingExpiration Program.cs'de tanımlandı
            };

            // Giriş yap (cookie oluştur)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Kullanıcı adını TempData'ya kaydet (Layout'da göstermek için)
            TempData["UserName"] = user.Username;
            TempData["FullName"] = user.FullName;

            // Ana sayfaya yönlendir
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            // Çıkış yap (cookie'yi temizle)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Çıkış yapıldı bilgisini göster
            TempData["LogoutMessage"] = "Başarıyla çıkış yaptınız.";

            // Login sayfasına yönlendir
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}