using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;

namespace NganHangDe_Backend.Models
{
    public class QuestionGroup
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string SubjectId { get; set; } = null!;
        public List<string> KnowledgeScope { get; set; } = null!;

        [BsonIgnoreIfNull]
        public string? Source { get; set; }
        [BsonIgnoreIfNull]
        public string? Method { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [BsonIgnoreIfNull] // don't save to database if null
        [SwaggerSchema(ReadOnly = true)]
        public DateTime? UpdatedAt { get; set; }


    }
}
