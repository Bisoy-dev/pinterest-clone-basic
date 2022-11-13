using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace PinterestClone.Infrastructure.Utils.Cloudinary;

public interface IUploadService
{
    Task<ImageUploadResult> Image(IFormFile file);
}