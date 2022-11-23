using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PinterestClone.Application.Services.Authentication;
using PinterestClone.Application.Services.UserService;
using PinterestClone.Contracts.User;
using PinterestClone.Infrastructure.Utils.Cloudinary;
using PinterestClone.Infrastructure.Utils.Jwt;

namespace PinterestClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly IUserManager _userManager;
    private readonly IUserService _userService;
    private readonly IUploadService _uploadService;

    public UserController(
        IJwtService jwtService,
        IUserManager userManager,
        IUserService userService,
        IUploadService uploadService)
    {
        _jwtService = jwtService;
        _userManager = userManager;
        _userService = userService;
        _uploadService = uploadService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody]UserRequest userRequest )
    {
        var result = await _userManager.Create(userRequest.Email, userRequest.Password);
        if(!result.IsSucceed) return BadRequest(new { Message = result.Errors.FirstOrDefault() });

        var token = _jwtService.Generate(result.User.UserId, result.User.Email);

        return Ok(new UserResult(result.User.Email, token));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserRequest userRequest)
    {
        var result = await _userManager.SignIn(userRequest.Email, userRequest.Password);
        if(!result.IsSucceed) return BadRequest(new { Message = result.Errors.FirstOrDefault() });
        
        var user = await _userService.FindByEmail(userRequest.Email);

        var token = _jwtService.Generate(user.UserId, user.Email);

        return Ok(new UserResult(user.Email, token));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfile(
        [FromRoute]string id, 
        [FromForm]UserUpdateProfileRequest userUpdate)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if(userId != id) return BadRequest(new { Message = "UserId from route is not equal userId from the context." });

        var files = HttpContext.Request.Form.Files;

        var user = await _userService.FindById(id);

        if(user is null) return BadRequest(new { Message = "User not found." });

        user.Email = userUpdate.Email;
        user.Images = files.Any() ? (await _uploadService.Images(files))
            .Select(i => i.SecureUrl.AbsoluteUri).ToList() : user.Images;

        await _userService.Update(id, user);

        return Ok(new { Message = "Profile updated successfully." });
        
    }
}