using PinterestClone.Application.Services.SavePinServices;
using PinterestClone.Infrastructure.Data.Models;

namespace PinterestClone.Application.Services.PinServices;

public interface IPinService 
{
    Task<Pin> Create(Pin pin);
    Task<Pin> FindById(string id);
    Task<List<Pin>> GetUserPins(string userId);
    Task<bool> Delete(string id);
    Task<(bool, string)> Update(string id, Pin updatedPin);
    Task<List<PinLookedUp>> GetAll();

}

public class PinLookedUp : Pin
{
    public UserProps User { get; set; } = null!;
}