using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    internal class CalibrationWizardStartViewModel : ViewModelBase
    {
        private readonly IMapper _mapper;
        private readonly INavigationService _navigationService;
        private readonly ICalibrationDb _calibrationDb;
        private string _name;

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

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private bool _isStarted;

        public bool IsStarted
        {
            get { return _isStarted; }
            set { _isStarted = value; OnPropertyChanged(); }
        }

        public RelayCommand StartCalibCommand { get; set; }
        public RelayCommand LoadCalibCommand { get; private set; }
        public RelayCommand DeleteCalibCommand { get; private set; }

        public CalibrationWizardStartViewModel(INavigationService navigationService, ICalibrationDb calibrationDb, IMapper mapper)
        {
            StartCalibCommand = new RelayCommand(StartCalibration);
            LoadCalibCommand = new RelayCommand(LoadCalibration);
            DeleteCalibCommand = new RelayCommand(DeleteCalibration);
            _navigationService = navigationService;
            _calibrationDb = calibrationDb;
            _mapper = mapper;
            GetCalibrations();
            TypedParameter calib = new TypedParameter(typeof(CalibrationViewModel), new CalibrationViewModel
            {
                Name = Name,
            });
        }

        private async void GetCalibrations()
        {
            IEnumerable<CalibrationModel> calibModels = await _calibrationDb.GetAll();
            Calibrations = new ObservableCollection<CalibrationViewModel>(calibModels.Select(x => _mapper.Map<CalibrationViewModel>(x)));
        }

        private async void LoadCalibration(object obj)
        {
            TypedParameter calib = new TypedParameter(typeof(CalibrationViewModel), obj);
            CalibrationMainViewModel calibMain = await _navigationService.NavigateAsync<CalibrationMainViewModel>(calib) as CalibrationMainViewModel;
            /*            calibMain.LoadCalibSetting(calib);*/
        }

        private async void StartCalibration(object obj)
        {
            TypedParameter calib = new TypedParameter(typeof(CalibrationViewModel), new CalibrationViewModel
            {
                Name = Name,
            });

            _navigationService.Navigate<CalibrationMainViewModel>(calib);
        }

        public async void DeleteCalibration(object obj)
        {
            CalibrationViewModel vmodel = obj as CalibrationViewModel;
            if (await _calibrationDb.Delete(vmodel.Id))
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