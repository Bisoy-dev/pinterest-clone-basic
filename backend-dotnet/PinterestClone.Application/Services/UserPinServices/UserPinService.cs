using MongoDB.Driver;
using PinterestClone.Infrastructure.Data;
using PinterestClone.Infrastructure.Data.Models;

namespace PinterestClone.Application.Services.UserPinServices;


public class UserPinService : IUserPinService
{
    private readonly IMongoData _mongoData;
    private readonly IMongoCollection<UserPin> _userPinCollection;
    public const string USERPIN_COLLECTION = "user_pins";

    public UserPinService(IMongoData mongoData)
    {
        _mongoData = mongoData;
        _userPinCollection = mongoData.Database.GetCollection<UserPin>(USERPIN_COLLECTION);
    }
    public async Task<UserPin> Create(UserPin userPin)
    {
        await _userPinCollection.InsertOneAsync(userPin);
        return userPin;
    }

    public async Task<UserPin> FindById(string userPinId)
    {
        return await (await _userPinCollection
            .FindAsync(up => up.UserPinId == userPinId))
            .FirstOrDefaultAsync();
    }

    public async Task<UserPin> FindByUserId(string userId)
    {
        return await (await _userPinCollection
            .FindAsync(up => up.UserId == userId))
            .FirstOrDefaultAsync();
    }

    public async Task<UserPin> Update(string userPinId, UserPin userPin)
    {
        await _userPinCollection.ReplaceOneAsync(Builders<UserPin>
            .Filter.Eq(up => up.UserPinId, userPinId),
            userPin);
        
        return userPin;
    }
}