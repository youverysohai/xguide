using System;

namespace X_Guide
{
    public interface IEventAggregator
    {
        void Publish<T>(T obj);
        void Subscribe<T>(Action<T> handler);
        void Unsubscribe<T>(Action<T> handler);
    }
}