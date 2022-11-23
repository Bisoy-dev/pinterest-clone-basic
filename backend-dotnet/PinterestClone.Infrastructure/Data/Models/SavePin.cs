namespace PinterestClone.Infrastructure.Data.Models;

public class SavePin
{
    [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string SavePinId { get; set; } = null!;
    [BsonElement("user_pin_id"), BsonRepresentation(BsonType.ObjectId)]
    public string UserPinId { get; set; } = null!;
    [BsonElement("pin_id"), BsonRepresentation(BsonType.ObjectId)]
    public string PinId { get; set; } = null!;
    [BsonElement("board_id"), BsonRepresentation(BsonType.ObjectId)]
    public string? BoardId { get; set; }
    [BsonElement("type"), BsonRepresentation(BsonType.String)]
    public string Type { get; set; } = null!;
    [BsonElement("date")]
    public BasicDate Date { get; set; } = new();

    public SavePin(){}
    public SavePin(
        string userPinId, 
        string pinId, 
        string? boardId, 
        string type)
    {
        UserPinId = userPinId;
        PinId = pinId;
        BoardId = boardId;
        Type = type;
    }
}