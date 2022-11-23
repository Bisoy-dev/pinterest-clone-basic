namespace PinterestClone.Infrastructure.Data.Models;

public class Board
{
    [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string BoardId { get; set; } = null!;
    [BsonElement("user_id"), BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;
    [BsonElement("title"), BsonRepresentation(BsonType.String)]
    public string Title { get; set; } = null!;
    [BsonElement("description"), BsonRepresentation(BsonType.String)]
    public string? Description { get; set; }
    [BsonElement("thumbnails")]
    public List<string> Thumbnails { get; set; } = new();
    [BsonElement("date")]
    public BasicDate Date { get; set; } = new();

}