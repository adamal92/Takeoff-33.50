using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparkySparky.Controllers;
using SparkySparky.Services;

namespace SparkySparky.Tests;

public sealed class HealthControllerTests
{
    [Fact]
    public void IsAlive_ReturnsOkWithAliveStatus()
    {
        var controller = new HealthController(new StubMongoHealthChecker(true));

        var result = controller.IsAlive();

        var response = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("alive", GetStatus(response.Value));
    }

    [Fact]
    public async Task GetHealth_WhenMongoIsAvailable_ReturnsOkWithHealthyStatus()
    {
        var controller = new HealthController(new StubMongoHealthChecker(true));

        var result = await controller.GetHealth(CancellationToken.None);

        var response = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("healthy", GetStatus(response.Value));
    }

    [Fact]
    public async Task GetHealth_WhenMongoIsUnavailable_ReturnsServiceUnavailable()
    {
        var controller = new HealthController(new StubMongoHealthChecker(false));

        var result = await controller.GetHealth(CancellationToken.None);

        var response = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status503ServiceUnavailable, response.StatusCode);
        Assert.Equal("unhealthy", GetStatus(response.Value));
    }

    private static string? GetStatus(object? value)
    {
        return value?.GetType().GetProperty("status")?.GetValue(value) as string;
    }

    private sealed class StubMongoHealthChecker : IMongoHealthChecker
    {
        private readonly bool isHealthy;

        public StubMongoHealthChecker(bool isHealthy)
        {
            this.isHealthy = isHealthy;
        }

        public Task<bool> IsHealthyAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(isHealthy);
        }
    }
}
