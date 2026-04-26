using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project3Vitour.Entities
{
    public class Destination
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DestinationId { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public string DayNight { get; set; }
        public bool Status { get; set; }
    }
}
