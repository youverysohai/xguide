using System.Diagnostics;

namespace X_Guide
{
    public class StopwatchHelper : Stopwatch
    {
        public void Stop(string tag)
        {
            Stop();
            Debug.WriteLine($"{tag} : Elapsed time = " + Elapsed.ToString());
        }
    }
}