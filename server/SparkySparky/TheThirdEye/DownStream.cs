using MetalAbs.SharedDTOs;
using MetalAbs.SharedMocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TheThirdEye
{
    public sealed class DownStream : IDownStream
    {
        public StreamMock[] ChunkGeoLocationStream()
        {
            throw new NotImplementedException();
        }

        public StreamMock GetGeoLocationStream()
        {
            throw new NotImplementedException();
        }

        public void SetCancelationToken(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Channel<XYZvector[]> SplitGeoLocationStream()
        {
            throw new NotImplementedException();
        }

        public void StopGeoLocationStream()
        {
            throw new NotImplementedException();
        }
    }
}
