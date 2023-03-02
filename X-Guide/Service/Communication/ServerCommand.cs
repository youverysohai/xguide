using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                case "status": CheckStatus(client); break;
                case "xguide": XGuideCommand(client, message); break;
                case "getpose": GetPoseCommand(client, message); break;


                default: ReturnErrorMessage(client); break;


            }



        }

        private void InProgressErrorMessage(TcpClient client)
        {
            _serverService.SendMessageAsync("Server is waiting for the appropriate response, your command is discarded.\n", client.GetStream());
        }
        private async void JogManipulator(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            List<string> jogCommand = new List<string>
            {
                "JOG,GLOBAL/TOOL,X,Y,Z,RX,RY,RZ,SPEED,ACCEL1\n",
                "JOG,GLOBAL/TOOL,X,Y,Z,RX,RY,RZ,SPEED,ACCEL2\n",
                "JOG,GLOBAL/TOOL,X,Y,Z,RX,RY,RZ,SPEED,ACCEL3\n",
            };

            byte[] buffer = new byte[1024];

            string data = "";

            foreach (string command in jogCommand)
            {
                _serverService.SendMessageAsync(command, stream);

                while (true)
                {

                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    data += Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    if (data.EndsWith("\n"))
                    {
                        Debug.WriteLine(data + " Inside loop");
                        string[] messages = data.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        // Handle the received message
                        // Convert the incoming data to a string and display it.
                        Debug.WriteLine("Received message: {0}", messages[0]);
                        if (data.Trim().ToLower() == "jogdone")
                        {
                            break;
                        }
                        else
                        {
                            InProgressErrorMessage(client);
                        }
                    }
                    data = "";


                    if (bytesRead == 0)
                    {

                        Debug.WriteLine("Client disconnected.");
                        return;
                    }
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
