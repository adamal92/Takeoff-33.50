using MetalAbs.SharedDTOs;
using System.Numerics;

namespace MetalLimb
{
    public sealed class EMFTranslatorMock
    {
        /// <summary>
        /// Reads the electromagnetic field vector.
        /// </summary>
        /// <returns>A vector representing the electromagnetic field values.</returns>
        public XYZvector ReadEMFV()
        {
            Console.WriteLine("Sensor is alive and functioning properly.");
            return new XYZvector { X = 0, Y = 0, Z = 0 };
        }
    }
}
