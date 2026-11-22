using MetalLimb;

namespace MetalLimb.Tests
{
    public class FullMetalFingerMockTests
    {
        protected readonly FullMetalFingerMock fingerMock;

        public FullMetalFingerMockTests()
        {
            this.fingerMock = new FullMetalFingerMock();
        }

        [Fact]
        public void IsAlive_Runs()
        {
            fingerMock.SensorIsAlive();
            Assert.True(true);
        }

        [Fact]
        public void IsSensorAlive_ReturnsTrue()
        {
            bool isAlive = fingerMock.IsSensorAlive();
            Assert.True(isAlive);
        }

        [Fact]
        public void ExitSleepMode_CallsSensorControl()
        {
            fingerMock.ExitSleepMode();
            // Add assertions to verify the sensor control method was called
        }

        [Fact]
        public void ToggleSleepMode_CallsSensorControl()
        {
            fingerMock.ToggleSleepMode(true);
            // Add assertions to verify the sensor control method was called
        }

        [Fact]
        public void ReadTemperature_ReturnsFloat()
        {
            float temperature = fingerMock.ReadTemperature();
            Assert.IsType<float>(temperature);
        }

        [Fact]
        public void ReadTemperature_ReturnsInExpectedRange()
        {
            float actualTemperature = fingerMock.ReadTemperature();
            Assert.True(actualTemperature >= -20 && actualTemperature <= 50);
        }

        public bool IsValidCoord(XYZvector coord) => coord != null && 
            (
                coord.X >= -180 && coord.X <= 180
                && coord.Y >= -90 && coord.Y <= 90
                && coord.Z >= -1000 && coord.Z <= 10000
            );

        [Fact]
        public void GeoLocationIsValidGeoVector()
        {
            
            XYZvector geoVector = fingerMock.ReadGeoLocationCoord();
            Assert.True(IsValidCoord(geoVector));
        }

        [Fact]
        public void ReadGeoLocationCoord_ReturnsXYZvector()
        {
            XYZvector geoVector = fingerMock.ReadGeoLocationCoord();
            Assert.IsType<XYZvector>(geoVector);
        }

        [Fact]
        public void ReadAcceleration_ReturnsXYZvector()
        {
            XYZvector accelVector = fingerMock.ReadAccelerationV();
            Assert.IsType<XYZvector>(accelVector);
        }

        [Fact]
        public void ReadAcceleration_ReturnsValidXYZvector()
        {
            XYZvector accelVector = fingerMock.ReadAccelerationV();
            Assert.True(IsValidCoord(accelVector));
        }
    }
}