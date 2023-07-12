using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using TcpConnectionHandler;
using TcpConnectionHandler.Server;
using VisionGuided;

namespace ManipulatorTcp
{
    public class ManipulatorTcpHandler : IRecipient<ManipulatorTcpRequest>
    {
        private readonly IServerTcp _serverTcp;

        public ManipulatorTcpHandler(IServerTcp serverTcp, IMessenger messenger)
        {
            _serverTcp = serverTcp;
            messenger.Register(this);
        }

        public void Receive(ManipulatorTcpRequest message)
        {
            message.Reply(GetCurrentPosition(message.Mode));
        }

        public async Task<Point?> GetCurrentPosition(Enum Mode)
        {
            await _serverTcp.WriteDataAsync($"GETPOSE,{Mode}");
            return await _serverTcp.RegisterSingleRequestHandler(GetCurrentPos);
        }

        private Point GetCurrentPos(NetworkStreamEventArgs args)
        {
            if (args.Data?.Length != 2) throw new Exception("Not awaited data");
            return new Point
            {
                X = double.Parse(args.Data[0]),
                Y = double.Parse(args.Data[1]),
            };
        }
    }

    public class ManipulatorTcpRequest : AsyncRequestMessage<Point?>
    {
        public readonly Mode Mode;

        public ManipulatorTcpRequest(Mode mode)
        {
            Mode = mode;
        }
    }
}