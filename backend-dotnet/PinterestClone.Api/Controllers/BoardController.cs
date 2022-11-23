using Microsoft.AspNetCore.Mvc;
using PinterestClone.Application.Services.BoardServices;
using PinterestClone.Application.Services.UserService;
using PinterestClone.Contracts.Board;
using PinterestClone.Infrastructure.Data.Models;
using PinterestClone.Infrastructure.Utils.Cloudinary;

namespace  PinterestClone.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BoardController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IBoardService _boardService;
    public BoardController(
        IUserService userService,
        IBoardService boardService
    )
    {
        _userService = userService;
        _boardService = boardService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody]SaveBoard saveBoard)
    {
        var user = await _userService.FindById(saveBoard.UserId);
        if(user is null) return BadRequest(new { Message = "User not found." });

        var result = await _boardService.Create(new Board
            {
                UserId = user.UserId,
                Title = saveBoard.Title,
                Description = saveBoard.Description,
            });
        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserBoards([FromRoute]string userId)
    {
        var boards = await _boardService.FindByUserId(userId);
        return Ok(boards);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute]string id,
        [FromBody]SaveBoard saveBoard)
    {
        var user = await _userService.FindById(saveBoard.UserId);
        if(user is null) return BadRequest(new { Message = "User not found." });

        var board = await _boardService.FindById(id);
        if(board is null) return BadRequest(new { Message = "Board not found" });

        board.Title = saveBoard.Title;
        board.Description = saveBoard.Description;

        var result = await _boardService.Update(board.BoardId, board);

        return Ok(result);
    }
}