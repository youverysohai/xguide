using CalibrationProvider;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Point = VisionGuided.Point;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    internal class NinePointCalibrationViewModel : ViewModelBase, IRecipient<NinePointData>
    {
        private readonly ICalibrationService _calibrationService;
        private readonly IMessenger _messenger;

        public ObservableCollection<bool> NinePointState { get; set; } = new ObservableCollection<bool>(new bool[9]);
        private SemaphoreSlim _semaphore;

        public RelayCommand NextCommand { get; set; }

        public NinePointCalibrationViewModel(ICalibrationService calibrationService, IMessenger messenger)
        {
            _calibrationService = calibrationService;
            _messenger = messenger;
            _messenger.Register(this);
            NextCommand = new RelayCommand(Continue9Point);
        }

        private void Continue9Point()
        {
            _semaphore.Release();
        }

        public void Receive(NinePointData message)
        {
            Task<Point[]> task = default;
            switch (message.Type)
            {
                case DataType.Vision: task = _calibrationService.LookingDownward9PointVision(BlockingCall); break;
                case DataType.Manipulator: task = _calibrationService.LookingDownward9PointManipulator(BlockingCall); break;
            }
            message.Reply(task);
        }

        private async Task BlockingCall(int arg)
        {
            using (_semaphore = new SemaphoreSlim(0))
            {
                await _semaphore.WaitAsync();
            }
        }

        public override void Dispose()
        {
            _messenger.UnregisterAll(this);
            base.Dispose();
        }
    }

    public enum DataType
    {
        Manipulator,
        Vision
    }

    public class NinePointData : AsyncRequestMessage<Point[]>
    {
        public DataType Type { get; set; }

        public NinePointData(DataType type = default)
        {
            Type = type;
        }
    }
}