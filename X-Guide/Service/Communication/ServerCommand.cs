using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.Service.Communation
{
    public class ServerCommand
    {



        public IServerService _serverService { get; }
        public ServerCommand(IServerService serverService)
        {

            _serverService = serverService;
            _serverService.MessageEvent += ValidateSyntax;
        }

        public void StartServer()
        {
            _serverService.StartServer();
        }

        public void ValidateSyntax(object sender, TcpClientEventArgs tcpClientEventArgs)
        {
            string message = tcpClientEventArgs.Message;
            TcpClient client = tcpClientEventArgs.TcpClient;
            var commands = message.Split(' ');


            switch (commands[0].Trim().ToLower())
            {
                case "jog": JogManipulator(client); break;
                case "jogdone": jogDone(client); break;
                case "status": CheckStatus(client); break;
                case "xguide": XGuideCommand(client, message); break;
                case "getpose": GetPoseCommand(client, message); break;


                default: ReturnErrorMessage(client); break;


            }



        }

        private void jogDone(TcpClient client)
        {
            ManualResetEventSlim _jogDoneReplyRecieved = _serverService.GetConnectedClientInfo(client).JogDoneReplyRecieved;

            _jogDoneReplyRecieved.Set();
        }

        private void InProgressErrorMessage(TcpClient client)
        {
            _serverService.SendMessageAsync("Server is waiting for the appropriate response, your command is discarded.\n", client.GetStream());
        }
        private void JogManipulator(TcpClient client)
        {
            var clientInfo = _serverService.GetConnectedClientInfo(client);
            ManualResetEventSlim _jogDoneReplyRecieved = clientInfo.JogDoneReplyRecieved;


            NetworkStream stream = client.GetStream();
            {
                if (!clientInfo.Jogging)
                {
                    clientInfo.Jogging = true;

                    List<string> jogCommand = new List<string>
            {
                "JOG,GLOBAL/TOOL,X,Y,Z,RX,RY,RZ,SPEED,ACCEL1\n",
                "JOG,GLOBAL/TOOL,X,Y,Z,RX,RY,RZ,SPEED,ACCEL2\n",
                "JOG,GLOBAL/TOOL,X,Y,Z,RX,RY,RZ,SPEED,ACCEL3\n",
            };

                    foreach (string command in jogCommand)
                    {
                        _serverService.SendMessageAsync(command, stream);
                        _jogDoneReplyRecieved.Wait(); //note that literally every thread is sharing this thing
                        _jogDoneReplyRecieved.Reset();
                    }
                    clientInfo.Jogging = false;
                }
                else
                {
                    _serverService.SendMessageAsync("Jog is in progress. Please refrain from initiating another jog command before completing the current cycle.\n", stream);
                }
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

        }
    }
}
