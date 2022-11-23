using Microsoft.AspNetCore.Mvc;
using PinterestClone.Application.Services.PinServices;

namespace PinterestClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedController : ControllerBase
{
    private readonly IPinService _pinService;

    public FeedController(IPinService pinService)
    {
        _pinService = pinService;
    }
    // temporary feed for users
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var feeds = await _pinService.GetAll();
        return Ok(feeds);
    }
}