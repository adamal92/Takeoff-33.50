using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;
using TheThirdEye;
using MetalAbs.SharedMocks;
using MetalAbs.SharedDTOs;

namespace TheThirdEye.Tests
{
    public class DownStreamTests
    {
        protected readonly DownStream downStream;

        public DownStreamTests()
        {
            this.downStream = new DownStream();
        }

        [Fact]
        public void ChunkGeoLocationStream_ReturnsArray()
        {
            StreamMock[] result = downStream.ChunkGeoLocationStream();
            Assert.NotNull(result);
        }

        [Fact]
        public void GetGeoLocationStream_ReturnsStreamMock()
        {
            StreamMock result = downStream.GetGeoLocationStream();
            Assert.NotNull(result);
        }

        [Fact]
        public void SplitGeoLocationStream_ReturnsChannel()
        {
            Channel<XYZvector> result = downStream.SplitGeoLocationStream();
            Assert.NotNull(result);
        }
    }
}
