using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SparkySparky.Controllers;

[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    private readonly IMongoClient mongoClient;

    public HealthController(IMongoClient mongoClient)
    {
        this.mongoClient = mongoClient;
    }

    [HttpGet("is-alive")]
    public IActionResult IsAlive()
    {
        return Ok(new { status = "alive" });
    }

    [HttpGet]
    public async Task<IActionResult> GetHealth(CancellationToken cancellationToken)
    {
        try
        {
            await mongoClient
                .GetDatabase("admin")
                .RunCommandAsync<BsonDocument>(
                    new BsonDocument("ping", 1),
                    cancellationToken: cancellationToken);
        }
        catch (MongoException)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new { status = "unhealthy" });
        }

        return Ok(new { status = "healthy" });
    }
}
