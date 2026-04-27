using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.SettingsDtos;
using Project3Vitour.Services.SetingsService;
using Project3Vitour.Services.SetingsService; // Servisinizin namespace'i

namespace Project3Vitour.Controllers
{
    public class AdminSettingsController : Controller
    {
        private readonly ISettingsService _settingsService;

        // Constructor Injection - Servisi buraya bağlıyoruz
        public AdminSettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        // GET: Ayarlar Sayfası
        [HttpGet]
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
        public async Task<IActionResult> UpdateProfile(UpdateSettingsDto updateSettingsDto)
        {
            // Veritabanındaki mevcut şifreyi korumak için kontrol ekliyoruz
            var current = await _settingsService.GetSettingsAsync();

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