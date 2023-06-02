using System;
using System.Collections.Generic;

namespace X_Guide
{
    public class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, List<object>> subscribers;

        public EventAggregator()
        {
            subscribers = new Dictionary<Type, List<object>>();
        }

        public void Publish<T>(T obj)
        {
            Type objType = obj.GetType();
            if (!subscribers.ContainsKey(objType)) return;
            List<object> handlers = subscribers[objType];
            foreach (var handler in handlers)
            {
                var action = handler as Action<T>;
                action?.Invoke(obj);
            }
        }

        public void Subscribe<T>(Action<T> handler)
        {
            Type objType = typeof(T);
            if (!subscribers.ContainsKey(objType))
            {
                subscribers[objType] = new List<object>();
            }

            subscribers[objType].Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            Type objType = typeof(T);
            if (subscribers.ContainsKey(objType))
            {
                subscribers[objType].Remove(handler);
            }
        }
    }
}