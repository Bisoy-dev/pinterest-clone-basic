
namespace PinterestClone.Infrastructure.Data.Models;

public class User
{
    [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;
    [BsonElement("email"), BsonRepresentation(BsonType.String)]
    public string Email { get; set; } = null!;
    [BsonElement("password_hash"), BsonRepresentation(BsonType.String)]
    public string PasswordHash { get; set; } = null!;
    [BsonElement("password_salt"), BsonRepresentation(BsonType.String)]
    public string PasswordSalt { get; set; } = null!;
    [BsonElement("images")]
    public List<string> Images { get; set; } = new();
    [BsonElement("date_created"), BsonRepresentation(BsonType.DateTime)]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}