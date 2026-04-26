namespace Project3Vitour.Dtos.TourDto
{
    public class GetTourByIdDto
    {
        public String TourId { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String CoverImageUrl { get; set; }
        public String Badge { get; set; }
        public int DayCount { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public bool IsStatus { get; set; }
        public List<string> Images { get; set; }
        public string MapLocationImageUrl { get; set; }
        public string CategoryId { get; set; }

        // Yeni eklenen alan: Mevcut rezervasyon miktarını tutar
        public int CurrentReservationCount { get; set; }
    }
}
