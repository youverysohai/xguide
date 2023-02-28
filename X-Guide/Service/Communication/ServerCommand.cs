using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.Service.Communation
{
    public class ServerCommand
    {
        public event EventHandler<TcpClientEventArgs> CommandEvent;
        public TcpClient _tcpClient { get; }
        public IServerService _serverService { get; }
        public ServerCommand(TcpClient tcpClient, IServerService serverService)
        {
            _tcpClient = tcpClient;
            _serverService = serverService;
        }

        

        public void ValidateSyntax(string message)
        {
            var commands = message.Split(' ');
            switch (commands[0].Trim().ToLower())
            {
                case "status": CheckStatus(); break;
                case "xguide": XGuideCommand(message);  break;
                case "getpose" : GetPoseCommand(message); break;
                 
                
                default: ReturnErrorMessage();  break;

                   
            }
        }

        private void GetPoseCommand(string message)
        {
            var commands = message.Split(',');
        }

        private void XGuideCommand(string message)
        {
            var commands = message.Split(',');
            string GlobalTool = commands[1];
            string FoundStatus = commands[2];
            string X = commands[3];
            string Y = commands[4];
            string Rz = commands[5];
            throw new NotImplementedException();
        }

        public void ReturnErrorMessage()
        {
            _serverService.SendMessageAsync("Unknown command :< \n", _tcpClient.GetStream());
        }

        public void CheckStatus()
        {
            _serverService.SendMessageAsync("Received your message!\n", _tcpClient.GetStream());
            CommandEvent?.Invoke(this, new TcpClientEventArgs(_tcpClient));
        }
    }
}
