using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PinterestClone.Application.Services.PinServices;
using PinterestClone.Application.Services.SavePinServices;
using PinterestClone.Application.Services.UserService;
using PinterestClone.Contracts.Pin;
using PinterestClone.Infrastructure.Data.Models;
using PinterestClone.Infrastructure.Utils.Cloudinary;

namespace PinterestClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PinController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUploadService _uploadService;
    private readonly IPinService _pinService;
    private readonly IPinManagerService _pinManagerService;
    private readonly ISavePinService _savePinService;

    public PinController(
        IUserService userService,
        IUploadService uploadService,
        IPinService pinService,
        IPinManagerService pinManagerService,
        ISavePinService savePinService)
    {
        _userService = userService;
        _uploadService = uploadService;
        _pinService = pinService;
        _pinManagerService = pinManagerService;
        _savePinService = savePinService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] UploadPinRequest uploadPin)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId != uploadPin.UserId) return BadRequest(new { Message = "Userid from body is not equal userid in the context." });

        var user = await _userService.FindById(uploadPin.UserId);

        if(user is null) return BadRequest(new { Message = "User not found." });

        var uploadResult = await _uploadService.Image(HttpContext.Request.Form.Files[0]);
        
        var newPin = new Pin(
            user.UserId, 
            uploadResult.SecureUrl.AbsoluteUri,
            uploadPin.Description,
             uploadPin.Title,
            uploadPin.DistinationLink);
        
        var result = await _pinManagerService.Upload(newPin);
        
        var savePinResult = await _savePinService.Create(new InsertSavePin(
                user.UserId,
                result.PinId,
                uploadPin.BoardId));

        var savePin = await _savePinService.FindById(savePinResult.SavePinId);

        return Ok(savePin);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPinById([FromRoute] string id)
    {
        var pin = await _pinService.FindById(id);
        return Ok(pin);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetPinsByUser([FromRoute] string userId)
    {
        var pins = await _pinService.GetUserPins(userId);
        return Ok(pins);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var result = await _pinService.Delete(id);
        return result ? Ok(new { Message = "Successfully deleted" })
             : BadRequest(new { Message = "Failed to delete" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] string id,
        [FromForm] UploadPinRequest uploadPin)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId != uploadPin.UserId) return BadRequest(new { Message = "Userid from body is not equal userid in the context." });

        var user = await _userService.FindById(uploadPin.UserId);

        if(user is null) return BadRequest(new { Message = "User not found." });

        var pin = await _pinService.FindById(id);

        if(pin is null) return BadRequest(new { Message = "Pin not found." });

        pin.Image = HttpContext.Request.Form.Files.Any() ?  
            (await _uploadService.Image(HttpContext.Request.Form.Files[0]))
                .SecureUrl.AbsoluteUri : 
            pin.Image;
        pin.Title = uploadPin.Title;
        pin.Description = uploadPin.Description;
        pin.DistinationLink = uploadPin.DistinationLink;

        (bool succeed, string message) = await _pinService.Update(id, pin);

        return succeed ? Ok(new { message }) : BadRequest(new { message });
    }

    [HttpPut("toggle-like")]
    public async Task<IActionResult> ToggleLike([FromBody] LikePinRequest likePin)
    {
        var user = await _userService.FindById(likePin.UserId);
        if(user is null) return BadRequest(new { Message = "User not found" });

        var pin = await _pinService.FindById(likePin.PinId);
        if(pin is null) return BadRequest(new { Message = "Pin not found" });

        (bool succeed, string message) = await _pinManagerService.ToggleLike(pin.PinId, user.UserId);

        return succeed ? Ok(new { message }) : BadRequest(new { message } );
    }

}