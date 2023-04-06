using AutoMapper;
using ModernWpf.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using VM.Core;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;
using X_Guide.Validation;
using X_Guide.VisionMaster;
using Xlent_Vision_Guided;

namespace X_Guide.MVVM.ViewModel
{
    public class SettingViewModel : ViewModelBase, INotifyDataErrorInfo
    {

        public RelayCommand SaveCommand { get; }
        public RelayCommand ManipulatorCommand { get; set; }
        public RelayCommand VisionCommand { get; set; }
 


        private readonly IManipulatorDb _manipulatorDb;
        private readonly IVisionDb _visionDb;
        private readonly ICalibrationDb _calibrationDb;
        private readonly IMapper _mapper;
        private readonly IClientService _clientService;
        private readonly IServerService _serverService;
        private readonly IVisionService _visionService;
        private readonly ErrorViewModel _errorViewModel;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => false;

        private GeneralSettingViewModel _server;

        public GeneralSettingViewModel Server
        {
            get { return _server; }
            set { _server = value; OnPropertyChanged(); }
        }


        private VisionViewModel _vision;

        private ObservableCollection<CalibrationModel> _calibSol;

        public ObservableCollection<CalibrationModel> CalibSol
        {
            get { return _calibSol; }
            set
            {
                _calibSol = value;
                OnPropertyChanged();
            }
        }

        private Calibration _calib;

        public Calibration Calib
        {
            get { return _calib; }
            set
            {
                _calib = value;
                OnPropertyChanged();

            }
        }
        double[] calibData;

        public RelayCommand OperationCommand { get; set; }

