using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide
{
    public class StopwatchHelper : Stopwatch
    {
        
        public new void Stop(string tag)
        {
            Stop();
            Debug.WriteLine($"{tag} : Elapsed time = " + Elapsed.ToString());
        }

    }
}
