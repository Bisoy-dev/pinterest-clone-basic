using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace PinterestClone.Infrastructure.Utils.Cloudinary;


public class UploadService : IUploadService
{
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;
    public UploadService(CloudinaryDotNet.Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }
    public async Task<ImageUploadResult> Image(IFormFile file)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            UniqueFilename = false,
            UseFilename = true,
            Overwrite = true
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return result;
    }
}