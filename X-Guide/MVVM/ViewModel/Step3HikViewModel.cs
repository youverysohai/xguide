﻿using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ToastNotifications;
using VM.Core;
using X_Guide.Aspect;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.State;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step3HikViewModel : ViewModelBase, ICalibrationStep
    {
        private readonly Notifier _notifier;
        private readonly ViewModelState _viewModelState;
        private readonly HikVisionService _visionService;
        private readonly CalibrationViewModel _calibration;
        private ManualResetEventSlim _manual;

        public Step3HikViewModel(CalibrationViewModel calibration, IVisionService visionService, ViewModelState viewModelState, Notifier notifier)
        {
            _calibration = calibration;
            _visionService = (HikVisionService)visionService;
            _notifier = notifier;
            _viewModelState = viewModelState;
            InitView();
        }

        public CalibrationViewModel Calibration
        {
            get
            {
                return _calibration;
            }
        }

        public bool IsProcedureEditable { get; set; } = true;

        public string Procedure
        {
            get
            {
                if (_calibration.Procedure == null) OnStateChanged?.Invoke(false);
                else OnStateChanged?.Invoke(true);
                return _procedure;
            }
            set
            {
                _calibration.Procedure = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<VmProcedure> Procedures { get; set; }

        public ObservableCollection<HikVisionViewModel> Visions { get; set; }
        public Action<bool> OnStateChanged { get; private set; }

        private string _procedure => _calibration.Procedure;

        public override bool ReadyToDisplay()
        {
            if (!_isLoaded)
            {
                using (_manual = new ManualResetEventSlim(false))
                {
                    _manual.Wait();
                    _isLoaded = true;
                }
            }
            return _isLoaded;
        }

        public void Register(Action action)
        {
        }

        public void RegisterStateChange(Action<bool> action)
        {
            OnStateChanged = action;
        }

        [ExceptionHandlingAspect]
        private async Task GetProcedures()
        {
            Procedures = new ObservableCollection<VmProcedure>(await _visionService.GetAllProcedures());
        }

        private async void InitView()
        {
            await GetProcedures();
            _manual.Set();
        }
    }
}