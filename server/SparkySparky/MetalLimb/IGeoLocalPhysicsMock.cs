using System.Numerics;

namespace MetalLimb
{
    public interface IGeoLocalPhysicsMock
    {
        /// <summary>
        /// Reads the Geo Location.
        /// </summary>
        /// <returns>A vector representing the current geographic location ATM.</returns>
        public XYZvector ReadGeoLocationCoord();

        /// <summary>
        /// Send target Geo Location to sensor for acquisition.
        /// </summary>
        /// <param name="targetCoord">The target geographic location coordinates to transmit.</param>
        public void TransmitTargetGeoLocationCoord(XYZvector targetCoord);

        /// <summary>
        /// Reads the Acceleration vector.
        /// </summary>
        /// <returns>A vector representing the acceleration values.</returns>
        public XYZvector ReadAccelerationV();

        /// <summary>
        /// Reads the geographic location stream.
        /// </summary>
        /// <returns>The geographic location stream.</returns>
        public StreamMock ReadGeoLocationStream();
    }
}
