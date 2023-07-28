using CalibrationProvider;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using X_Guide.CustomControls.Layouts;
using X_Guide.Extension.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using Point = VisionGuided.Point;
using RelayCommand = X_Guide.MVVM.Command.RelayCommand;

namespace X_Guide.MVVM.ViewModel
{



    [SupportedOSPlatform("windows")]
    internal class NinePointCalibrationViewModel : ViewModelBase, IRecipient<NinePointData>
    {
        private readonly ICalibrationService _calibrationService;
        private readonly IMessenger _messenger;
        public CalibrationViewModel Calibration { get; set; }

        public Provider provider { get; set; } = Provider.Manipulator;
        public string Header { get; set; }
        public int CurrentPosition { get; set; } = 1;


        //public ObservableCollection<bool> NinePointState { get; set; } = new ObservableCollection<bool>(new bool[9]);
        public ObservableCollection<Point> NinePoint { get; set; } = new ObservableCollection<Point>(Enumerable.Range(0, 9).Select(_ => new Point()));

        private SemaphoreSlim _semaphore;

        public bool CanNext { get; set; } = true;
        public RelayCommand NextCommand { get; set; }
        public RelayCommand StartCommand { get; set; }
        public RelayCommand SaveNinePointCommand { get; set; }


        public NinePointCalibrationViewModel(ICalibrationService calibrationService, IMessenger messenger, CalibrationViewModel calibration)
        {
            //Calibration = calibration;
            ////NinePointState.CollectionChanged += NinePointState_CollectionChanged;
            //_calibrationService = calibrationService;
            _messenger = messenger;
            _messenger.Register(this);
            //NextCommand = new RelayCommand(Continue9Point, (o) => CanNext);
            //StartCommand = new RelayCommand(Start9Point);
            //SaveNinePointCommand = new RelayCommand(SaveNinePoint);
            //CreateBorderItems();
        }

        private void SaveNinePoint(object obj)
        {
          //for (int i = 0; i < BorderItems.Count; i++)
          //  {

          //      Calibration.RobotPoints[i].X = BorderItems.ElementAt(i).XCoordinate;
          //      Calibration.RobotPoints[i].Y = BorderItems.ElementAt(i).YCoordinate;
          //  }
        }

        //private async void Start9Point(object obj)
        //{
        //    switch (provider)
        //    {
        //        case Provider.Manipulator: await LookingDownward9PointManipulator(); break;
        //        case Provider.Vision: await LookingDownward9PointVision(); break;
        //    }
        //}

        private void CreateBorderItems()
        {
            //BorderItems.Add(new BorderItem { XCoordinate = 0.0, YCoordinate = 0.0, Text = "1", Status = NinePointState[0], Row = 1, Column = 0 });
            //BorderItems.Add(new BorderItem { XCoordinate = 0.0, YCoordinate = 0.0, Text = "2", Status = NinePointState[1], Row = 1, Column = 1 });
            //BorderItems.Add(new BorderItem { XCoordinate = 0.0, YCoordinate = 0.0, Text = "3", Status = NinePointState[2], Row = 1, Column = 2 });
            //BorderItems.Add(new BorderItem { XCoordinate = 0.0, YCoordinate = 0.0, Text = "4", Status = NinePointState[3], Row = 2, Column = 0 });
            //BorderItems.Add(new BorderItem { XCoordinate = 0.0, YCoordinate = 0.0, Text = "5", Status = NinePointState[4], Row = 2, Column = 1 });
            //BorderItems.Add(new BorderItem { XCoordinate = 0.0, YCoordinate = 0.0, Text = "6", Status = NinePointState[5], Row = 2, Column = 2 });
            //BorderItems.Add(new BorderItem { XCoordinate = 0.0, YCoordinate = 0.0, Text = "7", Status = NinePointState[6], Row = 3, Column = 0 });
            //BorderItems.Add(new BorderItem { XCoordinate = 0.0, YCoordinate = 0.0, Text = "8", Status = NinePointState[7], Row = 3, Column = 1 });
            //BorderItems.Add(new BorderItem { XCoordinate = 0.0, YCoordinate = 0.0, Text = "9", Status = NinePointState[8], Row = 3, Column = 2 });


        }

        private void UpdateBorderItemNinePointState(int index)
        {
            //BorderItems[index].Status = NinePointState[index];

        }

        private void NinePointState_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewStartingIndex + 2 < 10)
                CurrentPosition = e.NewStartingIndex + 2;

        }

        //public async Task<Point[]> LookingDownward9PointVision()
        //{
        //    //var point = await _calibrationService.LookingDownward9Point(BlockingCall, Provider.Vision);
        //    //NinePointState[NinePointState.Count - 1] = true;
        //    //UpdateBorderItemNinePointState(NinePointState.Count - 1);
          
        //    //Calibration.VisionPoints = new ObservableCollection<Point>(point);
        //    //return point;
        //}

        //public async Task<Point[]> LookingDownward9PointManipulator()
        //{
        //    //var point = await _calibrationService.LookingDownward9Point(BlockingCall, Provider.Manipulator);
        //    //NinePointState[NinePointState.Count - 1] = true;
        //    //UpdateBorderItemNinePointState(NinePointState.Count - 1);
        //    //Calibration.RobotPoints = new ObservableCollection<Point>(point);

        //    //return point;
        //}

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
               //case DataType.Manipulator: task = _calibrationService.LookingDownward9PointManipulator(BlockingCall); break;
            }
            message.Reply(task);
        }

        private async Task BlockingCall(int arg)
        {
            CanNext = true;
            if (arg != 0) { Calibration.RobotPoints[arg - 1] = new Point(); UpdateBorderItemNinePointState(arg - 1); }
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
            //NinePointState.CollectionChanged -= NinePointState_CollectionChanged;
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