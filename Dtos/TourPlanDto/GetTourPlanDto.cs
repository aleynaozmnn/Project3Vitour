namespace Project3Vitour.Dtos.TourPlanDto
{

    //Entity:db deki ham yapı
    //Controller:Servisi çağıracak
    //Service:db den veriyi çekip DTO'ya dönüştürece ulaştıran katman
    //DTO:view a ekrana taşıdığın temiz veri

    public class GetTourPlanDto
    {
        public string TourId { get; set; }
        public string TourPlanId { get; set; }
        public int DayNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
