using SparkySparky.Models;
using MongoDB.Driver;

namespace SparkySparky.Services;

public sealed class PolygonService : IPolygonService
{
    private readonly IMongoCollection<PolygonEntity> collection;

    public PolygonService(IConfiguration configuration)
    {
        var connectionString = configuration["MongoDb:ConnectionString"]
            ?? throw new InvalidOperationException(
                "MongoDb:ConnectionString is missing.");

        var databaseName = configuration["MongoDb:DatabaseName"]
            ?? throw new InvalidOperationException(
                "MongoDb:DatabaseName is missing.");

        var collectionName = configuration["MongoDb:PolygonsCollectionName"]
            ?? "polygons";

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        collection = database.GetCollection<PolygonEntity>(collectionName);

        collection.Indexes.CreateOne(
            new CreateIndexModel<PolygonEntity>(
                Builders<PolygonEntity>.IndexKeys.Geo2DSphere(
                    polygon => polygon.Geometry)));
    }

    public async Task<IReadOnlyList<PolygonEntity>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await collection
            .Find(FilterDefinition<PolygonEntity>.Empty)
            .SortByDescending(polygon => polygon.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<PolygonEntity> CreateAsync(
        PolygonEntity polygon,
        CancellationToken cancellationToken)
    {
        await collection.InsertOneAsync(
            polygon,
            cancellationToken: cancellationToken);

        return polygon;
    }

    public async Task<bool> UpdateAsync(
        PolygonEntity polygon,
        CancellationToken cancellationToken)
    {
        if (!MongoDB.Bson.ObjectId.TryParse(polygon.Id, out _))
        {
            return false;
        }

        var updates = Builders<PolygonEntity>.Update
            .Set(existingPolygon => existingPolygon.Name, polygon.Name)
            .Set(existingPolygon => existingPolygon.Geometry, polygon.Geometry);
        var result = await collection.UpdateOneAsync(
            existingPolygon => existingPolygon.Id == polygon.Id,
            updates,
            cancellationToken: cancellationToken);

        return result.MatchedCount > 0;
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
            polygon => polygon.Id == id,
            cancellationToken);

        return result.DeletedCount > 0;
    }
}