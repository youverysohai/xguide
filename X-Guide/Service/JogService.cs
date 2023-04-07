using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.Service.Communication;

namespace X_Guide.Service
{
    public class JogService : IJogService
    {
        private readonly IServerService _serverService;
        private Queue<JogCommand> _jogQueue = new Queue<JogCommand>();
        private CancellationTokenSource cancelJog;
        private readonly BackgroundService _jogTask;

        public JogService(IServerService serverService)
        {
            _serverService = serverService;
            _jogTask = new BackgroundService(StartJog, true);
            _jogTask.Start();
        }

        public void EnqueueJog(JogCommand jogCommand)
        {
            _jogQueue.Enqueue(jogCommand);
        }

        private void StartJog()
        {
            if (_jogQueue.Count <= 0) return;
            try
            {
                SendJogCommand(_jogQueue.Dequeue()).GetAwaiter().GetResult();
            }
            catch
            {
                MessageBox.Show("Client disconnected when sending jog command. All queued commands are deleted.");
                _jogQueue.Clear();
            }

        }

        public async Task<bool> SendJogCommand(JogCommand jogCommand)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            await _serverService.ServerWriteDataAsync(jogCommand.ToString());
            var timer = new System.Timers.Timer(5000);
            timer.AutoReset = false;
            timer.Elapsed += (s, o) => cts.Cancel();
            timer.Start();
            bool status = await Task.Run(() => _serverService.RegisterRequestEventHandler((e) => ServerJogCommand(e, _serverService.GetConnectedClient().FirstOrDefault().Value.TcpClient.GetStream()), cts.Token));
            timer.Dispose();
            return status;
        }

        public bool ServerJogCommand(NetworkStreamEventArgs e, NetworkStream ce)
        {
            if (!e.Data[0].Trim().ToLower().Equals("jogdone") || !ce.Equals(ce)) throw new Exception("Not the awaited data");
            return true;
        }

    }


}

