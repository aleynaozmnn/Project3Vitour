using Microsoft.AspNetCore.Http; // IFormFile için gerekli

namespace Project3Vitour.Dtos.SettingsDtos
{
    public class UpdateSettingsDto
    {
        public string SettingID { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }

        // Şifre Ayarları
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        // Site Genel Ayarları  
        public string SiteTitle { get; set; }
        public string FooterText { get; set; }
        public string ContactEmail { get; set; }

        // --- FOTOĞRAF İÇİN EKLENEN ALANLAR ---

        // Veritabanında saklanacak olan dosya yolu (Örn: /images/profil.jpg)
        public string ImageUrl { get; set; }

        // Formdan gelecek olan gerçek dosya (Yükleme işlemi için)
        // [NotMapped] gibi düşünebilirsin, sadece taşıyıcıdır.
        public IFormFile ImageFile { get; set; }
    }
}