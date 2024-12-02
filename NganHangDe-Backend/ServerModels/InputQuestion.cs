using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NganHangDe_Backend.Models;
using NganHangDe_Backend.StaticModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NganHangDe_Backend.ServerModels
{
    public class InputQuestion
    {
        [Required]
        [DefaultValue("easy")]
        public string Type { get; set; } 
        [Required]
        [DefaultValue(10)]
        public int Class { get; set; }
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? SubjectId { get; set; } = null!;
        public string[] KnowledgeScope { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public Answer[] Answers { get; set; } = null!;
        public string Explanation { get; set; } = string.Empty;
        [Required]
        public string Difficulty { get; set; } = null!;
    }
}
