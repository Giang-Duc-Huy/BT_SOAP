using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BT_4.Models
{
    public class Book
    {
        [BsonId]                                   // Primary key
        [BsonRepresentation(BsonType.ObjectId)]    // Cho phép truyền ID dạng string
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string BookName { get; set; } = null!;

        public decimal Price { get; set; }

        public string Category { get; set; } = null!;

        public string Author { get; set; } = null!;
    }
}
