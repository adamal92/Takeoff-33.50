using MetalAbs.SharedDTOs;
using MetalAbs.SharedMocks;
using System.Numerics;

namespace MetalLimb
{
    public sealed class FullMetalFingerMock : IFullMetalFingerMock
    {
        private readonly EMFTranslatorMock eMFTranslatorMock;

        private readonly ISensorControlMock sensorControlMock;

        private readonly IGeoLocalPhysicsMock geoLocalPhysicsMock;

        public FullMetalFingerMock()
        {
            this.eMFTranslatorMock = new EMFTranslatorMock();
            this.sensorControlMock = new SensorControlMock();
            this.geoLocalPhysicsMock = new GeoLocalPhysicsMock();
        }

        public FullMetalFingerMock(
            EMFTranslatorMock eMFTranslatorMock,
            ISensorControlMock sensorControlMock,
            IGeoLocalPhysicsMock geoLocalPhysicsMock
            ) {
            this.eMFTranslatorMock = eMFTranslatorMock;
            this.sensorControlMock = sensorControlMock;
            this.geoLocalPhysicsMock = geoLocalPhysicsMock;
        }

        /// <summary>
        /// Reports that the sensor is alive and functioning properly.
        /// </summary>
        /// <remarks>
        /// Outputs a status message to the console indicating that the sensor is
        /// functioning.
        /// </remarks>
        public void SensorIsAlive()
        {
            Console.WriteLine("Sensor is alive and functioning properly.");
        }

        public bool IsSensorAlive()
        {
            Console.WriteLine("Checking if the sensor is alive...");
            return true; // Simulate that the sensor is alive
        }

        public void SimulateSensorFailure()
        {
            Console.WriteLine("Simulating sensor failure...");
            throw new InvalidOperationException("Sensor has failed.");
        }

        public void ResetSensor()
        {
            Console.WriteLine("Resetting the sensor...");
            // Simulate sensor reset logic here
        }

        public void CalibrateSensor()
        {
            Console.WriteLine("Calibrating the sensor...");
            // Simulate sensor calibration logic here
        }

        public void PerformSelfTest()
        {
            Console.WriteLine("Performing self-test on the sensor...");
            // Simulate self-test logic here
        }

        public bool IsVaultDoorOpen()
        {
            XYZvector emfVector = eMFTranslatorMock.ReadEMFV(); // Simulate reading the electromagnetic field vector
            Console.WriteLine("Checking if the door is open...");
            return false; // Simulate that the door is closed
        }

        public void OpenVaultDoor()
        {
            throw new NotImplementedException();
        }

        public void CloseVaultDoor()
        {
            throw new NotImplementedException();
        }

        public float GetEMFStrength()
        {
            throw new NotImplementedException();
        }

        public Vector3 GetEMFDirection()
        {
            var emfVector = this.eMFTranslatorMock.ReadEMFV();
            return new Vector3(emfVector.X, emfVector.Y, emfVector.Z); // Simulate reading the electromagnetic field vector
        }

        public float ReadTemperature()
        {
            return (float)(new Random().NextDouble() * 70 - 20); // Simulate reading the temperature in Celsius
            //return new Random().Next(-20, 50); // Simulate reading the temperature in Celsius
        }

        public void TurnOn()
        {
            sensorControlMock.TurnOn();
        }

        public void TurnOff()
        {
            sensorControlMock.TurnOff();
        }

        public void ActivateSleepMode()
        {
            sensorControlMock.ActivateSleepMode();
        }

        public XYZvector ReadGeoLocationCoord()
        {
            return this.geoLocalPhysicsMock.ReadGeoLocationCoord();
        }

        public void TransmitTargetGeoLocationCoord(XYZvector targetCoord)
        {
            this.geoLocalPhysicsMock.TransmitTargetGeoLocationCoord(targetCoord);
        }

        public XYZvector ReadAccelerationV()
        {
            return this.geoLocalPhysicsMock.ReadAccelerationV();
        }

        public void ExitSleepMode()
        {
            this.sensorControlMock.ExitSleepMode();
        }

        public void ToggleSleepMode(bool isActivated)
        {
            this.sensorControlMock.ToggleSleepMode(isActivated);
        }

        public StreamMock ReadGeoLocationStream()
        {
            return this.geoLocalPhysicsMock.ReadGeoLocationStream();
        }
    }
}
