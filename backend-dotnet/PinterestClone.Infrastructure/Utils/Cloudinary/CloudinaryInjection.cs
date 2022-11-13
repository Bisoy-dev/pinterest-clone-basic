using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PinterestClone.Infrastructure.Utils.Cloudinary;

public static class CloudinaryInjection
{
    public static IServiceCollection AddCloudinary(this IServiceCollection service, 
        IConfiguration configuration)
    {
        service.AddSingleton<CloudinaryDotNet.Cloudinary>(sp => {
            
            var cloudinary = new CloudinaryDotNet.Cloudinary(new Account(
                configuration["CloudinarySettings:Name"],
                configuration["CloudinarySettings:Key"],
                configuration["CloudinarySettings:Secret"]
            ));
            cloudinary.Api.Secure = true;
            return cloudinary;    
        });

        service.AddSingleton<IUploadService, UploadService>();
        return service;
    }
}