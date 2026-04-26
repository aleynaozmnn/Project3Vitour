using System.ComponentModel.DataAnnotations;

namespace Project3Vitour.Dtos.TourPlanDto
{
    public class CreateTourPlanDto
    {
        public string TourId { get; set; }

        [Required(ErrorMessage = "Gün numarası boş geçilemez")]
        public int DayNumber { get; set; }

        [Required(ErrorMessage = "Plan başlığı boş geçilemez")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama alanı boş geçilemez")]
        public string Description { get; set; }
    }
}
