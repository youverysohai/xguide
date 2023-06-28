using System.Net;

namespace TcpConnectionHandler
{
    public record TcpConfiguration
    {
        public IPAddress IPAddress { get; set; } = IPAddress.Parse("127.0.0.1");
        public int Port { get; set; } = 8080;
        public string? Terminator { get; set; } = "\n";
    }
}