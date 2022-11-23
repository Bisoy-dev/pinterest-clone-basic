using MongoDB.Driver;

namespace PinterestClone.Infrastructure.Data;

public class MongoData : IMongoData
{
    public IMongoDatabase Database { get; }

    public IMongoClient Client { get; }

    public MongoData(IMongoDatabase db)
    {
        Database = db;
        Client = db.Client;
    }
}