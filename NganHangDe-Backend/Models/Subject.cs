using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NganHangDe_Backend.Models
{
    public class Subject 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } 
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [BsonIgnoreIfNull]
        public DateTime? UpdatedAt { get; set; }
    }
}
