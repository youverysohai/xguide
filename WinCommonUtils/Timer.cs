using System.Timers;

namespace WinCommonUtils
{
    internal class Timer : System.Timers.Timer
    {
        public Timer(int interval, ElapsedEventHandler elapsed, bool autoReset = false)
        {
            Interval = interval;
            Elapsed += elapsed;
            AutoReset = autoReset;
        }
    }
}