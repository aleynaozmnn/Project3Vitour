namespace Project3Vitour.Dtos.ReviewDtos
{
    public class ResultReviewDto
    {
        public string ReviewId { get; set; }
        public string NameSurname { get; set; }
        public string Detail { get; set; }
        public int Score { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Status { get; set; }
        public string TourId { get; set; }
        public int GuideRating { get; set; }
        public int AccommodationRating { get; set; }
        public int TransportationRating { get; set; }
        public int ComfortRating { get; set; }
        public string Description { get; set; }
    }
}
