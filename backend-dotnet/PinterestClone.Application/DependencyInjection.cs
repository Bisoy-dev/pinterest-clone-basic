using Microsoft.Extensions.DependencyInjection;
using PinterestClone.Application.Services.Authentication;
using PinterestClone.Application.Services.BoardServices;
using PinterestClone.Application.Services.PinServices;
using PinterestClone.Application.Services.SavePinServices;
using PinterestClone.Application.Services.UserPinServices;
using PinterestClone.Application.Services.UserService;

namespace PinterestClone.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IUserManager, UserManager>();
        services.AddScoped<IPinService, PinService>();
        services.AddScoped<IPinManagerService, PinManagerService>();
        services.AddScoped<ISavePinService, SavePinService>();
        services.AddScoped<IUserPinService, UserPinService>();
        services.AddScoped<IBoardService, BoardService>();
        return services;
    }
}