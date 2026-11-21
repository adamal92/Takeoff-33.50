using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparkySparky.Controllers;
using SparkySparky.Services;

namespace SparkySparky.Tests
{
    public sealed class MetalLimbControllerTests
    {
        [Fact]
        public void IsAlive_ReturnsOkWithAliveStatus()
        {
            MetalLimbController controller = new(new StubMetalLimbService());

            var result = controller.Get();

            var response = Assert.IsType<string[]> (result);
            Assert.Equal("Yes", response[1]);
        }
    }

    internal sealed class StubMetalLimbService : IMetalLimbService
    {
        public StubMetalLimbService()
        { }

        public void SensorIsAlive()
        { }
    }
}
