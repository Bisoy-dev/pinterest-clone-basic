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

    public async Task<List<ImageUploadResult>> Images(IFormFileCollection files)
    {
        var result = new List<ImageUploadResult>();

        foreach(var file in files)
        {
            var uploadedResult = await Image(file);
            result.Add(uploadedResult);
        }

        return result;
    }
}