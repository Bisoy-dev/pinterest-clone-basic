using PinterestClone.Application.Services.UserService;
using PinterestClone.Infrastructure.Data;
using PinterestClone.Infrastructure.Data.Models;
using PinterestClone.Infrastructure.Utils.Encryptors;

namespace PinterestClone.Application.Services.Authentication;

public class UserManager : IUserManager
{
    private readonly IMongoData _mongoData;
    private readonly IEncryptor _encryptor;
    private readonly IUserService _userService;

    public UserManager(
        IMongoData mongoData,
        IEncryptor encryptor,
        IUserService userService)
    {
        _mongoData = mongoData;
        _encryptor = encryptor;
        _userService = userService;
    }
    public async Task<UserCreateResult> Create(string email, string password)
    {
        var errors = new List<string>();

        if(!await _userService.IsEmailUnique(email))
        {
            errors.Add("Email is already taken.");
            return new UserCreateResult(false, errors, new User());
        }
        var passwordResult = _encryptor.OnEncrypt(password);

        var user = new User
        {
            Email = email,
            PasswordHash = Convert.ToBase64String(passwordResult.Hash),
            PasswordSalt = Convert.ToBase64String(passwordResult.Salt),
            DateCreated = DateTime.UtcNow
        };

        var result = await _userService.Create(user);
        
        return new UserCreateResult(true, errors, result);
    }

    public async Task<UserSigningInResult> SignIn(string email, string password)
    {
        var errors = new List<string>();
        var user = await _userService.FindByEmail(email);
        if(user is null)
        {
            errors.Add("Incorrect email address.");
            return new UserSigningInResult(false, errors);
        }

        if(!_encryptor.Verify(password, Convert.FromBase64String(user.PasswordHash), Convert.FromBase64String(user.PasswordSalt)))
        {
            errors.Add("Incorrect password.");
            return new UserSigningInResult(false, errors);
        }

        return new UserSigningInResult(true, errors);

    }
}