using Microsoft.AspNetCore.Http; // IFormFile için gerekli

namespace Project3Vitour.Dtos.SettingsDtos
{
    public class UpdateSettingsDto
    {
        /*Dto sayesinde veritabanında ne olursa
        olsun son kullanıcıya sadece belli başlı bir şeyin iznini verebilirsin*/
        public string SettingID { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }

      
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        
        public string SiteTitle { get; set; }
        public string FooterText { get; set; }
        public string ContactEmail { get; set; }

         
        public string ImageUrl { get; set; }
        /*IFormFile->Kullanıcının bilgisayardan seçtiği dosyayı,
         sunucuya taşımak için kullandığım geçici bir kutu,tepsi*/
        public IFormFile ImageFile { get; set; }
    }
}