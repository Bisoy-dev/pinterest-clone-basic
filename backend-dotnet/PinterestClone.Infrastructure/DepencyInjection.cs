using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using PinterestClone.Infrastructure.Data;
using PinterestClone.Infrastructure.Utils.Cloudinary;
using PinterestClone.Infrastructure.Utils.Encryptors;
using PinterestClone.Infrastructure.Utils.Jwt;

namespace PinterestClone.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddMongoDb(configuration);
        services.AddAuth(configuration);   
        services.AddCloudinary(configuration);

        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<IEncryptor, PasswordEncryptor>();
        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt => 
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                    };
                });
        return services;
    }

    public static IServiceCollection AddMongoDb(this IServiceCollection services, 
        IConfiguration configuration)
    {

        services.AddSingleton(
            new MongoClient(configuration.GetConnectionString("MongoDBConnection"))
            .GetDatabase(configuration.GetConnectionString("MongoDBName")));

        services.AddSingleton<IMongoData, MongoData>();

        return services;
    }
}