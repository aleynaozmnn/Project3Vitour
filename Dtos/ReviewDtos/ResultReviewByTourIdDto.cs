namespace Project3Vitour.Dtos.ReviewDtos
{
    public class ResultReviewByTourIdDto
    {
        public string ReviewId { get; set; }
        public string NameSurname { get; set; }
        public string Detail { get; set; }
        public int Score { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Status { get; set; }
        public string TourId { get; set; }

        // Eksik olan ve hataya sebep olan alanlar:
        public int GuideRating { get; set; }
        public int AccommodationRating { get; set; }
        public int TransportationRating { get; set; }
        public int ComfortRating { get; set; }
    }
}