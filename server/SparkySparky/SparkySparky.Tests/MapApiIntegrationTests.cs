using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using SparkySparky.Services;

namespace SparkySparky.Tests;

public sealed class MapApiIntegrationTests : IClassFixture<MongoDbFixture>
{
    private readonly MongoDbFixture mongo;

    public MapApiIntegrationTests(MongoDbFixture mongo)
    {
        this.mongo = mongo;
    }

    [Fact]
    public async Task MapApi_RoundTripsPolygonsAndObjectsThroughMongo()
    {
        var databaseName = mongo.CreateDatabaseName();
        using var factory = CreateFactory(databaseName);
        using var client = factory.CreateClient();

        try
        {
            var registeredMongoClient = factory.Services.GetRequiredService<IMongoClient>();
            Assert.Same(
                registeredMongoClient,
                factory.Services.GetRequiredService<IMongoClient>());

            var health = await client.GetAsync("/api/health");
            Assert.True(
                health.StatusCode == HttpStatusCode.OK,
                $"Mongo health check returned {(int)health.StatusCode}: {await health.Content.ReadAsStringAsync()}");

            using var preflight = new HttpRequestMessage(HttpMethod.Options, "/api/objects");
            preflight.Headers.Add("Origin", "http://localhost:5173");
            preflight.Headers.Add("Access-Control-Request-Method", "POST");
            using var preflightResponse = await client.SendAsync(preflight);
            Assert.Equal(HttpStatusCode.NoContent, preflightResponse.StatusCode);
            Assert.Equal(
                "http://localhost:5173",
                Assert.Single(preflightResponse.Headers.GetValues("Access-Control-Allow-Origin")));

            var invalidPolygon = await client.PostAsJsonAsync(
                "/api/polygons",
                new
                {
                    name = "Invalid",
                    geometry = new
                    {
                        type = "Polygon",
                        coordinates = new[] { new[] { new[] { 1, 2 }, new[] { 3, 4 } } }
                    }
                });
            Assert.Equal(HttpStatusCode.BadRequest, invalidPolygon.StatusCode);

            var invalidObject = await client.PostAsJsonAsync(
                "/api/objects",
                new
                {
                    name = "Invalid",
                    symbolType = "marker",
                    geometry = new { type = "Point", coordinates = new[] { 1 } }
                });
            Assert.Equal(HttpStatusCode.BadRequest, invalidObject.StatusCode);

            var createdPolygonResponse = await client.PostAsJsonAsync(
                "/api/polygons",
                new
                {
                    name = "Test area",
                    geometry = new
                    {
                        type = "Polygon",
                        coordinates = new[]
                        {
                            new[]
                            {
                                new[] { 34.0, 32.0 },
                                new[] { 35.0, 32.0 },
                                new[] { 35.0, 33.0 },
                                new[] { 34.0, 32.0 }
                            }
                        }
                    }
                });
            Assert.Equal(HttpStatusCode.Created, createdPolygonResponse.StatusCode);
            using var createdPolygon = await JsonDocument.ParseAsync(
                await createdPolygonResponse.Content.ReadAsStreamAsync());
            var polygonId = AssertFeature(createdPolygon.RootElement, "Test area", "Polygon");
            AssertCoordinates(
                createdPolygon.RootElement.GetProperty("geometry").GetProperty("coordinates"),
                "[[[34,32],[35,32],[35,33],[34,32]]]");

            var createdObjectResponse = await client.PostAsJsonAsync(
                "/api/objects",
                new
                {
                    name = "Test marker",
                    symbolType = "jeep",
                    geometry = new
                    {
                        type = "Point",
                        coordinates = new[] { 34.7818, 32.0853 }
                    }
                });
            Assert.Equal(HttpStatusCode.Created, createdObjectResponse.StatusCode);
            using var createdObject = await JsonDocument.ParseAsync(
                await createdObjectResponse.Content.ReadAsStreamAsync());
            var objectId = AssertFeature(createdObject.RootElement, "Test marker", "Point");
            Assert.Equal(
                "jeep",
                createdObject.RootElement.GetProperty("symbolType").GetString());
            AssertCoordinates(
                createdObject.RootElement.GetProperty("geometry").GetProperty("coordinates"),
                "[34.7818,32.0853]");

            var database = registeredMongoClient.GetDatabase(databaseName);
            var storedPolygon = await database
                .GetCollection<BsonDocument>("polygons")
                .Find(Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(polygonId)))
                .SingleAsync();
            Assert.Equal("Test area", storedPolygon["Name"].AsString);
            Assert.Equal("Polygon", storedPolygon["Geometry"]["type"].AsString);

            var storedObject = await database
                .GetCollection<BsonDocument>("objects")
                .Find(Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(objectId)))
                .SingleAsync();
            Assert.Equal("Test marker", storedObject["Name"].AsString);
            Assert.Equal("jeep", storedObject["SymbolType"].AsString);
            Assert.Equal("Point", storedObject["Geometry"]["type"].AsString);

            using var polygons = await GetJson(client, "/api/polygons");
            var polygonFromList = Assert.Single(polygons.RootElement.EnumerateArray().ToArray());
            Assert.Equal(polygonId, polygonFromList.GetProperty("id").GetString());
            Assert.Equal("Test area", polygonFromList.GetProperty("name").GetString());
            AssertCoordinates(
                polygonFromList.GetProperty("geometry").GetProperty("coordinates"),
                "[[[34,32],[35,32],[35,33],[34,32]]]");

