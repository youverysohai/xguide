using System;

namespace X_Guide.Service.Communication
{
    public interface IServerCommand
    {
        void Operation(string[] parameter);

        void SubscribeOnOperationEvent(EventHandler<object> action);

        void UnsubscribeOnOperationEvent(EventHandler<object> action);
    }
}