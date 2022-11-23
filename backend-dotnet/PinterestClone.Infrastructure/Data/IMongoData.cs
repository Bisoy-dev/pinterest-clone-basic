using MongoDB.Driver;

namespace PinterestClone.Infrastructure.Data;

public interface IMongoData
{
    IMongoDatabase Database { get; }
    IMongoClient Client { get; }
}