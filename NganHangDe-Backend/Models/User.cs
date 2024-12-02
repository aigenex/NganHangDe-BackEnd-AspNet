using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace NganHangDe_Backend.Models
{
    public class User
    {
 

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [SwaggerSchema(ReadOnly = true)]
        public string? Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        [BsonIgnoreIfNull]
        public byte[]? Avatar { get; set; }
        public HashSet<string> Roles { get; set; }

        // hide field in swagger
        [SwaggerSchema(ReadOnly = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [SwaggerSchema(ReadOnly = true)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Roles = new HashSet<string>();
        }

        public void SetProfilePicture(byte[] profilePicture)
        {
            Avatar = profilePicture;
        }
    }
}
