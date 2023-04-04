using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;
using X_Guide.VisionMaster;
using Xlent_Vision_Guided;

namespace X_Guide.MVVM.ViewModel
{
    internal class CalibrationWizardStartViewModel : ViewModelBase
    {
        private IMapper _mapper;
        private readonly INavigationService _navigationService;
        private readonly IViewModelLocator _viewModelLocator;
        private readonly ICalibrationDb _calibrationDb;
        private string _name;
        private ObservableCollection<CalibrationViewModel> _calibrationList;

        public ObservableCollection<CalibrationViewModel> CalibrationList
        {
            get { return _calibrationList; }
            set { _calibrationList = value; OnPropertyChanged(); }
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
        private bool _isStarted;
        private readonly IServerService _serverService;

        public bool IsStarted
        {
            get { return _isStarted; }
            set { _isStarted = value; OnPropertyChanged(); }
        }

        public ICommand StartCalibCommand { get; set; }
        public RelayCommand LoadCalibCommand { get; private set; }

        public CalibrationWizardStartViewModel(INavigationService navigationService, ICalibrationDb calibrationDb, IMapper mapper)
        {
            StartCalibCommand = new RelayCommand(StartCalib);
            LoadCalibCommand = new RelayCommand(LoadCalib);
            _navigationService = navigationService;
            _calibrationDb = calibrationDb;
            _mapper = mapper;
            LoadAllCalibration();
         
        }

        private void LoadCalib(object obj)
        {
            var calib = new TypedParameter(typeof(CalibrationViewModel), obj);
            var calibMain = _navigationService.Navigate<CalibrationMainViewModel>(calib);
            (calibMain as CalibrationMainViewModel).LoadCalibSetting(calib);
           

        }

        private async void LoadAllCalibration()
        {
            var i = await _calibrationDb.GetAllCalibration();
            CalibrationList = new ObservableCollection<CalibrationViewModel>(i.Select(x=> _mapper.Map<CalibrationViewModel>(x)));

        }

        private void StartCalib(object arg)
        {
            var calib = new TypedParameter(typeof(CalibrationViewModel), new CalibrationViewModel
            {
                Name = Name,
            });

            _navigationService.Navigate<CalibrationMainViewModel>(calib);
        }

      
        public void DeleteCalibration(CalibrationViewModel calibration)
        {
            CalibrationList.Remove(calibration);
        }
    }
}
