namespace PinterestClone.Contracts.User;


public record UserRequest(string Email, string Password);

public record UserResult(string Email, string Token);