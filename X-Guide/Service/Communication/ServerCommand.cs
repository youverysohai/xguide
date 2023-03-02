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

        public IServerService _serverService { get; }
        public ServerCommand(IServerService serverService)
        {
       
            _serverService = serverService;
            _serverService.MessageEvent += ValidateSyntax;
        }

      

        public void ValidateSyntax(object sender, TcpClientEventArgs tcpClientEventArgs)
        {
            string message = tcpClientEventArgs.Message;
            TcpClient client = tcpClientEventArgs.TcpClient;
            var commands = message.Split(' ');
            switch (commands[0].Trim().ToLower())
            {
                case "status": CheckStatus(client); break;
                case "xguide": XGuideCommand(client, message);  break;
                case "getpose" : GetPoseCommand(client, message); break;
                 
                
                default: ReturnErrorMessage(client);  break;

                   
            }
        }

        private void GetPoseCommand(TcpClient client, string message)
        {
            var commands = message.Split(',');
        }

        private void XGuideCommand(TcpClient client, string message)
        {
            var commands = message.Split(',');
            string GlobalTool = commands[1];
            string FoundStatus = commands[2];
            string X = commands[3];
            string Y = commands[4];
            string Rz = commands[5];
            throw new NotImplementedException();
        }

        public void ReturnErrorMessage(TcpClient client)
        {
            _serverService.SendMessageAsync("Unknown command :< \n", client.GetStream());
        }

        public void CheckStatus(TcpClient client)
        {
            _serverService.SendMessageAsync("Received your message!\n", client.GetStream());
            CommandEvent?.Invoke(this, new TcpClientEventArgs(client));
        }
    }
}
