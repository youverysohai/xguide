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

        public ManipulatorViewModel Manipulator
        {
            get
            {
                OnPropertyChanged(nameof(Manipulator.Id));
                if (_calibration.Manipulator != null)
                    _messenger?.Send(new CalibrationStateChanged(PageState.Enable));
                else _messenger?.Send(new CalibrationStateChanged(PageState.Disable));
                return _calibration.Manipulator;
            }
            set
            {
                if (_calibration.Manipulator == value) return;

                if (_calibration.Manipulator is null)
                {
                    _calibration.Manipulator = value;
                }
                else if (_messenger.Send(new MessageBoxRequest(StrRetriver.Get("VI001"), BoxState.Warning)).Equals(MessageBoxResult.Yes))
                {
                    _calibration.Manipulator = value;
                    _messenger.Send(new CalibrationStateChanged(PageState.Reset));
                    _calibration.ResetProperties();
                }
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

        public Step1ViewModel(IManipulatorDb manipulatorDb, IMapper mapper, CalibrationViewModel calibration, IDisposeService disposeService, IMessenger messenger)
        {
            _messenger = messenger;
            disposeService.Add(this);
            _manipulatorDb = manipulatorDb;
            _mapper = mapper;
            _calibration = calibration;
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