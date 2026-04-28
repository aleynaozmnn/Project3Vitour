using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project3Vitour.Entities
{
    public class Tour
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String TourId { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String CoverImageUrl { get; set; }
        public string MapLocationImageUrl { get; set; }
        public String Badge { get; set; }
        public int DayCount { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public bool IsStatus { get; set; }
        public List<string> Images { get; set; }
        public string CategoryId { get; set; }
        public string TitleEN { get; set; }
        public string DescriptionEN { get; set; }

    }
}
