using System.Text.Json;
using MongoDB.Driver;
using SparkySparky.Models;
using SparkySparky.Services;

namespace SparkySparky.Tests;

public sealed class MapObjectServiceTests : IClassFixture<MongoDbFixture>
{
    private readonly MongoDbFixture mongo;

    public MapObjectServiceTests(MongoDbFixture mongo)
    {
        this.mongo = mongo;
    }

    [Fact]
    public async Task MapObjectCrud_PersistsAndReadsValidGeoJson()
    {
        var databaseName = mongo.CreateDatabaseName();
        var service = new MapObjectService(
            new MongoClient(mongo.ConnectionString),
            mongo.CreateConfiguration(databaseName));
        var cancellationToken = CancellationToken.None;

        try
        {
            var mapObject = new MapObjectEntity
            {
                Name = "Initial marker",
                SymbolType = "marker",
                Geometry = ParseGeometry("""{"type":"Point","coordinates":[34.7818,32.0853]}"""),
                CreatedAt = DateTime.UtcNow
            };

            var created = await service.CreateAsync(mapObject, cancellationToken);

            Assert.False(string.IsNullOrWhiteSpace(created.Id));
            var stored = Assert.Single(await service.GetAllAsync(cancellationToken));
            Assert.Equal("Initial marker", stored.Name);
            Assert.Equal("marker", stored.SymbolType);
            AssertGeometryEqual(mapObject.Geometry, stored.Geometry);

            var createdAt = stored.CreatedAt;
            var updatedGeometry = ParseGeometry(
                """{"type":"Point","coordinates":[35.2,32.2]}""");
            var updated = await service.UpdateAsync(
                new MapObjectEntity
                {
                    Id = stored.Id,
                    Name = "Updated marker",
                    SymbolType = "jeep",
                    Geometry = updatedGeometry,
                    CreatedAt = DateTime.UtcNow.AddDays(1)
                },
                cancellationToken);

            Assert.True(updated);
            var updatedStored = Assert.Single(await service.GetAllAsync(cancellationToken));
            Assert.Equal("Updated marker", updatedStored.Name);
            Assert.Equal("jeep", updatedStored.SymbolType);
            Assert.Equal(createdAt, updatedStored.CreatedAt);
            AssertGeometryEqual(updatedGeometry, updatedStored.Geometry);

            Assert.False(
                await service.UpdateAsync(
                    new MapObjectEntity
                    {
                        Id = "not-an-object-id",
                        Name = "Missing",
                        SymbolType = "marker",
                        Geometry = mapObject.Geometry,
                        CreatedAt = DateTime.UtcNow
                    },
                    cancellationToken));
            Assert.False(
                await service.UpdateAsync(
                    new MapObjectEntity
                    {
                        Id = "000000000000000000000001",
                        Name = "Missing",
                        SymbolType = "marker",
                        Geometry = mapObject.Geometry,
                        CreatedAt = DateTime.UtcNow
                    },
                    cancellationToken));

            Assert.True(await service.DeleteAsync(stored.Id!, cancellationToken));
            Assert.Empty(await service.GetAllAsync(cancellationToken));
            Assert.False(await service.DeleteAsync(stored.Id!, cancellationToken));
            Assert.False(await service.DeleteAsync("not-an-object-id", cancellationToken));
        }
        finally
        {
            mongo.DropDatabase(databaseName);
        }
    }

    private static GeoJsonGeometry ParseGeometry(string json)
    {
        using var document = JsonDocument.Parse(json);
        return new GeoJsonGeometry
        {
            Type = document.RootElement.GetProperty("type").GetString()!,
            Coordinates = document.RootElement.GetProperty("coordinates").Clone()
        };
    }

    private static void AssertGeometryEqual(
        GeoJsonGeometry expected,
        GeoJsonGeometry actual)
    {
        Assert.Equal(expected.Type, actual.Type);
        AssertJsonEqual(expected.Coordinates, actual.Coordinates);
    }

    private static void AssertJsonEqual(JsonElement expected, JsonElement actual)
    {
        Assert.Equal(expected.ValueKind, actual.ValueKind);
        if (expected.ValueKind == JsonValueKind.Array)
        {
            var expectedItems = expected.EnumerateArray().ToArray();
            var actualItems = actual.EnumerateArray().ToArray();
            Assert.Equal(expectedItems.Length, actualItems.Length);
            for (var index = 0; index < expectedItems.Length; index++)
            {
                AssertJsonEqual(expectedItems[index], actualItems[index]);
            }

            return;
        }

        if (expected.ValueKind == JsonValueKind.Number)
        {
            Assert.Equal(expected.GetDouble(), actual.GetDouble());
            return;
        }

        Assert.Equal(expected.ToString(), actual.ToString());
    }
}
