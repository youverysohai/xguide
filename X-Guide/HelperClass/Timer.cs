using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace X_Guide.HelperClass
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
