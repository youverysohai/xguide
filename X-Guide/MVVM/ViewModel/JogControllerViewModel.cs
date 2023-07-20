using ManipulatorTcp;
using System.Runtime.Versioning;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.State;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    public class JogControllerViewModel
    {
        private readonly IJogService _jogService;

        public RelayCommand JogCommand { get; }
        public int JogDistance { get; set; }
        public int RotationAngle { get; set; }
        public CalibrationViewModel Calibration { get; set; }

        public StateViewModel AppState { get; set; }
        public JogControllerViewModel(IJogService jogService,StateViewModel state)
        {
            AppState = state;
            _jogService = jogService;
            JogCommand = new RelayCommand(Jog);
            _jogService.Start();
        }

        private void Jog(object parameter)
        {
            if (JogDistance == 0) JogDistance = 10;
            if (RotationAngle == 0) RotationAngle = 10;
            int x = 0, y = 0, z = 0, rz = 0, rx = 0, ry = 0;

            switch (parameter)
            {
                case "Y+": y = JogDistance; break;
                case "Y-": y = -JogDistance; break;
                case "X+": x = JogDistance; break;
                case "X-": x = -JogDistance; break;
                case "Z+": z = JogDistance; break;
                case "Z-": z = -JogDistance; break;
                case "RZ+": rz = RotationAngle; break;
                case "RZ-": rz = -RotationAngle; break;
                case "RX+": rx = RotationAngle; break;
                case "RX-": rx = -RotationAngle; break;
                case "RY+": ry = RotationAngle; break;
                case "RY-": ry = -RotationAngle; break;
            }
            JogCommand command = new JogCommand().SetX(x).SetY(y).SetZ(z).SetRZ(rz).SetSpeed(Calibration.Speed).SetAcceleration(Calibration.Acceleration);
            _jogService.Enqueue(command);
        }
    }
}