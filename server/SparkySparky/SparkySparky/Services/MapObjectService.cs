using SparkySparky.Models;
using MongoDB.Driver;

namespace SparkySparky.Services;

public sealed class MapObjectService : IMapObjectService
{
    private readonly IMongoCollection<MapObjectEntity> collection;

    public MapObjectService(IConfiguration configuration)
    {
        var connectionString = configuration["MongoDb:ConnectionString"]
            ?? throw new InvalidOperationException(
                "MongoDb:ConnectionString is missing.");

        var databaseName = configuration["MongoDb:DatabaseName"]
            ?? throw new InvalidOperationException(
                "MongoDb:DatabaseName is missing.");

        var collectionName = configuration["MongoDb:ObjectsCollectionName"]
            ?? "objects";

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        collection = database.GetCollection<MapObjectEntity>(collectionName);

        collection.Indexes.CreateOne(
            new CreateIndexModel<MapObjectEntity>(
                Builders<MapObjectEntity>.IndexKeys.Geo2DSphere(
                    mapObject => mapObject.Geometry)));
    }

    public async Task<IReadOnlyList<MapObjectEntity>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await collection
            .Find(FilterDefinition<MapObjectEntity>.Empty)
            .SortByDescending(mapObject => mapObject.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<MapObjectEntity> CreateAsync(
        MapObjectEntity mapObject,
        CancellationToken cancellationToken)
    {
        await collection.InsertOneAsync(
            mapObject,
            cancellationToken: cancellationToken);

        return mapObject;
    }

    public async Task<bool> DeleteAsync(
        string id,
        CancellationToken cancellationToken)
    {
        if (!MongoDB.Bson.ObjectId.TryParse(id, out _))
        {
            return false;
        }

        var result = await collection.DeleteOneAsync(
            mapObject => mapObject.Id == id,
            cancellationToken);

        return result.DeletedCount > 0;
    }
}