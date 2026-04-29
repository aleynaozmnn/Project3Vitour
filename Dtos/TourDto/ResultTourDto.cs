namespace Project3Vitour.Dtos.TourDto
{
    public class ResultTourDto
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
        public int CurrentReservationCount { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public String TitleEN { get; set; } 

        public String DescriptionEN { get; set; }
    }
}
