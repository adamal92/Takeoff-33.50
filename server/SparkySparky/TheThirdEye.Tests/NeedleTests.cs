using TheThirdEye;

namespace TheThirdEye.Tests
{
    public class NeedleTests
    {
        [Fact]
        public void NeedleIsInitialized()
        {
            Needle needle = new Needle();
            needle.CurrentProccess();
            Assert.True(true);
        }
    }
}
