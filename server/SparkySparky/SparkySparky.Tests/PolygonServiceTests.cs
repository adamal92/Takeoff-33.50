using System.Text.Json;
using SparkySparky.Models;
using SparkySparky.Services;

namespace SparkySparky.Tests;

public sealed class PolygonServiceTests : IClassFixture<MongoDbFixture>
{
    private readonly MongoDbFixture mongo;

    public PolygonServiceTests(MongoDbFixture mongo)
    {
        this.mongo = mongo;
    }

    [Fact]
    public async Task PolygonCrud_PersistsAndReadsValidGeoJson()
    {
        var databaseName = mongo.CreateDatabaseName();
        var service = new PolygonService(mongo.CreateConfiguration(databaseName));
        var cancellationToken = CancellationToken.None;

        try
        {
            var polygon = new PolygonEntity
            {
                Name = "Initial area",
                Geometry = ParseGeometry(
                    """{"type":"Polygon","coordinates":[[[34,32],[35,32],[35,33],[34,32]]]}"""),
                CreatedAt = DateTime.UtcNow
            };

            var created = await service.CreateAsync(polygon, cancellationToken);

            Assert.False(string.IsNullOrWhiteSpace(created.Id));
            var stored = Assert.Single(await service.GetAllAsync(cancellationToken));
            Assert.Equal("Initial area", stored.Name);
            AssertGeometryEqual(polygon.Geometry, stored.Geometry);

            var createdAt = stored.CreatedAt;
            var updated = await service.UpdateAsync(
                new PolygonEntity
                {
                    Id = stored.Id,
                    Name = "Updated area",
                    Geometry = ParseGeometry(
                        """{"type":"Polygon","coordinates":[[[36,32],[37,32],[37,33],[36,32]]]}"""),
                    CreatedAt = DateTime.UtcNow.AddDays(1)
                },
                cancellationToken);

            Assert.True(updated);
            var updatedStored = Assert.Single(await service.GetAllAsync(cancellationToken));
            Assert.Equal("Updated area", updatedStored.Name);
            Assert.Equal(createdAt, updatedStored.CreatedAt);
            AssertGeometryEqual(
                ParseGeometry(
                    """{"type":"Polygon","coordinates":[[[36,32],[37,32],[37,33],[36,32]]]}"""),
                updatedStored.Geometry);

            Assert.False(
                await service.UpdateAsync(
                    new PolygonEntity
                    {
                        Id = "not-an-object-id",
                        Name = "Missing",
                        Geometry = polygon.Geometry,
                        CreatedAt = DateTime.UtcNow
                    },
                    cancellationToken));
            Assert.False(
                await service.UpdateAsync(
                    new PolygonEntity
                    {
                        Id = "000000000000000000000001",
                        Name = "Missing",
                        Geometry = polygon.Geometry,
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
