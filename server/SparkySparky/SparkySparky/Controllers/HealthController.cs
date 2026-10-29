using Microsoft.AspNetCore.Mvc;
using SparkySparky.Services;

namespace SparkySparky.Controllers;

[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    private readonly IMongoHealthChecker mongoHealthChecker;

    public HealthController(IMongoHealthChecker mongoHealthChecker)
    {
        this.mongoHealthChecker = mongoHealthChecker;
    }

    [HttpGet("is-alive")]
    public IActionResult IsAlive()
    {
        return Ok(new { status = "alive" });
    }

    [HttpGet]
    public async Task<IActionResult> GetHealth(CancellationToken cancellationToken)
    {
        if (!await mongoHealthChecker.IsHealthyAsync(cancellationToken))
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new { status = "unhealthy" });
        }

        return Ok(new { status = "healthy" });
    }
}
