using MongoDB.Driver;
using PinterestClone.Application.Services.BoardServices;
using PinterestClone.Application.Services.PinServices;
using PinterestClone.Application.Services.UserPinServices;
using PinterestClone.Infrastructure.Data;
using PinterestClone.Infrastructure.Data.Models;


namespace PinterestClone.Application.Services.SavePinServices;

public class SavePinService : ISavePinService
{
    private readonly IPinService _pinService;
    private readonly IMongoCollection<SavePin> _savePinCollection;
    private readonly IMongoData _mongoData;
    private readonly IUserPinService _userPinService;
    public const string SAVEPIN_COLLECTION = "save_pins";

    public SavePinService(
        IPinService pinService,
        IMongoData mongoData,
        IUserPinService userPinService)
    {
        _pinService = pinService;
        _mongoData = mongoData;
        _userPinService = userPinService;
        _savePinCollection = mongoData.Database.GetCollection<SavePin>(SAVEPIN_COLLECTION);
    }
    public async Task<SavePin> Create(InsertSavePin savePinToInsert)
    {
        var pin = await _pinService.FindById(savePinToInsert.PinId);
        if(pin is null)
        {
            throw new Exception("Pin not found.");
        }
        
        using var session = await _mongoData.Client.StartSessionAsync();

        session.StartTransaction();
        
        try
        {
            var userPinCollection = _mongoData.Database.GetCollection<UserPin>(UserPinService.USERPIN_COLLECTION);
            var savePinCollection = _mongoData.Database.GetCollection<SavePin>(SavePinService.SAVEPIN_COLLECTION);
            var boardCollection = _mongoData.Database.GetCollection<Board>(BoardService.BOARD_COLLECTION);

            UserPin userPin = await _userPinService.FindByUserId(savePinToInsert.UserId);

            if(userPin is null)
            {
                userPin = new UserPin { UserId = savePinToInsert.UserId };
                await userPinCollection.InsertOneAsync(session, userPin);
            }
            else
            {
                userPin.Date.DateModified = DateTime.UtcNow;
            }

            Board? board = await (await boardCollection
                    .FindAsync(b => b.BoardId == savePinToInsert.BoardId))
                    .FirstOrDefaultAsync();
            
            var newSavePin = new SavePin(
                    userPin.UserPinId, 
                    pin.PinId, 
                    board?.BoardId,
                    (pin.UserId == savePinToInsert.UserId ? "Owned" : "Save"));

            // await _savePinCollection.InsertOneAsync(newSavePin);
            await savePinCollection.InsertOneAsync(session, newSavePin);
            // await _userPinService.Update(userPin.UserPinId, userPin);
            await userPinCollection.ReplaceOneAsync(session, Builders<UserPin>
                .Filter.Eq(up => up.UserPinId, userPin.UserPinId),
                userPin);

            await session.CommitTransactionAsync();

            return newSavePin;
        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SavePinLookedUp>> GetAll()
    {
        var boardCollection = _mongoData.Database.GetCollection<Board>(BoardService.BOARD_COLLECTION);
        var pinCollection = _mongoData.Database.GetCollection<Pin>(PinService.PIN_COLLECTION);
        var userPinCollection = _mongoData.Database.GetCollection<UserPin>(UserPinService.USERPIN_COLLECTION);
        
        var results = await (from spl in (_savePinCollection.AsQueryable()
            .Join(pinCollection.AsQueryable(), s => s.PinId, 
            p => p.PinId, (s, p) => new SavePinLookedUp
            {
                SavePinId = s.SavePinId,
                UserPinId = s.UserPinId,
                PinId = s.PinId,
                BoardId = s.BoardId,
                Date = s.Date,
                Type = s.Type,
                Pin = p
            })
            .AsQueryable()
            .Join(userPinCollection.AsQueryable(), spl => spl.UserPinId, 
            up => up.UserPinId,
            (spl, up) => new SavePinLookedUp
            {
                SavePinId = spl.SavePinId,
                UserPinId = spl.UserPinId,
                PinId = spl.PinId,
                BoardId = spl.BoardId,
                Date = spl.Date,
                Type = spl.Type,
                Pin = spl.Pin,
                UserPin = up
            }).AsQueryable()) 
            join b in boardCollection.AsQueryable()
            on spl.BoardId equals b.BoardId into boardGroup
            from board in boardGroup.DefaultIfEmpty()
            select new SavePinLookedUp
            {
                SavePinId = spl.SavePinId,
                UserPinId = spl.UserPinId,
                PinId = spl.PinId,
                BoardId = spl.BoardId,
                Date = spl.Date,
                Type = spl.Type,
                Pin = spl.Pin,
                UserPin = spl.UserPin,
                Board = board
            })
            .ToAsyncEnumerable()
            .ToListAsync();
              
        return results;
        
    }

    public async Task<List<SavePinLookedUp>> FinByUserId(string userId)
    {
        var users = _mongoData.Database.GetCollection<User>(UserService.UserService.USER_COLLECTION);
        var pins = _mongoData.Database.GetCollection<Pin>(PinService.PIN_COLLECTION);
        var userPins = _mongoData.Database.GetCollection<UserPin>(UserPinService.USERPIN_COLLECTION);
        var boards = _mongoData.Database.GetCollection<Board>(BoardService.BOARD_COLLECTION);

        var results = await (from spl in (_savePinCollection.AsQueryable().Join(pins.AsQueryable(), sp => sp.PinId, 
        p => p.PinId, (sp, p) => new SavePinLookedUp 
            {
                SavePinId = sp.SavePinId,
                UserPinId = sp.UserPinId,
                PinId = sp.PinId,
                BoardId = sp.BoardId,
                Date = sp.Date,
                Type = sp.Type,
                Pin = p
            }).AsQueryable()
            .Join(userPins.AsQueryable(), spl => spl.UserPinId, 
            up => up.UserPinId, (spl, up) => new SavePinLookedUp
            {
                SavePinId = spl.SavePinId,
                UserPinId = spl.UserPinId,
                PinId = spl.PinId,
                BoardId = spl.BoardId,
                Date = spl.Date,
                Type = spl.Type,
                Pin = spl.Pin,
                UserPin = up
            }).AsQueryable()
            .Join(users.AsQueryable(), spl => spl.UserPin.UserId, 
            u => u.UserId, (spl, u) => new SavePinLookedUp
            {
                SavePinId = spl.SavePinId,
                UserPinId = spl.UserPinId,
                PinId = spl.PinId,
                BoardId = spl.BoardId,
                Date = spl.Date,
                Type = spl.Type,
                Pin = spl.Pin,
                UserPin = spl.UserPin,
                User = new UserProps { UserId = u.UserId, Email = u.Email, Images = u.Images }
            })) join b in boards.AsQueryable() on spl.BoardId equals b.BoardId
            into boardGroups from board in boardGroups.DefaultIfEmpty()
            select new SavePinLookedUp 
            {
                SavePinId = spl.SavePinId,
                UserPinId = spl.UserPinId,
                PinId = spl.PinId,
                BoardId = spl.BoardId,
                Date = spl.Date,
                Type = spl.Type,
                Pin = spl.Pin,
                UserPin = spl.UserPin,
                User = spl.User,
                Board = board
            })
            .AsQueryable()
            .Where(spl => spl.User.UserId == userId)
            .ToAsyncEnumerable()
            .ToListAsync();
        
        return results;
    }

    public async Task<SavePinLookedUp?> FindById(string id)
    {
        var users = _mongoData.Database.GetCollection<User>(UserService.UserService.USER_COLLECTION);
        var pins = _mongoData.Database.GetCollection<Pin>(PinService.PIN_COLLECTION);
        var userPins = _mongoData.Database.GetCollection<UserPin>(UserPinService.USERPIN_COLLECTION);
        var boards = _mongoData.Database.GetCollection<Board>(BoardService.BOARD_COLLECTION);

        var result = await (from spl in (_savePinCollection.AsQueryable().Join(pins.AsQueryable(), sp => sp.PinId, 
        p => p.PinId, (sp, p) => new SavePinLookedUp 
            {
                SavePinId = sp.SavePinId,
                UserPinId = sp.UserPinId,
                PinId = sp.PinId,
                BoardId = sp.BoardId,
                Date = sp.Date,
                Type = sp.Type,
                Pin = p
            }).AsQueryable()
            .Join(userPins.AsQueryable(), spl => spl.UserPinId, 
            up => up.UserPinId, (spl, up) => new SavePinLookedUp
            {
                SavePinId = spl.SavePinId,
                UserPinId = spl.UserPinId,
                PinId = spl.PinId,
                BoardId = spl.BoardId,
                Date = spl.Date,
                Type = spl.Type,
                Pin = spl.Pin,
                UserPin = up
            }).AsQueryable()
            .Join(users.AsQueryable(), spl => spl.UserPin.UserId, 
            u => u.UserId, (spl, u) => new SavePinLookedUp
            {
                SavePinId = spl.SavePinId,
                UserPinId = spl.UserPinId,
                PinId = spl.PinId,
                BoardId = spl.BoardId,
                Date = spl.Date,
                Type = spl.Type,
                Pin = spl.Pin,
                UserPin = spl.UserPin,
                User = new UserProps { UserId = u.UserId, Email = u.Email, Images = u.Images }
            })) join b in boards.AsQueryable() on spl.BoardId equals b.BoardId
            into boardGroups from board in boardGroups.DefaultIfEmpty()
            select new SavePinLookedUp 
            {
                SavePinId = spl.SavePinId,
                UserPinId = spl.UserPinId,
                PinId = spl.PinId,
                BoardId = spl.BoardId,
                Date = spl.Date,
                Type = spl.Type,
                Pin = spl.Pin,
                UserPin = spl.UserPin,
                User = spl.User,
                Board = board
            })
            .AsQueryable()
            .Where(spl => spl.SavePinId == id)
            .ToAsyncEnumerable()
            .FirstOrDefaultAsync();

        return result;
    }
}