using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NganHangDe_Backend.Models
{
    public class Answer
    {
        public string Content { get; set; } = null!;
        public bool IsCorrect { get; set; } = false;
    }
}
