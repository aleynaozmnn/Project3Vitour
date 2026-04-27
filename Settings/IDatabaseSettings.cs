namespace Project3Vitour.Settings
{
    //Interface:priz(kendine göre sınırları belirler),Class:şarj adaptörü(bu sınırlara uymak zorundadır)
    public interface IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string TourCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ReviewCollectionName { get; set; }
        public string TourPlanCollectionName { get; set; }
        public string ReservationCollectionName { get; set; }
        public string ImageCollectionName { get; set; }
        public string DestinationCollectionName { get; set; }
        public string SettingsCollectionName { get; set; }
    }
}
