using MongoDB.Bson;
using MongoDB.Driver;
using PinterestClone.Infrastructure.Data.Models;

namespace PinterestClone.Application.Services.SavePinServices;

public interface ISavePinService
{
    Task<SavePin> Create(InsertSavePin savePinToInsert);
    Task<List<SavePinLookedUp>> GetAll();
    Task<List<SavePinLookedUp>> FinByUserId(string userId);
    Task<SavePinLookedUp?> FindById(string id);
}

public record InsertSavePin(
    string UserId, 
    string PinId, 
    string? BoardId);

public class SavePinLookedUp : SavePin
{
    public Board Board { get; set; } = null!;
    public Pin Pin { get; set; } = null!;
    public UserPin UserPin { get; set; } = null!;
    public UserProps User { get; set; } = null!;

    

}

public class UserProps
{
    public string UserId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<string> Images { get; set; } = new();
}


