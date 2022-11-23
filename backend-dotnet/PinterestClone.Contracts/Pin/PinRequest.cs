namespace PinterestClone.Contracts.Pin;

public record UploadPinRequest(
    string UserId,
    string Title,
    string Description,
    string DistinationLink,
    string? BoardId);

public record LikePinRequest(string PinId, string UserId);