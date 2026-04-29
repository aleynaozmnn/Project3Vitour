using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.SettingsDtos;
using Project3Vitour.Services.SetingsService;
using System.Security.Claims;

namespace Project3Vitour.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISettingsService _settingsService;

        public LoginController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UpdateSettingsDto loginDto)
        {
            var settings = await _settingsService.GetSettingsAsync();
            //db deki gerçek verilerle  kullanıcının formda yazdığı bilgileri karşılaştırırım
            if (settings != null && settings.Username == loginDto.Username && settings.NewPassword == loginDto.NewPassword)
            {
                 

                //Claim->Bir kişinin kimliğine dair bilgilerdir
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, settings.Username),
                    new Claim(ClaimTypes.Email, settings.Email ?? ""),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                //Oluşturduğum etiketleri bir araya getirip bir kimlik akrtı oluşturdum
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // Beni hatırla seçeneği gibi çalışır
                    IsPersistent = true,
                    // 60 dakika sonra oturum düşer
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60) 
                };

                /* Sistem oluşturduğum o k,mlik kartını tarayıcıya çerez olarak fırlatır.
                Artık kullanıcı her yeni sayfa istediğinde tarayıcı bu çerezi yollar sisteme.*/
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "AdminSettings");
            }

            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        // Çıkış yapma metodu (Bunu da eklemiş olalım)
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}