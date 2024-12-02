using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using NganHangDe_Backend.ServerModels;
using NganHangDe_Backend.StaticModels;
using Swashbuckle.AspNetCore.Annotations;

namespace NganHangDe_Backend.Models
{
    public class Question 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [SwaggerIgnore]
        public string? Id { get; set; }
        public string Type { get; set; } = null!;
        public int Class { get; set; }
        //public Subject? Subject { get; set; } = null!;
        [BsonRepresentation(BsonType.ObjectId)]
        public string SubjectId { get; set; } = null!;
        public string Difficulty { get; set; } = null!;
        public string[] KnowledgeScope { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string? Title { get; set; }
        public Answer[] Answers { get; set; } = null!;
        [BsonIgnoreIfNull] // don't save to database if null
        public string? Explanation { get; set; }
        [SwaggerIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [BsonIgnoreIfNull]
        [SwaggerIgnore]
        public DateTime? UpdatedAt { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [SwaggerIgnore]
        public string? CreatedBy { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        [SwaggerIgnore]
        public string? UpdatedBy { get; set; }


        //public static Question CreateQuestionModel(InputQuestion input)
        //{
        //    // get all value of static class QuestionType

        //    var keys = typeof(QuestionType).GetFields().Select(f => f.GetValue(null)).Cast<string>().ToList();

        //    if (!keys.Contains(input.Type))
        //    {
        //        throw new ArgumentOutOfRangeException("Type", $"type must be one of {string.Join(", ", keys)}");
        //    }

        //    return new Question
        //    {
        //        Type = input.Type,
        //        Class = input.Class,
        //        #pragma warning disable CS8601 // Possible null reference assignment.
        //        SubjectId = input.SubjectId,
        //        KnowledgeScope = input.KnowledgeScope,
        //        Title = input.Title,
        //        Answers = input.Answers,
        //        Explanation = input.Explanation,
        //        Difficulty = input.Difficulty
        //    };
        //}

    }
}
