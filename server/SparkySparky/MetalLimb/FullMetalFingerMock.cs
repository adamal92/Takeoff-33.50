namespace MetalLimb
{
    public sealed class FullMetalFingerMock : IFullMetalFingerMock
    {
        public void SensorIsAlive()
        {
            Console.WriteLine("Sensor is alive and functioning properly.");
        }
    }
}
