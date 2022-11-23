using PinterestClone.Infrastructure.Data.Models;

namespace PinterestClone.Application.Services.Authentication;

public interface IUserManager
{
    Task<UserCreateResult> Create(string email, string password);
    Task<UserSigningInResult> SignIn(string email, string password); 
}

public record UserCreateResult(bool IsSucceed,
    List<string> Errors, User User );

public record UserSigningInResult(bool IsSucceed, List<string> Errors);
