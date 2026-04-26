using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Project3Vitour.Entities
{
    [BsonIgnoreExtraElements]
    public class TourPlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourPlanId { get; set; }
        public string TourId { get; set; }
        public int DayNumber { get; set; }
        public string Title { get; set; }
        public string Description{ get; set; }
    }
}
