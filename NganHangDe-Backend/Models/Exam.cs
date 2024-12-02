using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;

namespace NganHangDe_Backend.Models
{
    public class Exam
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [SwaggerIgnore]
        public string? Id { get; set; }
        [BsonIgnoreIfNull]
        public string? Code { get; set; }
        public string Title { get; set; } = null!;
        public int Duration { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string SubjectId { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string? Description { get; set; }
        public bool IsOfficial { get; set; } = false;
        public string[] KnowledgeScope = null!;
        [BsonRepresentation(BsonType.ObjectId)]
        public HashSet<string> QuestionIds { get; set; } = null!;
        [SwaggerIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [BsonIgnoreIfNull]
        [SwaggerIgnore]
        public DateTime? UpdatedAt { get; set; }

    }
}
