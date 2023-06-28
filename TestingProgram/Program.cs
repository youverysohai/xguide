// See https://aka.ms/new-console-template for more information
using System.Net;
using TcpConnectionHandler;
using VM.Core;

TcpConfiguration tcpConfiguration = new TcpConfiguration()
{
    IPAddress = IPAddress.Parse("192.168.10.90"),
    Port = 8080,
    Terminator = "\n",
};

//IClientTcp clientTcp = new ClientTcp(tcpConfiguration);
//IVisionService visionService = new HikVisionService(@"C:\Users\Xlent_XIR02\Desktop\circle.sol", clientTcp);
//var i = (visionService as HikVisionService)!.GetAllProcedures();
//var j = i;
VmSolution.Load(@"C:\Users\Xlent_XIR02\Desktop\circle.sol");
var i = new List<VmProcedure>();
VmSolution.Instance.GetAllProcedureObjects(ref i);
var j = i;