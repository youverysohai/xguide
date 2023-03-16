using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace X_Guide.Service
{
    public class BackgroundService
    {
        private Thread _workerThread;
        private bool _isRunning;
        private readonly Action _action;


        public BackgroundService(Action action)
        {
            _action = action;
        }

        public void Start()
        {
            if (_workerThread == null)
            {
                _isRunning = true;
                _workerThread = new Thread(DoWork);
                _workerThread.Start();
            }
        }

        public void Stop()
        {
            _isRunning = false;
/*            _workerThread?.Join();*/
            _workerThread = null;
   

        }

        private void DoWork()
        {

            while (_isRunning)
            {
                _action();
                Thread.Sleep(1000);
            }

        }


    }
}
