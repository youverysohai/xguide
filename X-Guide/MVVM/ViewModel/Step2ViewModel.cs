using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Generic;
using System.Linq;
using X_Guide.Enums;
using X_Guide.MessageToken;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using Orientation = XGuideSQLiteDB.Models.Orientation;
namespace X_Guide.MVVM.ViewModel
{
    internal class Step2ViewModel : ViewModelBase
    {
        private readonly CalibrationViewModel _calibration;
        private readonly IMessenger _messenger;

        public CalibrationViewModel Calibration
        {
            get
            {
                if (_calibration.Orientation == default) _messenger.Send(new CalibrationStateChanged(PageState.Disable));
                else _messenger.Send(new CalibrationStateChanged(PageState.Enable));
                return _calibration;
            }
        }

        public List<ValueDescription> _orientationType = EnumHelperClass.GetAllIntAndDescriptions(typeof(Orientation)).ToList();

        public IEnumerable<ValueDescription> OrientationType
        {
            get { return _orientationType; }
        }

        public RelayCommand OrientationCommand { get; set; }

        public Step2ViewModel(CalibrationViewModel calibration, IMessenger messenger)
        {
            _messenger = messenger;
            _calibration = calibration;
        }
    }
}