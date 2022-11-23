using PinterestClone.Infrastructure.Data.Models;

namespace PinterestClone.Application.Services.PinServices;

public interface IPinManagerService
{
    Task<Pin> Upload(Pin pin);
    Task<(bool, string)> ToggleLike(string pinId, string userId);
}