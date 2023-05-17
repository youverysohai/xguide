using AutoMapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using X_Guide.Communication.Service;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step1ViewModel : ViewModelBase, ICalibrationStep
    {
        #region properties

        private readonly CalibrationViewModel _calibration;

        private event Action OnManipulatorChanged;

        private event Action<bool> OnStateChanged;

        public ManipulatorViewModel Manipulator
        {
            get
            {
                OnPropertyChanged(nameof(Manipulator.Id));
                if (_calibration.Manipulator != null)
                    OnStateChanged?.Invoke(true);
                else OnStateChanged?.Invoke(false);
                return _calibration.Manipulator;
            } //null
            set
            {
                if (_calibration.Manipulator == value) return;

                if (_calibration.Manipulator is null)
                {
                    _calibration.Manipulator = value;
                }
                else if (_messageService.ShowWarningMessage(StrRetriver.Get("WA000")).Equals(MessageBoxResult.Yes))
                {
                    _calibration.Manipulator = value;
                    _calibration.ResetProperties();
                    OnManipulatorChanged?.Invoke();
                }
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ManipulatorViewModel> _manipulators;

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

        private readonly IMessageService _messageService;

        private IMapper _mapper { get; }
        public IServerService _serverService { get; }

        #endregion properties

        public Step1ViewModel(IManipulatorDb manipulatorDb, IMapper mapper, IMessageService messageService, CalibrationViewModel calibration)
        {
            _manipulatorDb = manipulatorDb;
            _messageService = messageService;
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

        public void Register(Action action)
        {
            OnManipulatorChanged = action;
        }

        public void RegisterStateChange(Action<bool> action)
        {
            OnStateChanged = action;
        }
    }
}