using System.Numerics;

namespace MetalLimb
{
    public sealed class XYZvector
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

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
