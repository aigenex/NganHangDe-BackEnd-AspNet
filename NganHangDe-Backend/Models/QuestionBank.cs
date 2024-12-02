using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace NganHangDe_Backend.Models
{

    public class QuestionBank
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [SwaggerIgnore]
        public string? Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [SwaggerIgnore]
        [BsonIgnore]
        public Question[] Questions { get; set; } = null!;


        [SwaggerIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string[] QuestionIds { get; set; } 


        [BsonRepresentation(BsonType.ObjectId)]
        public string SubjectId { get; set; } = null!;

 
        [SwaggerIgnore]
        public Subject? Subject { get; set; }

        public int Class { get; set; }

        [SwaggerIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [BsonIgnoreIfNull]
        [SwaggerIgnore]
        public DateTime? UpdatedAt { get; set; }


        [BsonIgnore]
        public int? QuestionCount = 0;

        public QuestionBank(string title, string? description, string subjectId, int @class)
        {
            Title = title;
            Description = description;
            SubjectId = subjectId;
            Class = @class;
            QuestionIds = Array.Empty<string>();
        }

        //private int GetQuestionCount()
        //{
        //    // check if Items is stimuluses or a single question
        //    var count = 0;
        //    foreach (var item in Items)
        //    {
        //        if (item is Stimulus stimulus)
        //        {
        //            count += stimulus.GetQuestionCount();
        //        }
        //        else if (item is Question)
        //        {
        //            count++;
        //        }
        //    }
        //    return count;
        //}

        public void AddItem(string questionId)
        {
            _ = this.QuestionIds.Append(questionId);
        }

    }
}
