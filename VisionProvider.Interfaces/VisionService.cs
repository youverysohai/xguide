using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using VisionGuided;

namespace VisionProvider.Interfaces
{
    public class VisionService : IRecipient<VisionCenterRequest>
    {
        protected CancellationTokenSource? _cancellationTokenSource;

        public VisionService(IMessenger messenger)
        {
            messenger.Register(this);
        }

        public virtual void Receive(VisionCenterRequest message)
        {
            message.Reply(new Point());
        }
    }

    public class VisionCenterRequest : AsyncRequestMessage<Point?>
    {
    }
}