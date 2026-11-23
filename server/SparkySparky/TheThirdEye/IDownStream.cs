using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;
using MetalAbs.SharedMocks;
using MetalAbs.SharedDTOs;

namespace TheThirdEye
{
    public interface IDownStream
    {
        /// <summary>
        /// Gets the geolocation stream.
        /// </summary>
        /// <returns>A stream mock containing geolocation data.</returns>
        public StreamMock GetGeoLocationStream();

        /// <summary>
        /// Chunks the geolocation stream into smaller segments for processing.
        /// </summary>
        /// <returns>A stream mocks array containing chunked geolocation data.</returns>
        public StreamMock[] ChunkGeoLocationStream();

        /// <summary>
        /// Splits the geolocation stream to a channel for concurrent processing.
        /// </summary>
        /// <returns>A channel containing geolocation data.</returns>
        public Channel<XYZvector[]> SplitGeoLocationStream();
    }
}
