using AutoMapper;
using IMVSCircleFindModuCs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToastNotifications;
using VM.Core;
using VMControls.Interface;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;
using X_Guide.VisionMaster;
using Xlent_Vision_Guided;
using ToastNotifications.Messages;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step6ViewModel : ViewModelBase
    {

        private CalibrationViewModel _calibration;
        public CalibrationViewModel Calibration
        {
            get { return _calibration; }
            set
            {
                _calibration = value;
                OnPropertyChanged();
            }
        }
        private IVmModule _visProcedure;

        public IVmModule VisProcedure
        {
            get { return _visProcedure; }
            set
            {
                _visProcedure = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand OperationCommand { get; set; }
        private readonly ICalibrationDb _calibDb;
        private readonly ICalibrationService _calibService;
        private readonly IMapper _mapper;
        private readonly Notifier _notifier;

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CalibrateCommand { get; set; }



        public Step6ViewModel(CalibrationViewModel calibration, ICalibrationDb calibDb, ICalibrationService calibService, IMapper mapper, Notifier notifier)
        {
            _calibration = calibration;
            _calibDb = calibDb;
            _calibService = calibService;
            _mapper = mapper;
            _notifier = notifier;
            CalibrateCommand = new RelayCommand(Calibrate);
            SaveCommand = new RelayCommand(Save);
        }

        private void Calibrate(object obj)
        {
            _calibService.EyeInHand2DConfig_Calibrate(_calibration);
        }

        private async void Save(object obj)
        {
            if (!await _calibDb.IsExist(_calibration.Id))
            {
                await _calibDb.Add(_mapper.Map<CalibrationModel>(_calibration));
                _notifier.ShowSuccess(StrRetriver.Get("SC000"));
            }
            else
            {
                await _calibDb.Update(_mapper.Map<CalibrationModel>(_calibration));
                _notifier.ShowSuccess($"{_calibration.Name} : {StrRetriver.Get("SC001")}");
            }
           
        }
     
    }
}
