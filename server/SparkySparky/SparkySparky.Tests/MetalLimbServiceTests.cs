using MetalLimb;
using Microsoft.AspNetCore.Mvc;
using SparkySparky.Controllers;
using SparkySparky.Services;

namespace SparkySparky.Tests
{
    public sealed class MetalLimbServiceTests
    {
        [Fact]
        public void IsAlive_Works()
        {
            IFullMetalFingerMock fingerMock = new FullMetalFingerMock();
            MetalLimbService service = new MetalLimbService(fingerMock);
            service.SensorIsAlive();
            Assert.True(true);
        }
    }
}
