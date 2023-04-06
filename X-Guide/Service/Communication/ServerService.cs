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
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;
using Xlent_Vision_Guided;

namespace X_Guide.Communication.Service
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ServerService : TCPBase, IServerService
    {

        private int _port { get; }
        private IPAddress _ip { get; }

        private readonly ICalibrationDb _calibrationDb;
        private readonly IClientService _clientService;
        private BackgroundService searchClient;
        private TcpListener _server;
        private bool valid = false;
        private bool started = false;
        private TcpClientInfo client;
        public event EventHandler<TcpClientEventArgs> ClientEvent;
        public event EventHandler<TcpListenerEventArgs> ListenerEvent;

        public event EventHandler<TcpClientEventArgs> MessageEvent;

        private readonly ConcurrentDictionary<int, TcpClientInfo> _connectedClient = new ConcurrentDictionary<int, TcpClientInfo>();

        CancellationTokenSource cts;


        public ServerService(IPAddress ip, int port, string terminator, ICalibrationDb calibrationDb, IClientService clientService) : base(terminator)
        {
            _port = port;
            _ip = ip;
            _calibrationDb = calibrationDb;
            _clientService = clientService;
            searchClient = new BackgroundService(SearchForClient, true);
            searchClient.Start();

        }

        private void SearchForClient()
        {

            if (_connectedClient.Count == 0)
            {
                valid = false;
                Debug.WriteLine(_connectedClient.Count());
            }
            else
            {
                valid = true;
                Debug.WriteLine("Connected");
                client = _connectedClient.FirstOrDefault().Value;
            }
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
            _dataReceived += ProcessCommand;
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
                    Task.Run(async () => { 
                        await RecieveDataAsync(client.GetStream(), cts.Token);
                        _connectedClient.TryRemove(client.GetHashCode(), out _);
                    });
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

        private void ProcessCommand(object sender, NetworkStreamEventArgs e)
        {
            var data = e.Data.Select(x => x.Trim().ToLower()).ToList();
            if (!data[0].Equals("xguide") || data.Count < 3) return;
            CalibrateOperation(data[1], data[2]);

        }


        private async void CalibrateOperation(string calibName, string flowName)
        {
            /*   CalibrationViewModel calibration = await _calibrationDb.GetCalibration(calibName);
               if (calibration == null) return;
                
               Point Vis_Center = await _clientService.GetVisCenter();
               VisionGuided.EyeInHandConfig2D_Operate(Vis_Center, 15, new double[] { calibration.CXOffSet, calibration.CYOffset, calibration.CRZOffset });*/
            MessageBox.Show("Start operation!");

        }

        public async Task SendJogCommand(JogCommand jogCommand)
        {
            if (valid)
            {
                await ServerWriteDataAsync(jogCommand.ToString());
                while (await Task.Run(() => RegisterRequestEventHandler((e) => ServerJogCommand(e, client.TcpClient.GetStream())))) { };
            }
            else
            {
                MessageBox.Show("Requires client connections!");
            }

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

        public async Task ServerWriteDataAsync(string data)
        {
            await WriteDataAsync(data, _connectedClient.FirstOrDefault().Value.TcpClient.GetStream());
        }
    }
}








