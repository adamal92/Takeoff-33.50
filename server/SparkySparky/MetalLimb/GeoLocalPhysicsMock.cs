using MetalAbs.SharedDTOs;
using MetalAbs.SharedMocks;
using System.Numerics;

namespace MetalLimb
{
    public sealed class GeoLocalPhysicsMock : IGeoLocalPhysicsMock
    {
        private XYZvector ParseXYZvectorFromVector3(Vector3 vector)
        {
            return new XYZvector { X = vector.X, Y = vector.Y, Z = vector.Z };
        }

        /// <summary>
        /// Reads the Geo Location.
        /// </summary>
        /// <returns>A vector representing the current geographic location ATM.</returns>
        public XYZvector ReadGeoLocationCoord()
        {
            Vector3 mockVector = Vector3.Zero;
            return ParseXYZvectorFromVector3(mockVector);
        }

        /// <summary>
        /// Send target Geo Location to sensor for acquisition.
        /// </summary>
        /// <param name="targetCoord">The target geographic location coordinates to transmit.</param>
        public void TransmitTargetGeoLocationCoord(XYZvector targetCoord)
        {
            Console.WriteLine($"Transmitting target geographic location: {targetCoord}");
        }

        /// <summary>
        /// Reads the Acceleration vector.
        /// </summary>
        /// <returns>A vector representing the acceleration values.</returns>
        public XYZvector ReadAccelerationV()
        {
            Vector3 mockVector = Vector3.Zero;
            return ParseXYZvectorFromVector3(mockVector);
        }

        /// <summary>
        /// Reads the gyroscope vector.
        /// </summary>
        /// <returns>An <see cref="XYZvector"/> representing the gyroscope vector.</returns>
        public XYZvector ReadGyroscopeV()
        {
            Vector3 mockVector = Vector3.Zero;
            return ParseXYZvectorFromVector3(mockVector);
        }

        public StreamMock ReadGeoLocationStream()
        {
            return new StreamMock(new byte[0]);
        }
    }
}
