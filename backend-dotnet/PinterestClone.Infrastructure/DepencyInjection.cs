using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PinterestClone.Infrastructure.Data.UserIdentity;
using PinterestClone.Infrastructure.Utils.Cloudinary;
using PinterestClone.Infrastructure.Utils.Jwt;

namespace PinterestClone.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(opt => 
            opt.UseSqlServer(configuration.GetConnectionString("UserIdentityConnection")));
            
        services.AddCloudinary(configuration);

        services.AddSingleton<IJwtService, JwtService>();
        return services;
    }
}