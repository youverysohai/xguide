using System.Net.Sockets;
using System.Windows;
using TcpConnectionHandler;
using TcpConnectionHandler.Server;
using WinCommonUtils;

namespace ManipulatorTcp
{
    public class JogService : IJogService
    {
        private readonly IServerTcp _serverService;
        private readonly Queue<JogCommand> _jogQueue = new Queue<JogCommand>();
        private readonly BackgroundService _jogTask;

        public JogService(IServerTcp serverService)
        {
            _serverService = serverService;
            _jogTask = new BackgroundService(StartJog, true);
        }

        public void Enqueue(JogCommand jogCommand)
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
            await _serverService.WriteDataAsync(jogCommand.ToString());

            var timer = new System.Timers.Timer(300000);
            timer.AutoReset = false;
            timer.Elapsed += (s, o) => cts.Cancel();
            timer.Start();

            bool status = await _serverService.RegisterSingleRequestHandler((e) => ServerJogCommand(e, _serverService.GetConnectedClient().FirstOrDefault().Value.TcpClient.GetStream()), cts.Token);
            timer.Dispose();
            return status;
        }

        public bool ServerJogCommand(NetworkStreamEventArgs e, NetworkStream ce)
        {
            if (!e.Data![0].Trim().ToLower().Equals("jogdone") || !ce.Equals(ce)) throw new Exception("Not the awaited data");
            return true;
        }

        public void Start()
        {
            _jogTask.Start();
        }

        public void Stop()
        {
            _jogTask.Stop();
            _jogQueue.Clear();
        }
    }
}