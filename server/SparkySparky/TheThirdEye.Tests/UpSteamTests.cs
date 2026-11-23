using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;
using TheThirdEye;
using MetalAbs.SharedDTOs;

namespace TheThirdEye.Tests
{
    public class UpSteamTests
    {
        private readonly UpSteam upSteam;

        public UpSteamTests()
        {
            this.upSteam = new UpSteam();
        }

        [Fact]
        public void ReadGeoLocationStream_ReturnsArray()
        {
            Channel<XYZvector[]> channel = Channel.CreateUnbounded<XYZvector[]>();
            XYZvector[] result = upSteam.ReadGeoLocationStream(channel);
            Assert.NotNull(result);
        }

        [Fact]
        public void ProcessGeoLocationStream_DoesNotThrow()
        {
            XYZvector[] geoData = new XYZvector[0];
            Exception ex = Record.Exception(() => upSteam.ProcessGeoLocationStream(geoData));
            Assert.Null(ex);
        }

        [Fact]
        public void ProduceGeoLocationStream_DoesNotThrow()
        {
            Channel<XYZvector[]> channel = Channel.CreateUnbounded<XYZvector[]>();
            Exception ex = Record.Exception(() => upSteam.ProduceGeoLocationStream(channel));
            Assert.Null(ex);
        }
    }
}
