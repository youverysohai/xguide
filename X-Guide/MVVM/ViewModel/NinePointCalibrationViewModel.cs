using CalibrationProvider;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using X_Guide.Extension.Model;
using Point = VisionGuided.Point;
using RelayCommand = X_Guide.MVVM.Command.RelayCommand;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    internal class NinePointCalibrationViewModel : ViewModelBase, IRecipient<NinePointData>
    {
        private readonly ICalibrationService _calibrationService;
        private readonly IMessenger _messenger;
        public string Header { get; set; }
        public int CurrentPosition { get; set; } = 1 ;

        public ObservableCollection<bool> NinePointState { get; set; } = new ObservableCollection<bool>(new bool[9]);
        public ObservableCollection<BorderItem> BorderItems { get; set; } = new ObservableCollection<BorderItem>();

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
            CreateBorderItems();
        }

        private void CreateBorderItems()
        {
            BorderItems.Add(new BorderItem { Text = "1", Status = NinePointState[0], Row = 0, Column = 0 });
            BorderItems.Add(new BorderItem { Text = "2", Status = NinePointState[1], Row = 0, Column = 1 });
            BorderItems.Add(new BorderItem { Text = "3", Status = NinePointState[2], Row = 0, Column = 2 });
            BorderItems.Add(new BorderItem { Text = "4", Status = NinePointState[3], Row = 1, Column = 0 });
            BorderItems.Add(new BorderItem { Text = "5", Status = NinePointState[4], Row = 1, Column = 1 });
            BorderItems.Add(new BorderItem { Text = "6", Status = NinePointState[5], Row = 1, Column = 2 });
            BorderItems.Add(new BorderItem { Text = "7", Status = NinePointState[6], Row = 2, Column = 0 });
            BorderItems.Add(new BorderItem { Text = "8", Status = NinePointState[7], Row = 2, Column = 1 });
            BorderItems.Add(new BorderItem { Text = "9", Status = NinePointState[8], Row = 2, Column = 2 });

        }

        private void UpdateBorderItemNinePointState(int index)
        {
            BorderItems[index].Status = NinePointState[index];

        }

        private void NinePointState_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewStartingIndex + 2 < 10)
                CurrentPosition = e.NewStartingIndex + 2;
            
        }

        public async Task<Point[]> LookingDownward9PointVision()
        {
            var point = await _calibrationService.LookingDownward9Point(BlockingCall, Provider.Vision);
            NinePointState[NinePointState.Count - 1] = true;
            UpdateBorderItemNinePointState(NinePointState.Count - 1);

            return point;
        }

        public async Task<Point[]> LookingDownward9PointManipulator()
        {
            var point = await _calibrationService.LookingDownward9Point(BlockingCall, Provider.Manipulator);
            NinePointState[NinePointState.Count - 1] = true;
            UpdateBorderItemNinePointState(NinePointState.Count - 1);
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
            if (arg != 0) { NinePointState[arg - 1] = true; UpdateBorderItemNinePointState(arg - 1); }
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