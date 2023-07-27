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
        private readonly IRepository<Calibration> _repository;
        private readonly INavigationService _navigationService;

        public ObservableCollection<CalibrationViewModel> Calibrations { get; set; }

        public bool IsStarted { get; set; }

        private readonly ILifetimeScope _calibrationScope;

        private readonly ILifetimeScope _scope;

        public string Name { get; set; }
        public RelayCommand StartCalibCommand { get; set; }
        public RelayCommand LoadCalibCommand { get; private set; }
        public RelayCommand DeleteCalibCommand { get; private set; }

        public CalibrationWizardStartViewModel(INavigationService navigationService, IMapper mapper, IRepository<Calibration> repository, ILifetimeScope lifetimeScope)
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
            List<Calibration> calibModels = _repository.GetAll();
            Calibrations = new ObservableCollection<CalibrationViewModel>(calibModels.Select(x => _mapper.Map<CalibrationViewModel>(x)));
        }

        private async void LoadCalibration(object obj)
        {
            CalibrationViewModel calibration = obj as CalibrationViewModel;

            NamedParameter calib = new NamedParameter("cid", calibration.Id);
            await _navigationService.NavigateAsync<CalibrationMainViewModel>(calib);
        }

        private void StartCalibration(object obj)
        {
            _navigationService.Navigate<CalibrationMainViewModel>(new NamedParameter("name", Name));
        }

        public override void Dispose()
        {
            _scope?.Dispose();
            base.Dispose();
        }

        public async void DeleteCalibration(object obj)
        {
            CalibrationViewModel vmodel = obj as CalibrationViewModel;
            if (await _repository.DeleteById(vmodel.Id))
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