namespace Project3Vitour.Dtos.ReservationDto
{
    public class ResultReservationDto
    {
        public string ReservationId { get; set; }
        public string TourId { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int PersonCount { get; set; }
        public DateTime ReservationDate { get; set; }
        public string Description { get; set; }
    }
}
