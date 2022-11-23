namespace PinterestClone.Infrastructure.Data.Models;

public class Pin
{
    [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string PinId { get; set; } = null!;
    [BsonElement("user_id"), BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;
    [BsonElement("img"), BsonRepresentation(BsonType.String)]
    public string Image { get; set; } = null!;
    [BsonElement("title"), BsonRepresentation(BsonType.String)]
    public string Title { get; set; } = null!;
    [BsonElement("description"), BsonRepresentation(BsonType.String)]
    public string Description { get; set; } = string.Empty;
    [BsonElement("distination_link"), BsonRepresentation(BsonType.String)]
    public string DistinationLink { get; set; } = string.Empty;
    [BsonElement("likes")]
    public HashSet<string> Likes { get; set; } = new();
    [BsonElement("comments")]
    public List<string> Comments { get; set; } = new();
    [BsonElement("date")]
    public BasicDate Date { get; set; } = new();

    public Pin(
        string userId, 
        string image,
        string description,
        string title,
        string distinationLink = "")
    {
        UserId = userId;
        Image = image;
        Description = description;
        DistinationLink = distinationLink;
        Title = title;
    }

    public Pin(){}
}