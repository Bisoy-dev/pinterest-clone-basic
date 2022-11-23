namespace PinterestClone.Infrastructure.Data.Models;

public class BasicDate
{
    [BsonElement("date_created"), BsonRepresentation(BsonType.DateTime)]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    [BsonElement("date_modified"), BsonRepresentation(BsonType.DateTime)]
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}