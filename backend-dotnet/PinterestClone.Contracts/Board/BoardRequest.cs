namespace PinterestClone.Contracts.Board;

public record SaveBoard(
    string UserId, 
    string Title,
    string? Description);