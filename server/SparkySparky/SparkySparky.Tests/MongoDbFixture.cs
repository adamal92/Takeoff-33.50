using Microsoft.Extensions.Configuration;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SparkySparky.Tests;

public sealed class MongoDbFixture : IDisposable
{
    private readonly MongoDbRunner runner = MongoDbRunner.Start();

    public string ConnectionString => runner.ConnectionString;

    public string CreateDatabaseName() => $"test_{ObjectId.GenerateNewId()}";

    public IConfiguration CreateConfiguration(string databaseName)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["MongoDb:ConnectionString"] = ConnectionString,
                ["MongoDb:DatabaseName"] = databaseName
            })
            .Build();
    }

    public void DropDatabase(string databaseName)
    {
        new MongoClient(ConnectionString).DropDatabase(databaseName);
    }

    public void Dispose()
    {
        runner.Dispose();
    }
}
