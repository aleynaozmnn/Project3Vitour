using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.SettingsDtos;
using Project3Vitour.Services.SetingsService;
using System.Security.Claims;
using Project3Vitour.Services.MailServices;

namespace Project3Vitour.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISettingsService _settingsService;
        private readonly IMailService _mailService;

        public LoginController(ISettingsService settingsService,IMailService mailService)
        {
            _settingsService = settingsService;
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        //*******************************************************************************************************************************
        //MAİL DOĞRULAMA EKLEMEDEN ÖNCEKİ INDEX POST METHODUMDU
        /*public async Task<IActionResult> Index(UpdateSettingsDto loginDto)
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

                // Sistem oluşturduğum o kimlik kartını tarayıcıya çerez olarak fırlatır.
                Artık kullanıcı her yeni sayfa istediğinde tarayıcı bu çerezi yollar sisteme.
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "AdminSettings");
            }

            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }   */

        //*************************************************************************************************************************************

        //YENİ**
        public async Task<IActionResult>Index(UpdateSettingsDto logindto)
        {
            var settings=await _settingsService.GetSettingsAsync();
            if(settings!=null && settings.Username==logindto.Username &&settings.NewPassword==logindto.NewPassword)
            {
                Random random=new Random();
                string code = random.Next(100000, 999999).ToString();

                //Kodu db ye kaydediyorum
                settings.ConfirmCode = code;
                await _settingsService.UpdateSettingsAsync(settings);
                TempData["GeneratedCode"] = code;
                _mailService.SendMail(settings.Email, "Vitour Giriş Onay Kodunuz", $"Sisteme giriş için onay kodunuz: {code}");
                 
                return RedirectToAction("ConfirmEmail");

            }
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı girilmiştir!");
            return View();
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult ConfirmEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string code)
        {
            var sessionCode = TempData["GeneratedCode"] as string;
            // var settings = await _settingsService.GetSettingsAsync();
            if (string.IsNullOrEmpty(sessionCode))
            {
                var settings = await _settingsService.GetSettingsAsync();
                sessionCode = settings.ConfirmCode;
            }

            if (sessionCode != null && sessionCode.Trim() == code?.Trim())
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,"Admin"),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
               

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "AdminSettings");

            }
            TempData["GeneratedCode"] = sessionCode;
            ViewBag.Error = "Girdiğiniz onay kodu hatalı! (Gelen: " + code + ")";
            return View();
        }
    }
}