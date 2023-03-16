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

        public event EventHandler ClientDisconnectedEvent;
        public ServerCommand(IServerService serverService)
        {
            _serverService = serverService;
            _serverService.MessageEvent += ValidateSyntax;
            _serverService.ClientEvent += OnClientDisconnectedEvent;
        }

        private void OnClientDisconnectedEvent(object sender, TcpClientEventArgs e)
        {
            ClientDisconnectedEvent?.Invoke(sender, e);
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


        public async void StartJogCommand(TcpClientInfo client, CancellationToken cancellationToken)
        {
            //need to handle when the client disconnected

            client.Jogging = true;
            await Task.Run(() => ServerJogCommand(client, cancellationToken));
            client.Jogging = false;
            Debug.WriteLine("Jog session ended!");
        }

        public async Task ServerJogCommand(TcpClientInfo client, CancellationToken cancellationToken)
        {
            NetworkStream stream = client.TcpClient.GetStream();
            ManualResetEventSlim _jogDoneReplyRecieved = client.JogDoneReplyRecieved;


            while (!cancellationToken.IsCancellationRequested)
            {
       
                if (commandQeueue.Count <= 0)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                string jogCommand = commandQeueue.Dequeue();

                //check if the message can be recieved by the client
                bool IsMessageSent = await _serverService.SendMessageAsync(jogCommand, stream);

                if (IsMessageSent)
                {
                    Timer timer = new Timer(5000);
                    timer.AutoReset = false;
                    timer.Elapsed += (sender, e) => JogCommandExpired(sender, _jogDoneReplyRecieved);
                    timer.Start();
                    _jogDoneReplyRecieved.Wait();
                    timer.Dispose();
                    _jogDoneReplyRecieved.Reset();
                }
                else
                {
                    HandleClientDisconnection();
                    break;

                }
            }
        }

        private void JogCommandExpired(object sender, ManualResetEventSlim _jogDoneReplyRecieved)
        {
            _jogDoneReplyRecieved.Set(); Debug.WriteLine("Jog command expired!"); ((Timer)sender).Dispose();
        }

        private void HandleClientDisconnection()
        {
            commandQeueue.Clear();
            Debug.WriteLine("Client has disconnected!");
        }

        private void JogDone(TcpClient client)
        {
            TcpClientInfo clientInfo = _serverService.GetConnectedClientInfo(client);
            ManualResetEventSlim _jogDoneReplyRecieved = clientInfo.JogDoneReplyRecieved;
            if (clientInfo.Jogging)
            {
                _jogDoneReplyRecieved.Set();
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
