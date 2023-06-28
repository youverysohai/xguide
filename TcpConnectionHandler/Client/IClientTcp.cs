namespace TcpConnectionHandler.Client
{
    public interface IClientTcp
    {
        void ConnectServer();

        Task WriteDataAsync(string data);

        Task<T?> RegisterSingleRequestHandler<T>(Func<NetworkStreamEventArgs, T> action, CancellationToken ct = new CancellationToken());
    }
}