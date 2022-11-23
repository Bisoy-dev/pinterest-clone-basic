using PinterestClone.Infrastructure.Data.Models;

namespace PinterestClone.Application.Services.PinServices;

public class PinManagerService : IPinManagerService
{
    private readonly IPinService _pinService;

    public PinManagerService(IPinService pinService)
    {
        _pinService = pinService;
    }
    public async Task<(bool, string)> ToggleLike(string pinId, string userId)
    {
        var pin = await _pinService.FindById(pinId);
        if(pin is null) return (false, "Pin not found.");

        bool like = pin.Likes.Add(userId);

        if(!like)
        {
            pin.Likes.Remove(userId);
        }

        return await _pinService.Update(pin.PinId, pin);
    }

    public Task<Pin> Upload(Pin pin)
    {
        return _pinService.Create(pin);
    }
}