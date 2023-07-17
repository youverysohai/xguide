using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Versioning;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using XGuideSQLiteDB;
using XGuideSQLiteDB.Models;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    internal class CalibrationWizardStartViewModel : ViewModelBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly INavigationService _navigationService;

        private ObservableCollection<CalibrationViewModel> _calibrations;

        public ObservableCollection<CalibrationViewModel> Calibrations
        {
            get { return _calibrations; }
            set
            {
                _calibrations = value;
                OnPropertyChanged();
            }
        }

        private bool _isStarted;
        private readonly ILifetimeScope _calibrationScope;

        public bool IsStarted
        {
            get { return _isStarted; }
            set { _isStarted = value; OnPropertyChanged(); }
        }

        private readonly ILifetimeScope _scope;

        public CalibrationViewModel Calibration { get; }
        public RelayCommand StartCalibCommand { get; set; }
        public RelayCommand LoadCalibCommand { get; private set; }
        public RelayCommand DeleteCalibCommand { get; private set; }

        public CalibrationWizardStartViewModel(INavigationService navigationService, IMapper mapper, IRepository repository, ILifetimeScope lifetimeScope)
        {
            StartCalibCommand = new RelayCommand(StartCalibration);
            LoadCalibCommand = new RelayCommand(LoadCalibration);
            DeleteCalibCommand = new RelayCommand(DeleteCalibration);
            _navigationService = navigationService;
            _mapper = mapper;
            _repository = repository;
            GetCalibrations();
        }

        private void GetCalibrations()
        {
            List<Calibration> calibModels = _repository.GetAll<Calibration>();
            Calibrations = new ObservableCollection<CalibrationViewModel>(calibModels.Select(x => _mapper.Map<CalibrationViewModel>(x)));
        }

        private async void LoadCalibration(object obj)
        {
            TypedParameter calib = new TypedParameter(typeof(CalibrationViewModel), obj);
            CalibrationMainViewModel calibMain = await _navigationService.NavigateAsync<CalibrationMainViewModel>(calib) as CalibrationMainViewModel;
        }

        private void StartCalibration(object obj)
        {
            _navigationService.Navigate<CalibrationMainViewModel>();
        }

        public override void Dispose()
        {
            _scope?.Dispose();
            base.Dispose();
        }

        public async void DeleteCalibration(object obj)
        {
            CalibrationViewModel vmodel = obj as CalibrationViewModel;
            if (await _repository.DeleteById<Calibration>(vmodel.Id))
            {
                Calibrations.Remove(vmodel);
            }
            else
            {
                System.Windows.MessageBox.Show("Delete failed!");
            }
        }
    }
}