using TheThirdEye;

namespace SparkySparky.Services
{
    public class NeedleService : INeedleService
    {
        public void InitializeNeedle()
        {
            Needle needle = new Needle();
            needle.CurrentProccess();
        }
    }
}
