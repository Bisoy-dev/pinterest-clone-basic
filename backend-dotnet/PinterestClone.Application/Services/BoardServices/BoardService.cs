using MongoDB.Driver;
using PinterestClone.Infrastructure.Data;
using PinterestClone.Infrastructure.Data.Models;

namespace PinterestClone.Application.Services.BoardServices;

public class BoardService : IBoardService
{
    public const string BOARD_COLLECTION = "boards";
    private readonly IMongoCollection<Board> _boardCollection;
    public BoardService(IMongoData mongoData)
    {
        _boardCollection = mongoData.Database.GetCollection<Board>(BOARD_COLLECTION);
    }
    public async Task<Board> Create(Board board)
    {
        await _boardCollection.InsertOneAsync(board);
        return board;
    }

    public async Task<Board> FindById(string id)
    {
        return await (await _boardCollection
            .FindAsync(b => b.BoardId == id))
            .FirstOrDefaultAsync();
    }

    public async Task<List<Board>> FindByUserId(string userId)
    {
        return await (await _boardCollection
            .FindAsync(b => b.UserId == userId))
            .ToListAsync();
    }

    public async Task<Board> Update(string id, Board board)
    {
        var boardQuery = await FindById(id);
        if(boardQuery is null)
        {
            throw new Exception("Board not found!");
        }

        await _boardCollection.ReplaceOneAsync(
            Builders<Board>.Filter.Eq(b => b.BoardId, id),
            board
        );

        return board;
    }
}