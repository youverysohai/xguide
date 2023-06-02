using System;
using System.Collections.Concurrent;

namespace X_Guide.Service
{
    internal class DisposeService : IDisposeService
    {
        public ConcurrentBag<IDisposable> disposeObjects = new ConcurrentBag<IDisposable>();

        public void Add(IDisposable dispose)
        {
            disposeObjects.Add(dispose);
        }

        public void Dispose()
        {
            foreach (IDisposable dispose in disposeObjects)
            {
                dispose?.Dispose();
            }
        }
    }
}