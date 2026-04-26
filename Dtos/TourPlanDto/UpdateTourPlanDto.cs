using System.ComponentModel.DataAnnotations;

namespace Project3Vitour.Dtos.TourPlanDto
{
    public class UpdateTourPlanDto
    {
        public string TourPlanId { get; set; }
        public string TourId { get; set; }

        [Required(ErrorMessage = "Gün numarası zorunludur")]
        public int DayNumber { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur")]
        public string Description { get; set; }
    }
}
