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
    /// <summary>
    /// Breaks down a geolocation data stream into smaller chunks and hands them for concurrent processing.
    /// Specifies the contract for downstream operations related to geolocation data streams.
    /// Provides operations for retrieving and preparing geolocation data streams for processing.
    /// </summary>
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

        /// <summary>
        /// Stops the geolocation stream processing and releases any associated resources.
        /// </summary>
        public void StopGeoLocationStream();

        /// <summary>
        /// Sets the cancellation token used to signal cancellation.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        public void SetCancelationToken(CancellationToken cancellationToken);
    }
}
