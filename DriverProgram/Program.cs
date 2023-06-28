// See https://aka.ms/new-console-template for more information

using HikVisionProvider;
using System.Net;
using TcpConnectionHandler;
using TcpConnectionHandler.Client;
using VisionProvider.Interfaces;

TcpConfiguration tcpConfiguration = new TcpConfiguration()
{
    IPAddress = IPAddress.Parse("192.168.10.90"),
    Port = 8080,
    Terminator = "\n",
};

IClientTcp clientTcp = new ClientTcp(tcpConfiguration);
IVisionService visionService = new HikVisionService(@"C:\Users\Xlent_XIR02\Desktop\circle.sol", clientTcp);

var i = (visionService as HikVisionService);
i.ImportSol(@"C:\Users\Xlent_XIR02\Desktop\circle.sol");
var j = i.GetAllProcedures();
Console.WriteLine("Hi :)");
Console.Read();