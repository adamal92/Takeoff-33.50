using System.Numerics;
namespace MetalLimb
{
    public interface IFullMetalFingerMock : ISensorControlMock, IGeoLocalPhysicsMock
    {
        public void SensorIsAlive();
        public bool IsSensorAlive();
        public void SimulateSensorFailure();
        public void ResetSensor();
        public void CalibrateSensor();
        public void PerformSelfTest();
        public bool IsVaultDoorOpen();
        public void OpenVaultDoor();
        public void CloseVaultDoor();
        public float GetEMFStrength();
        public Vector3 GetEMFDirection();
        public float ReadTemperature();
    }
}
