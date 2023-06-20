using CommunityToolkit.Mvvm.Messaging;
using System.Threading;

namespace CalibrationTesting
{
    internal class Jasper : IRecipient<SomethingRequest>
    {
        public Jasper()
        {
            WeakReferenceMessenger.Default.Register(this);
        }

        public void Receive(SomethingRequest message)
        {
            Thread.Sleep(5000);
            message.Reply(true);
        }
    }
}