using MongoDB.Driver;
using PinterestClone.Infrastructure.Data;
using PinterestClone.Infrastructure.Data.Models;

namespace PinterestClone.Application.Services.UserService;


public class UserService : IUserService
{
    private readonly IMongoData _mongoData;
    private readonly IMongoCollection<User> _userCollection;
    public const string USER_COLLECTION = "users";

    public UserService(IMongoData mongoData)
    {
        _mongoData = mongoData;
        _userCollection = mongoData.Database.GetCollection<User>(USER_COLLECTION);
    }
    public async Task<User> Create(User user)
    {
        await _userCollection.InsertOneAsync(user);
        return user;
    }

    public async Task<User> FindByEmail(string email)
    {
        return await (await _userCollection.FindAsync(u => u.Email == email))
            .FirstOrDefaultAsync();
    }

    public async Task<User> FindById(string id)
    {
        return await (await _userCollection.FindAsync(u => u.UserId == id))
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsEmailUnique(string email)
    {
        return !await (await _userCollection.FindAsync(u => u.Email == email)).AnyAsync();
    }

    public async Task<User> Update(string id, User user)
    {
        if(!(await _userCollection.FindAsync(u => u.UserId == id)).Any())
        {
            throw new Exception("User not found!");
        }
        await _userCollection.ReplaceOneAsync(Builders<User>
            .Filter.Eq(u => u.UserId, id), 
            user);
        
        return user;
    }
}