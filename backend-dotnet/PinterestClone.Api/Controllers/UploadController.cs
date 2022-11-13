using System.Net.Http.Headers;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using PinterestClone.Infrastructure.Utils.Cloudinary;

namespace PinterestClone.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly IUploadService _uploadService;
    public UploadController(IUploadService uploadService)
    {
        _uploadService = uploadService;
    }

    [HttpPost("image")]
    public async Task<IActionResult> UploadImage()
    {
        var files = HttpContext.Request.Form.Files;
        if(!files.Any()) return BadRequest(new { Message = "No image to be upload!" });

        var result = await _uploadService.Image(files[0]);
        return Ok(new { Image = result.SecureUrl.AbsoluteUri });
        
    }
}