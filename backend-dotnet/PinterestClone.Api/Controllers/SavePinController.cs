using Microsoft.AspNetCore.Mvc;
using PinterestClone.Application.Services.SavePinServices;
using PinterestClone.Application.Services.UserService;
using PinterestClone.Contracts.SavePin;

namespace PinterestClone.Api.Controllers;

[ApiController]
[Route("api/save")]
public class SavePinController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ISavePinService _savePinService;

    public SavePinController(
        IUserService userService,
        ISavePinService savePinService
    )
    {
        _userService = userService;
        _savePinService = savePinService;
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] Save savePin) 
    {
        var user = await _userService.FindById(savePin.UserId);
        if(user is null) return BadRequest(new { Message = "User not found." } );

        try
        {
            var result = await _savePinService.Create(new InsertSavePin(
                    user.UserId,
                    savePin.PinId,
                    savePin.BoardId
                    ));
        }
        catch (Exception ex)
        {
            
            return BadRequest(new { ex.Message });
        }
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var results = await _savePinService.GetAll();
            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserSavePins([FromRoute]string userId)
    {
        var savePins = await _savePinService.FinByUserId(userId);
        return Ok(savePins);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute]string id)
    {
        var savePin = await _savePinService.FindById(id);
        return Ok(savePin);
    }
}