namespace MetalLimb
{
    public sealed class SensorControlMock : ISensorControlMock
    {
        public void TurnOn()
        {
            Console.WriteLine("Sensor is turned on.");
        }

        public void TurnOff()
        {
            Console.WriteLine("Sensor is turned off.");
        }

        public void ActivateSleepMode()
        {
            Console.WriteLine("Sleep mode activated.");
        }

        public void ExitSleepMode()
        {
            Console.WriteLine("Sleep mode exited.");
        }

        public void ToggleSleepMode(bool isActivated)
        {
            if (isActivated)
            {
                ActivateSleepMode();
            }
            else
            {
                ExitSleepMode();
            }
        }
    }
}
