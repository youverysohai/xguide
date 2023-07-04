using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using TcpConnectionHandler;
using TcpConnectionHandler.Client;
using VisionGuided;
using VisionProvider.Interfaces;
using Timer = System.Timers.Timer;

namespace TcpVisionProvider
{
    public class TcpVisionService : VisionService, IVisionService
    {
        private readonly IClientTcp _clientTcp;

        public string Trigger { get; set; } = "CAPTURE";

        public TcpVisionService(IClientTcp clientTcp, IMessenger messenger) : base(messenger)
        {
            _clientTcp = clientTcp;
        }

        public override void Receive(VisionCenterRequest message)
        {
            message.Reply(GetVisCenter().GetAwaiter().GetResult());
        }

        /// <inheritdoc/>

        public async Task<Point?> GetVisCenter()
        {
            await _clientTcp.WriteDataAsync($"{Trigger}");
            _cancellationTokenSource = new CancellationTokenSource();
            var timer = new Timer(20000);
            timer.Elapsed += (s, e) => _cancellationTokenSource.Cancel();
            timer.AutoReset = false;
            timer.Start();
            Point? point = await _clientTcp.RegisterSingleRequestHandler(GetVisCenterEvent, _cancellationTokenSource.Token);
            timer.Dispose();
            Debug.WriteLine(point);
            return point;
        }

        private Point? GetVisCenterEvent(NetworkStreamEventArgs e)
        {
            string[]? data = e.Data;

            if (data?.Length == 4)
            {
                if (data[1] != "")
                {
                    return new Point(double.Parse(data[1]), -double.Parse(data[2]), double.Parse(data[3]));
                }
                else
                {
                    return null;
                }
            }
            throw new Exception($"{this} : Data not found!");
        }

        public Task ImportSolAsync(string filepath)
        {
            throw new NotImplementedException();
        }
    }
}