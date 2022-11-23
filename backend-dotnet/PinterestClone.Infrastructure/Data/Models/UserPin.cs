namespace PinterestClone.Infrastructure.Data.Models;

public class UserPin
{
    [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string UserPinId { get; set; } = null!;
    [BsonElement("user_id"), BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;
    [BsonElement("date")]
    public BasicDate Date { get; set; } = new();
}