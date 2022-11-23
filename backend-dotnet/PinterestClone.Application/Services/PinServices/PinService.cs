using MongoDB.Driver;
using PinterestClone.Application.Services.SavePinServices;
using PinterestClone.Infrastructure.Data;
using PinterestClone.Infrastructure.Data.Models;

namespace PinterestClone.Application.Services.PinServices;

public class PinService : IPinService
{
    private readonly IMongoData _mongoData;
    private readonly IMongoCollection<Pin> _pinCollection;
    public const string PIN_COLLECTION = "pins";

    public PinService(IMongoData mongoData)
    {
        _mongoData = mongoData;
        _pinCollection = mongoData.Database.GetCollection<Pin>(PIN_COLLECTION);
    }
    public async Task<Pin> Create(Pin pin)
    {
        await _pinCollection.InsertOneAsync(pin);

        return pin;
    }

    public async Task<bool> Delete(string id)
    {
        if(await FindById(id) is null)
        {
            return false;
        }

        await _pinCollection.DeleteOneAsync(Builders<Pin>
            .Filter.Eq(p => p.PinId, id));

        return true;
    }

    public async Task<Pin> FindById(string id)
    {
        return await (await _pinCollection.FindAsync(p => p.PinId == id))
            .FirstOrDefaultAsync();
    }

    public async Task<List<Pin>> GetUserPins(string userId)
    {
        return await (await _pinCollection.FindAsync(p => p.UserId == userId))
            .ToListAsync();
    }

    public async Task<(bool, string)> Update(string id, Pin updatedPin)
    {
        if(await FindById(id) is null)
        {
            return (false, "Pin not found.");
        }

        await _pinCollection.ReplaceOneAsync(Builders<Pin>
            .Filter.Eq(p => p.PinId, id), updatedPin);

        return (true, "Successfully updated.");
    }

    public async Task<List<PinLookedUp>> GetAll()
    {
        var result = await (from p in _pinCollection.AsQueryable()
            join u in _mongoData.Database.GetCollection<User>(UserService.UserService.USER_COLLECTION)
            .AsQueryable() on p.UserId equals u.UserId
            select new PinLookedUp 
            {
                PinId = p.PinId,
                UserId = p.UserId,
                Likes = p.Likes,
                Description = p.Description,
                Title = p.Title,
                DistinationLink = p.DistinationLink,
                Image = p.Image,
                Comments = p.Comments,
                Date = p.Date,
                User = new UserProps { UserId = u.UserId, Email = u.Email, Images = u.Images }
            })
            .ToAsyncEnumerable()
            .ToListAsync();

        return result;
    }
}