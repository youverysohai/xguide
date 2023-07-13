using CalibrationProvider;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Point = VisionGuided.Point;
using RelayCommand = X_Guide.MVVM.Command.RelayCommand;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    internal class NinePointCalibrationViewModel : ViewModelBase, IRecipient<NinePointData>
    {
        private readonly ICalibrationService _calibrationService;
        private readonly IMessenger _messenger;

        public int CurrentPosition { get; set; } = 0;

        public ObservableCollection<bool> NinePointState { get; set; } = new ObservableCollection<bool>(new bool[9]);
        private SemaphoreSlim _semaphore;

        public bool CanNext { get; set; } = true;
        public RelayCommand NextCommand { get; set; }

        public NinePointCalibrationViewModel(ICalibrationService calibrationService, IMessenger messenger)
        {
            NinePointState.CollectionChanged += NinePointState_CollectionChanged;
            _calibrationService = calibrationService;
            _messenger = messenger;
            _messenger.Register(this);
            NextCommand = new RelayCommand(Continue9Point, (o) => CanNext);
        }

        private void NinePointState_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CurrentPosition = e.NewStartingIndex;
        }

        public async Task<Point[]> LookingDownward9PointVision()
        {
            var point = await _calibrationService.LookingDownward9Point(BlockingCall, Provider.Vision);
            NinePointState[NinePointState.Count - 1] = true;
            return point;
        }

        public async Task<Point[]> LookingDownward9PointManipulator()
        {
            var point = await _calibrationService.LookingDownward9Point(BlockingCall, Provider.Manipulator);
            NinePointState[NinePointState.Count - 1] = true;
            return point;
        }

        private void Continue9Point(object arg)
        {
            try
            {
                _semaphore.Release();
            }
            catch
            {
            }
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
            CanNext = true;
            if (arg != 0) NinePointState[arg - 1] = true;
            NextCommand.OnCanExecuteChanged();

            using (_semaphore = new SemaphoreSlim(0))
            {
                await _semaphore.WaitAsync();
            }

            CanNext = false;
            NextCommand.OnCanExecuteChanged();
        }

        public override void Dispose()
        {
            _messenger.UnregisterAll(this);
            NinePointState.CollectionChanged -= NinePointState_CollectionChanged;
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