            using var objects = await GetJson(client, "/api/objects");
            var objectFromList = Assert.Single(objects.RootElement.EnumerateArray().ToArray());
            Assert.Equal(objectId, objectFromList.GetProperty("id").GetString());
            Assert.Equal("Test marker", objectFromList.GetProperty("name").GetString());
            Assert.Equal("jeep", objectFromList.GetProperty("symbolType").GetString());
            AssertCoordinates(
                objectFromList.GetProperty("geometry").GetProperty("coordinates"),
                "[34.7818,32.0853]");

            var updatedPolygon = await client.PutAsJsonAsync(
                $"/api/polygons/{polygonId}",
                new
                {
                    name = "Updated area",
                    geometry = new
                    {
                        type = "Polygon",
                        coordinates = new[]
                        {
                            new[]
                            {
                                new[] { 36.0, 32.0 },
                                new[] { 37.0, 32.0 },
                                new[] { 37.0, 33.0 },
                                new[] { 36.0, 32.0 }
                            }
                        }
                    }
                });
            Assert.Equal(HttpStatusCode.NoContent, updatedPolygon.StatusCode);

            var updatedObject = await client.PutAsJsonAsync(
                $"/api/objects/{objectId}",
                new
                {
                    name = "Updated marker",
                    symbolType = "marker",
                    geometry = new { type = "Point", coordinates = new[] { 35.0, 33.0 } }
                });
            Assert.Equal(HttpStatusCode.NoContent, updatedObject.StatusCode);

            using var updatedPolygons = await GetJson(client, "/api/polygons");
            var updatedPolygonFromList = Assert.Single(
                updatedPolygons.RootElement.EnumerateArray().ToArray());
            Assert.Equal("Updated area", updatedPolygonFromList.GetProperty("name").GetString());
            AssertCoordinates(
                updatedPolygonFromList.GetProperty("geometry").GetProperty("coordinates"),
                "[[[36,32],[37,32],[37,33],[36,32]]]");
            using var updatedObjects = await GetJson(client, "/api/objects");
            var updatedObjectFromList = Assert.Single(
                updatedObjects.RootElement.EnumerateArray().ToArray());
            Assert.Equal("Updated marker", updatedObjectFromList.GetProperty("name").GetString());
            Assert.Equal("marker", updatedObjectFromList.GetProperty("symbolType").GetString());
            AssertCoordinates(
                updatedObjectFromList.GetProperty("geometry").GetProperty("coordinates"),
                "[35,33]");

            var storedUpdatedPolygon = await database
                .GetCollection<BsonDocument>("polygons")
                .Find(Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(polygonId)))
                .SingleAsync();
            Assert.Equal("Updated area", storedUpdatedPolygon["Name"].AsString);
            var storedUpdatedObject = await database
                .GetCollection<BsonDocument>("objects")
                .Find(Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(objectId)))
                .SingleAsync();
            Assert.Equal("Updated marker", storedUpdatedObject["Name"].AsString);
            Assert.Equal("marker", storedUpdatedObject["SymbolType"].AsString);

            Assert.Equal(
                HttpStatusCode.NoContent,
                (await client.DeleteAsync($"/api/polygons/{polygonId}")).StatusCode);
            Assert.Equal(
                HttpStatusCode.NoContent,
                (await client.DeleteAsync($"/api/objects/{objectId}")).StatusCode);

            using var remainingPolygons = await GetJson(client, "/api/polygons");
            Assert.Empty(remainingPolygons.RootElement.EnumerateArray());
            using var remainingObjects = await GetJson(client, "/api/objects");
            Assert.Empty(remainingObjects.RootElement.EnumerateArray());

            Assert.Equal(
                0L,
                await database.GetCollection<BsonDocument>("polygons")
                    .CountDocumentsAsync(FilterDefinition<BsonDocument>.Empty));
            Assert.Equal(
                0L,
                await database.GetCollection<BsonDocument>("objects")
                    .CountDocumentsAsync(FilterDefinition<BsonDocument>.Empty));
        }
        finally
        {
            mongo.DropDatabase(databaseName);
        }
    }

    private WebApplicationFactory<Program> CreateFactory(string databaseName)
    {
        return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IMongoClient>();
                services.AddSingleton<IMongoClient>(
                    new MongoClient(mongo.ConnectionString));

                services.RemoveAll<IConfiguration>();
                services.AddSingleton<IConfiguration>(
                    new ConfigurationBuilder()
                        .AddInMemoryCollection(new Dictionary<string, string?>
                        {
                            ["MongoDb:ConnectionString"] = mongo.ConnectionString,
                            ["MongoDb:DatabaseName"] = databaseName
                        })
                        .Build());

                services.RemoveAll<IPolygonService>();
                services.RemoveAll<IMapObjectService>();
                services.AddSingleton<IPolygonService, PolygonService>();
                services.AddSingleton<IMapObjectService, MapObjectService>();
            });
        });
    }

    private static async Task<JsonDocument> GetJson(HttpClient client, string path)
    {
        var response = await client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        return await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
    }

    private static string AssertFeature(
        JsonElement feature,
        string expectedName,
        string expectedGeometryType)
    {
        var id = feature.GetProperty("id").GetString();
        Assert.False(string.IsNullOrWhiteSpace(id));
        Assert.Equal(expectedName, feature.GetProperty("name").GetString());
        Assert.Equal(
            expectedGeometryType,
            feature.GetProperty("geometry").GetProperty("type").GetString());
        Assert.True(feature.TryGetProperty("createdAt", out _));
        return id!;
    }

    private static void AssertCoordinates(JsonElement actual, string expectedJson)
    {
        using var expected = JsonDocument.Parse(expectedJson);
        AssertJsonEqual(expected.RootElement, actual);
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
