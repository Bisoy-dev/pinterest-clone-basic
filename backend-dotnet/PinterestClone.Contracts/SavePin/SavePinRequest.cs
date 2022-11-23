namespace PinterestClone.Contracts.SavePin;

public record Save(
        string UserId, 
        string? BoardId, 
        string PinId);