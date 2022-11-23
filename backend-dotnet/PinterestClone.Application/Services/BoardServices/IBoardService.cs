using PinterestClone.Infrastructure.Data.Models;

namespace PinterestClone.Application.Services.BoardServices;

public interface IBoardService
{
    Task<Board> Create(Board board);
    Task<List<Board>> FindByUserId(string userId);
    Task<Board> Update(string id, Board board);
    Task<Board> FindById(string id);
}