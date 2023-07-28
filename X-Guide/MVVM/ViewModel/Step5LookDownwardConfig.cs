using CalibrationProvider;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using X_Guide.Extension.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using Point = VisionGuided.Point;
using RelayCommand = X_Guide.MVVM.Command.RelayCommand;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    internal class Step5LookDownwardConfig : ViewModelBase, IRecipient<NinePointData>
    {


        private readonly CalibrationViewModel _calibration;
        private readonly ICalibrationService _calibrationService;
        private readonly IMessenger _messenger;

        public CalibrationViewModel Calibration => _calibration;


        public Provider provider { get; set; } = Provider.Manipulator;
        public string Header { get; set; }
        public int CurrentPosition { get; set; } = 1;


        //public ObservableCollection<bool> NinePointState { get; set; } = new ObservableCollection<bool>(new bool[9]);
        public ObservableCollection<Point> NinePoint { get; set; } = new ObservableCollection<Point>(Enumerable.Range(0, 9).Select(_ => new Point()));

        private SemaphoreSlim _semaphore;

        public bool CanNext { get; set; } = true;
        public RelayCommand NextCommand { get; set; }
        public RelayCommand StartCommand { get; set; }

        public Step5LookDownwardConfig(ICalibrationService calibrationService, IMessenger messenger, NinePointCalibrationViewModel ninePoint, CalibrationViewModel calibration)
        {
            _calibration = calibration;
            _calibrationService = calibrationService;
            _messenger = messenger;
            _messenger.Register(this);
            ninePoint.Header = "Camera Look Downward";
            NextCommand = new RelayCommand(Continue9Point, (o) => CanNext);
            StartCommand = new RelayCommand(Start9Point);
        }


        private async void Start9Point(object obj)
        {
            await _calibrationService.LookingDownward9Point(Calibration.RobotPoints, BlockingCall, Provider.Manipulator);

        }



        private void NinePointState_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewStartingIndex + 2 < 10)
                CurrentPosition = e.NewStartingIndex + 2;
        
        }



        //public async Task<Point[]> LookingDownward9PointManipulator()
        //{
        //    //var point = await _calibrationService.LookingDownward9Point(BlockingCall, Provider.Manipulator);

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
                //case DataType.Vision: task = _calibrationService.LookingDownward9PointVision(BlockingCall); break;
                //case DataType.Manipulator: task = _calibrationService.LookingDownward9PointManipulator(BlockingCall); break;
            }
            message.Reply(task);
        }

   

        private async Task BlockingCall(int arg)
        {

            CanNext = true;
            NextCommand.OnCanExecuteChanged();

            using (_semaphore = new SemaphoreSlim(0))
            {
                await _semaphore.WaitAsync();

            }



            CanNext = false;
            NextCommand.OnCanExecuteChanged();

        }

     
    }
}