using MetalAbs.SharedDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TheThirdEye
{
    /// <summary>
    /// Handles a single stream of geographic data and processes it
    /// </summary>
    public interface IUpSteam
    {
        /// <summary>
        /// Reads the geographic data stream from a source <c>Channel</c> and returns an array of XYZvector objects.
        /// </summary>
        /// <param name="channel">The channel from which to read the geographic data.</param>
        /// <returns>An array of XYZvector objects.</returns>
        public XYZvector[] ReadGeoLocationStream(Channel<XYZvector[]> channel);

        /// <summary>
        /// Processes the geographic data stream.
        /// </summary>
        /// <param name="geoData">The geographic data to process.</param>
        public void ProcessGeoLocationStream(XYZvector[] geoData);

        /// <summary>
        /// Produces the processed geographic data stream and writes it to a <c>Channel</c>, returning it to the parent thread.
        /// </summary>
        /// <param name="channel">The channel to which to write the geographic data.</param>
        public void ProduceGeoLocationStream(Channel<XYZvector[]> channel);
    }
}
