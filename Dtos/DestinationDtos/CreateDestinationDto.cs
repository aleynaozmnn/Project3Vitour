using System.ComponentModel.DataAnnotations;

namespace Project3Vitour.Dtos.DestinationDtos
{
    public class CreateDestinationDto
    {
        [Required(ErrorMessage = "Şehir adı boş geçilemez.")]
        [MinLength(2, ErrorMessage = "Şehir adı en az 2 karakter olmalıdır.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Ülke adı boş geçilemez.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Görsel URL alanı zorunludur.")]
        public string ImageUrl { get; set; }

        [Range(0, 500000, ErrorMessage = "Fiyat 0 ile 500.000 arasında olmalıdır.")]
        public decimal Price { get; set; }

        [Range(1, 150, ErrorMessage = "Kapasite 1 ile 150 kişi arasında olmalıdır.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Konaklama süresi (Gün/Gece) boş geçilemez.")]
        public string DayNight { get; set; }

        // Yeni eklenen tur varsayılan olarak aktif gelsin
        public bool Status { get; set; } = true;
    }
}