        public VisionViewModel Vision
        {
            get { return _vision; }
            set
            {
                _vision = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<VisionViewModel> _visions;

        public ObservableCollection<VisionViewModel> Visions
        {
            get { return _visions; }
            set
            {
                _visions = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NewVisionCommand { get; set; }

        private VisionViewModel _newVision;

        public VisionViewModel NewVision
        {
            get { return _newVision; }
            set { _newVision = value; OnPropertyChanged(); }
        }


        public RelayCommand NewManipulatorCommand { get; set; }

        private ManipulatorViewModel _newManipulator = new ManipulatorViewModel();

        public ManipulatorViewModel NewManipulator
        {
            get { return _newManipulator; }
            set { _newManipulator = value; OnPropertyChanged(); }
        }


        private ManipulatorViewModel _manipulator;

        public ManipulatorViewModel Manipulator
        {
            get { return _manipulator; }
            set
            {
                _manipulator = value;
                OnPropertyChanged();
            }
        }

        //SettingViewModel properties
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

        private IEnumerable<ValueDescription> _manipulatorTypeList;

        public IEnumerable<ValueDescription> ManipulatorTypeList
        {
            get { return _manipulatorTypeList; }
            set
            {
                _manipulatorTypeList = value;
                OnPropertyChanged();
            }
        }




        private string _logFilePath;
        public string LogFilePath
        {
            get { return _logFilePath; }
            set
            {
                _logFilePath = value;
                OnPropertyChanged(nameof(LogFilePath));
            }
        }

        private IEnumerable<ValueDescription> _terminatorList;

        public IEnumerable<ValueDescription> TerminatorList
        {
            get { return _terminatorList; }
            set
            {
                _terminatorList = value;
                OnPropertyChanged();
            }
        }

        public SettingViewModel(IManipulatorDb machineDb, IVisionDb visionDb, IMapper mapper, ICalibrationDb calibrationDb, IClientService clientService, IServerService serverService, IVisionService visionService)
        {
            _manipulatorDb = machineDb;
            _visionDb = visionDb;
            _calibrationDb = calibrationDb;
            _mapper = mapper;
            _clientService = clientService;
            _serverService = serverService;
            _visionService = visionService;

            GetAllMachine();
            GetVisions();
            //Visions = new ObservableCollection<VisionViewModel>
            //{
            //    new VisionViewModel
            //    {
            //        Name = "Vision1",
            //        Ip = new ObservableCollection<string>(new string[] { "192", "168", "11", "90" }),
            //        Port = 8000,
            //        Terminator = ""

            //    } , 
            //    new VisionViewModel
            //    {
            //    Name = "Hik",
            //    Ip = new ObservableCollection<string>(new string[]{"192", "163","11","33"}),
            //    Port=8000,
            //    Terminator="\r"
            //    } , new VisionViewModel
            //    {
            //    Name = "Vision2",
            //    Ip = new ObservableCollection<string>(new string[]{"192", "128","21","90"}),
            //    Port=8000,
            //    Terminator="\r\n"}  };
            Server = new GeneralSettingViewModel
            {
                Ip = new ObservableCollection<string>(new string[] { "192", "168", "11", "90" }),
                Port = 8000,
                Terminator = ""

            };


            ManipulatorTypeList = EnumHelperClass.GetAllIntAndDescriptions(typeof(ManipulatorType));
            TerminatorList = EnumHelperClass.GetAllValuesAndDescriptions(typeof(Terminator));

            LoadAllCalibFile();
            VisionCommand = new RelayCommand(OnVisionChangeEvent);
            ManipulatorCommand = new RelayCommand(OnManipulatorChangeEvent);
            SaveCommand = new RelayCommand(SaveSetting);
            OperationCommand = new RelayCommand(Operation);
            NewManipulatorCommand = new RelayCommand(AddNewManipulator);
            NewVisionCommand = new RelayCommand(AddNewVision);
        }

        private void AddNewVision(object obj)
        {
            //bool saveStatus = await _manipulatorDb.CreateVision(_mapper.Map<VisionModeld>(NewManipulator));
            //if (saveStatus)
            //{
            //    System.Windows.MessageBox.Show("Added New Manipulator");
            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("Failed to save setting!");
            //}
        }

        private async void AddNewManipulator(object obj)
        {

            bool saveStatus = await _manipulatorDb.CreateManipulator(_mapper.Map<ManipulatorModel>(NewManipulator));
            if (saveStatus)
            {
                System.Windows.MessageBox.Show("Added New Manipulator");
            }
            else
            {
                System.Windows.MessageBox.Show("Failed to save setting!");
            }

            GetAllMachine();

        }

        public SettingViewModel()
        {
        }

        private async void GetVisions()
        {
            IEnumerable<VisionModel> models = await _visionDb.GetVisions();
            
            Visions = new ObservableCollection<VisionViewModel>(models.Select(x=> _mapper.Map<VisionViewModel>(x)));
        }

        private async void LoadAllCalibFile()
        {
            var i = await _calibrationDb.GetCalibrations();

            CalibSol = new ObservableCollection<CalibrationModel>(i);
        }


        private async void Operation(object parameter)
        {
            var _tcpClientInfo = _serverService.GetConnectedClient().First().Value;
            try
            {
                await _visionService.ImportSol($"{Calib.Vision.Filepath}");
                _visionService.RunProcedure("Long", true);
                ConnectServer();

                Point VisCenter = await _visionService.GetVisCenter();
                double[] OperationData = VisionGuided.EyeInHandConfig2D_Operate(VisCenter, new double[] { (double)Calib.CXOffset, (double)Calib.CYOffset, (double)Calib.CRZOffset, (double)Calib.CameraXScaling });
                OperationData[2] -= 30;
                if (OperationData[2] > 180) OperationData[2] -= 360;
                else if (OperationData[2] <= 180) OperationData[2] += 360;
                await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetX(OperationData[0]).SetY(OperationData[1]).SetRZ(OperationData[2]));
                await _serverService.SendJogCommand(_tcpClientInfo, new JogCommand().SetZ(-178));
                System.Windows.MessageBox.Show("Press OK to reset machine!");
                await _serverService.ServerWriteDataAsync("RESET", _tcpClientInfo.TcpClient.GetStream());
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Exception: {ex.Message} | Aborting the calibration process!");
                await _serverService.ServerWriteDataAsync("RESET", _tcpClientInfo.TcpClient.GetStream());
                return;
            }
        }

        private async void ConnectServer()
        {
            await _clientService.ConnectServer();
        }
        private async void GetAllMachine()
        {
            IEnumerable<ManipulatorModel> models = await _manipulatorDb.GetAllManipulator();
            IEnumerable<ManipulatorViewModel> viewModels = models.Select(x => _mapper.Map<ManipulatorViewModel>(x));
            Manipulators = new ObservableCollection<ManipulatorViewModel>(viewModels);
        }

        private async void SaveSetting(object obj)
        {



            bool saveStatus = await _manipulatorDb.SaveManipulator(_mapper.Map<ManipulatorModel>(Manipulator));
            if (saveStatus)
            {
                System.Windows.MessageBox.Show(ConfigurationManager.AppSettings["SaveSettingCommand_SaveMessage"]);
            }
            else
            {
                System.Windows.MessageBox.Show("Failed to save setting!");
            }

            GetAllMachine();

        }

        private void OnManipulatorChangeEvent(object obj)
        {

            Manipulator = ((ManipulatorViewModel)obj).Clone() as ManipulatorViewModel;

        }
        private void OnVisionChangeEvent(object obj)
        {

            Vision = ((VisionViewModel)obj).Clone() as VisionViewModel;

        }


        public IEnumerable GetErrors(string propertyName)
        {
            return _errorViewModel.GetErrors(propertyName);
        }

    }



}



