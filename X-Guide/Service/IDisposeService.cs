using System;

namespace X_Guide.Service
{
    internal interface IDisposeService
    {
        void Add(IDisposable dispose);
        void Dispose();
    }
}