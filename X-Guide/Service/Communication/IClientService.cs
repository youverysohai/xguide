using System;
using System.Threading;
using System.Threading.Tasks;
using X_Guide.CustomEventArgs;

namespace X_Guide.Communication.Service
{
    public interface IClientService
    {
        Task ConnectServer();

        Task WriteDataAsync(string data);

        Task<T> RegisterSingleRequestHandler<T>(Func<NetworkStreamEventArgs, T> action, CancellationToken ct = new CancellationToken());
    }
}