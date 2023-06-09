using AutoMapper;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using X_Guide.Communication.Service;
using X_Guide.MessageToken;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step1ViewModel : ViewModelBase
    {
        #region properties

        private readonly CalibrationViewModel _calibration;

        private ManipulatorViewModel _manipulator;

        public ManipulatorViewModel Manipulator
        {
            get => _manipulator;

            set
            {
                _manipulator = CheckManipulatorChange(value);
                CheckEnableState();
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ManipulatorViewModel> _manipulators;
        private readonly IMessenger _messenger;

        public ObservableCollection<ManipulatorViewModel> Manipulators
        {
            get { return _manipulators; }
            set
            {
                _manipulators = value;
                OnPropertyChanged();
            }
        }

        public IManipulatorDb _manipulatorDb { get; }

        private IMapper _mapper { get; }
        public IServerService _serverService { get; }

        #endregion properties

        public ManipulatorViewModel CheckManipulatorChange(ManipulatorViewModel value)
        {
            //if null or same as initial manipulator -> nothing happens
            if (value == _calibration.Manipulator || _manipulator == null)
            {
                _calibration.Manipulator = value;
                return value;
            }

            //If different, prompt a confirmation message box -> yes : change & reset | no : nothing happens
            MessageBoxResult result = _messenger.Send(new MessageBoxRequest(StrRetriver.Get("WA000"), BoxState.Warning));
            if (result != MessageBoxResult.Yes) return _calibration.Manipulator;

            _calibration.Manipulator = value;
            _messenger.Send(new CalibrationStateChanged(PageState.Reset));
            _calibration.ResetProperties();
            return value;
        }

        public void CheckEnableState()
        {
            PageState state = _calibration.Manipulator is null ? PageState.Disable : PageState.Enable;
            _messenger.Send(new CalibrationStateChanged(state));
        }

        public Step1ViewModel(IManipulatorDb manipulatorDb, IMapper mapper, CalibrationViewModel calibration, IDisposeService disposeService, IMessenger messenger)
        {
            _messenger = messenger;
            disposeService.Add(this);
            _manipulatorDb = manipulatorDb;
            _mapper = mapper;
            _calibration = calibration;
            _manipulator = _calibration.Manipulator;
            CheckEnableState();
            GetManipulators();
        }

        private async void GetManipulators()
        {
            var models = await _manipulatorDb.GetAll();
            var viewModels = models.Select(x => _mapper.Map<ManipulatorViewModel>(x));
            Manipulators = new ObservableCollection<ManipulatorViewModel>(viewModels);
            OnPropertyChanged(nameof(Manipulator));
        }
    }
}