using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.ViewModel;
using X_Guide.Service.Communication;
using Timer = System.Timers.Timer;

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
            return _serverService.GetConnectedClient();

        }


        public void SetServerReadTerminator(string terminator)
        {
            _serverService.SetServerReadTerminator(terminator);
        }
        public void ValidateSyntax(object sender, TcpClientEventArgs tcpClientEventArgs)
        {
            string message = tcpClientEventArgs.Message;
            TcpClient client = tcpClientEventArgs.TcpClient;
            var commands = message.Split(' ');


            switch (commands[0].Trim().ToLower())
            {
                case "jogdone": JogDone(client); break;
                case "status": CheckStatus(client); break;
                case "xguide": XGuideCommand(client, message); break;
                case "getpose": GetPoseCommand(client, message); break;
                default: ReturnErrorMessage(client); break;
            }
        }


        public async void StartJogCommand(CancellationToken cancellationToken, TcpClientInfo client)
        {
            //need to handle when the client disconnected
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
                Timer timer = new Timer(5000);
                timer.AutoReset = false;
                timer.Elapsed += (sender, e) => { _jogDoneReplyRecieved.Set(); MessageBox.Show("Jog Command expired!"); ((Timer)sender).Dispose(); };
                timer.Start();
                _jogDoneReplyRecieved.Wait();
                timer.Dispose();
                _jogDoneReplyRecieved.Reset();
            }
            client.Jogging = false;
        }

        private void JogDone(TcpClient client)
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
       
        }

        public void ReturnErrorMessage(TcpClient client)
        {
            _serverService.SendMessageAsync("Unknown command.\n", client.GetStream());
        }

        public void CheckStatus(TcpClient client)
        {
            _serverService.SendMessageAsync("Received your message!\n", client.GetStream());

        }
    }
}
