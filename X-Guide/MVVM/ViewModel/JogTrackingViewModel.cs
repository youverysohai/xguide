using CommunityToolkit.Mvvm.Messaging;
using ManipulatorTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    public class JogTrackingViewModel:ViewModelBase,IRecipient<JogDoneReceived>
    {
        private readonly IMessenger _messenger;
        public bool IsJogTracking { get; set; } = false;
        public bool JogTrackingEnabled { get; set; }
        public int TrackedXMove { get; set; } = 0;
        public int TrackedYMove { get; set; } = 0;
        public RelayCommand StartJogTrackingCommand { get; set; }
        public CalibrationViewModel Calibration { get; set; }
        public JogTrackingViewModel(IMessenger messenger,CalibrationViewModel calibration)
        {
            Calibration = calibration;
            _messenger = messenger;
            messenger.Register(this); 
            StartJogTrackingCommand = new RelayCommand(StartJogTracking);
        }

        private void StartJogTracking(object obj)
        {
            IsJogTracking = !IsJogTracking;
            if (!IsJogTracking)
            {
                TrackedXMove = TrackedYMove = 0;
            }
        }
        public override void Dispose()
        {
            _messenger.UnregisterAll(this);
            base.Dispose();
        }

        public void Receive(JogDoneReceived message)
        {
            if (!IsJogTracking) return;

            JogCommand command = message.Value;
            TrackedXMove += (int)command.X;
            TrackedYMove += (int)command.Y;
        }
    }
}
