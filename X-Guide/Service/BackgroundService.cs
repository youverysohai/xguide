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
        private CancellationTokenSource _cts;
        private readonly Action _action;
        private bool _repeat;
        private int _delay;

        public BackgroundService(Action action, bool repeat = false, int delay = 1000)
        {
            _action = action;
            _repeat = repeat;
            _delay = delay;
          
        }

        public void Start()
        {
            if (_workerThread == null)
            {
                _cts = new CancellationTokenSource();
                _workerThread = new Thread(() => DoWork(_cts.Token));
                _workerThread.Start();
            }
        }

        public void Stop()
        {
            _cts.Cancel();
            _workerThread?.Join();
            _workerThread = null;
   

        }

        private async void DoWork(CancellationToken ct)
            {
            if (_repeat)
            {
                while (!ct.IsCancellationRequested)
                {
                    await Task.Run(() => _action());
                    Thread.Sleep(_delay);
                }
            }
            else
            {
                await Task.Run(() => _action());
            }

        }


    }
}
