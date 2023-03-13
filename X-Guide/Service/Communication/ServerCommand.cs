using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.ViewModel;
using X_Guide.Service.Communication;

namespace X_Guide.Service.Communation
{
    public class ServerCommand
    {
        public Queue<string> commandQeueue = new Queue<string>();
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

        public ConcurrentDictionary<int, TcpClientInfo> GetConnectedClient()
        {
            try
            {
                return _serverService.GetConnectedClient();
            }
            catch
            {
                return null;
            }
        }

        public void ValidateSyntax(object sender, TcpClientEventArgs tcpClientEventArgs)
        {
            string message = tcpClientEventArgs.Message;
            TcpClient client = tcpClientEventArgs.TcpClient;
            var commands = message.Split(' ');


            switch (commands[0].Trim().ToLower())
            {

                case "jogdone": jogDone(client); break;
                case "status": CheckStatus(client); break;
                case "xguide": XGuideCommand(client, message); break;
                case "getpose": GetPoseCommand(client, message); break;


                default: ReturnErrorMessage(client); break;


            }

        }







        public async void StartJogCommand(CancellationToken cancellationToken, TcpClientInfo client)
        {
            await Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await ServerJogCommand(client);
  
                }
            });
           
            MessageBox.Show("Jog session ended!");
        }
        public async Task ServerJogCommand(TcpClientInfo client)
        {
            NetworkStream stream = client.TcpClient.GetStream();
            ManualResetEventSlim _jogDoneReplyRecieved = client.JogDoneReplyRecieved;
            while (commandQeueue.Count > 0)
            {
                client.Jogging = true;
                await Task.Run(() => _serverService.SendMessageAsync(commandQeueue.Dequeue(), stream));
                Debug.WriteLine("Sc = " + commandQeueue.Count);
                _jogDoneReplyRecieved.Wait();
                _jogDoneReplyRecieved.Reset();
            }
            client.Jogging = false;
        }

        private void jogDone(TcpClient client)
        {
            TcpClientInfo clientInfo = _serverService.GetConnectedClientInfo(client);
            ManualResetEventSlim _jogDoneReplyRecieved = clientInfo.JogDoneReplyRecieved;
            Debug.WriteLine("status : " + _jogDoneReplyRecieved.IsSet);
            if (clientInfo.Jogging)
            {
                _jogDoneReplyRecieved.Set();
               
            }
            else
            {
                _serverService.SendMessageAsync("No new jog command available!", client.GetStream());
            }
        }


        public void JogManipulator(TcpClient client)
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
