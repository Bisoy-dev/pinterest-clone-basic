using Microsoft.AspNetCore.Mvc;
using PinterestClone.Contracts.User;
using PinterestClone.Infrastructure.Utils.Jwt;

namespace PinterestClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IJwtService _jwtService;
    public UserController(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody]UserRequest userRequest )
    {
        var token = _jwtService.Generate(Guid.NewGuid().ToString(), userRequest.Email);

        return Ok(new UserResult(userRequest.Email, token));
    }
}