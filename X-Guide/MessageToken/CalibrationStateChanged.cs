using CommunityToolkit.Mvvm.Messaging.Messages;

namespace X_Guide.MessageToken
{
    internal enum PageState
    {
        Disable,
        Enable,
        Reset,
    }

    internal class CalibrationStateChanged : ValueChangedMessage<PageState>
    {
        public CalibrationStateChanged(PageState state) : base(state)
        {
        }
    }
}