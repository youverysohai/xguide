using CalibrationProvider;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Threading;
using VisionGuided;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    internal class Step5TopConfigViewModel
    {
        public string Text { get; set; }

        private readonly ICalibrationService _calibrationService;
        private readonly IMessenger _messenger;
        private readonly ManualResetEventSlim manual;
        public NinePointCalibrationViewModel NinePoint { get; set; }
        public JogControllerViewModel JogController { get; set; }
        public ObservableCollection<bool> NinePointState { get; set; } = new ObservableCollection<bool>(new bool[9]);
        public RelayCommand NextCommand { get; set; }
        public RelayCommand StartCommand { get; set; }

        public Step5TopConfigViewModel(JogControllerViewModel controller, ICalibrationService calibrationService, NinePointCalibrationViewModel ninePoint, IMessenger messenger)
        {
            NinePoint = ninePoint;
            _calibrationService = calibrationService;
            _messenger = messenger;
            JogController = controller;

            StartCommand = new RelayCommand(Start9Point);
        }

        private async void Start9Point()
        {

            Point[] points = await _messenger.Send(new NinePointData(DataType.Manipulator));
            foreach (var point in points)
            {
                Debug.WriteLine(point);
            }

        }
    }
}