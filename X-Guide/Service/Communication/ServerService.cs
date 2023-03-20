using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.CustomEventArgs;
using X_Guide.Service.Communication;
using X_Guide.Service.Communication;


namespace X_Guide.Communication.Service
{
    public class ServerService : TCPBase, IServerService
    {

        private int _port { get; }
        private IPAddress _ip { get; }
        private TcpListener _server;

        private bool started = false;

        public event EventHandler<TcpClientEventArgs> ClientEvent;
        public event EventHandler<TcpListenerEventArgs> ListenerEvent;

        public event EventHandler<TcpClientEventArgs> MessageEvent;

        private readonly ConcurrentDictionary<int, TcpClientInfo> _connectedClient = new ConcurrentDictionary<int, TcpClientInfo>();

        CancellationTokenSource cts;




        public ServerService(IPAddress ip, int port, string terminator)
        {
            _port = port;
            _ip = ip;
            Terminator = terminator;
        }

        public bool getServerStatus()
        {
            return started;
        }

        public void StopServer()
        {
            cts.Cancel();
        }


        public async Task StartServer()
        {
            cts = new CancellationTokenSource();

            _server = new TcpListener(_ip, _port);
            try
            {

                _server.Start();
                started = true;
                ListenerEvent?.Invoke(this, new TcpListenerEventArgs(_server));


                CancellationToken sct = cts.Token;


                while (!cts.IsCancellationRequested)
                {
                    TcpClient client = await Task.Run(() => _server.AcceptTcpClientAsync(), cts.Token);

                    Debug.WriteLine("Client connected.");

                    _connectedClient.TryAdd(client.GetHashCode(), new TcpClientInfo(client));


                    // Handle the client connection in a separate task.
#pragma warning disable CS4014 // This warning has to be suppressed to disallow the await keyword from blocking the task
                    Task.Run(() => RecieveDataAsync(client.GetStream(), cts.Token));
#pragma warning restore CS4014
                }
            }
            catch
            {
                Debug.WriteLine($"Server is closed.");
                started = false;
                ListenerEvent?.Invoke(this, new TcpListenerEventArgs(_server));
            }

        }

        public async Task StartJogCommand(TcpClientInfo client, Queue<string> commandQueue, CancellationToken cancellationToken)
        {


            while (!cancellationToken.IsCancellationRequested)
            {

                if (commandQueue.Count <= 0)
                {
                    Thread.Sleep(1000);
                    continue;
                }
             
                string jogCommand = commandQueue.Dequeue();

                await ServerWriteDataAsync(jogCommand, client.TcpClient.GetStream());
                while (await Task.Run(() => RegisterRequestEventHandler((e) => ServerJogCommand(e, client.TcpClient.GetStream())))) { };
      

                //check if the message can be recieved by the client
                /* 

                   client.Jogging = false;

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

                   }*/
            }

            Debug.WriteLine("Jog session ended!");
        }


        public void DisposeClient(TcpClient client)
        {
            client.Close();
            _connectedClient.TryRemove(client.GetHashCode(), out _);
            ClientEvent?.Invoke(this, new TcpClientEventArgs(client));
            client.Dispose();
        }

        public bool ServerJogCommand(NetworkStreamEventArgs e, NetworkStream ce)
        {
            if (!e.Data[0].Trim().ToLower().Equals("jogdone") || !e.Equals(ce)) return false;
            return true;
        }

        public ConcurrentDictionary<int, TcpClientInfo> GetConnectedClient()
        {
            return _connectedClient;
        }

        public TcpClientInfo GetConnectedClientInfo(TcpClient tcpClient)
        {
            if (tcpClient == null) return null;
            TcpClientInfo tcpClientInfo;
            _connectedClient.TryGetValue(tcpClient.GetHashCode(), out tcpClientInfo);
            return tcpClientInfo;

        }

        public void SetServerReadTerminator(string terminator)
        {
            Terminator = terminator;
        }

        public async Task ServerWriteDataAsync(string data, NetworkStream stream)
        {
            await WriteDataAsync(data, stream);
        }
    }
}








