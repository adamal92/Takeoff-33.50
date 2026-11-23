using MetalAbs.SharedDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TheThirdEye
{
    public class UpSteam : IUpSteam
    {
        public void ProcessGeoLocationStream(XYZvector[] geoData)
        {
            throw new NotImplementedException();
        }
        public void ProduceGeoLocationStream(Channel<XYZvector[]> channel)
        {
            throw new NotImplementedException();
        }

        public XYZvector[] ReadGeoLocationStream(Channel<XYZvector[]> channel)
        {
            throw new NotImplementedException();
        }
    }
}
