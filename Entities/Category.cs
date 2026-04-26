using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace Project3Vitour.Entities
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String CategoryId { get; set; }
        public String CategoryName{ get; set; }
        public bool CategoryStatus { get; set; }
    }
}
