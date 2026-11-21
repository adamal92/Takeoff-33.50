using MetalLimb;

namespace SparkySparky.Services
{
    public sealed class MetalLimbService : IMetalLimbService
    {
        private readonly IFullMetalFingerMock fingerMock;

        public MetalLimbService(IFullMetalFingerMock fingerMock)
        {
            this.fingerMock = fingerMock;
        }

        public void SensorIsAlive()
        {
            fingerMock.SensorIsAlive();
        }
    }
}
