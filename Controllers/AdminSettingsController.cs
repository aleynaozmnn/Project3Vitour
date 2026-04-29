using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.SettingsDtos;
using Project3Vitour.Services.SetingsService;
using Project3Vitour.Services.SetingsService;  

namespace Project3Vitour.Controllers
{
    public class AdminSettingsController : Controller
    {
        /*İlk 2 satırda yaptığım işlem şudur->Dependency Injection:
        Controller burda Service,git bana veriyi getir der*/
        private readonly ISettingsService _settingsService;
        public AdminSettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        
        [HttpGet]
        /*Kullanıcı ayarlar sayfasına tıkladığında burası çalışır
         Sadece veri çek+kullanıcıya göster mantığını taşır*/
        public async Task<IActionResult> Index()
        {

            var values = await _settingsService.GetSettingsAsync();


            if (values == null)
            {
                return View(new UpdateSettingsDto());
            }

            return View(values);
        }


        [HttpPost]
        /*Kullanıcı kaydet butonuna bastığında çalışan kısımdır
         Dışardan UpdateSettingsDto-bir paket veri alır ve bunu işler*/
        public async Task<IActionResult> UpdateProfile(UpdateSettingsDto updateSettingsDto)
        {
            // Veritabanındaki mevcut şifreyi korumak için kontrol ekliyoruz
            var current = await _settingsService.GetSettingsAsync();

            /*Bunu yapma sebebim ise şudur->Kullanıcıher bilgisini güncellemek istemeyebilir
             Bu kontrol olmasaydı kullanıcı mailini güncelleyip şifresini boş bıraktığında db deki şifre alanını silip yerine boşluk konabilirdi
            Bu mantık hatasını engellemek için bu iş mantığı(Business Logic)*/
            if (string.IsNullOrEmpty(updateSettingsDto.NewPassword))
            {
                updateSettingsDto.NewPassword = current.NewPassword;
            }

            await _settingsService.UpdateSettingsAsync(updateSettingsDto);
            TempData["SuccessMessage"] = "Profil ve Güvenlik bilgileri başarıyla güncellendi.";
            return RedirectToAction("Index");
        }
    }
}