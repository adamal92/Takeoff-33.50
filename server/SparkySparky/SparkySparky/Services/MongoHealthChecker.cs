using MongoDB.Bson;
using MongoDB.Driver;

namespace SparkySparky.Services;

public sealed class MongoHealthChecker : IMongoHealthChecker
{
    private readonly IMongoClient mongoClient;

    public MongoHealthChecker(IMongoClient mongoClient)
    {
        this.mongoClient = mongoClient;
    }

    public async Task<bool> IsHealthyAsync(CancellationToken cancellationToken)
    {
        try
        {
            await mongoClient
                .GetDatabase("admin")
                .RunCommandAsync<BsonDocument>(
                    new BsonDocument("ping", 1),
                    cancellationToken: cancellationToken);

            return true;
        }
        catch (MongoException)
        {
            return false;
        }
    }
}
