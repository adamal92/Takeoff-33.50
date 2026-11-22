using System;
using System.Collections.Generic;
namespace MetalLimb
{
    public interface ISensorControlMock
    {
        public void TurnOn();
        public void TurnOff();
        public void ActivateSleepMode();
        public void ExitSleepMode();
        public void ToggleSleepMode(bool isActivated);
    }
}
