using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.Store;
using X_Guide.Service;

namespace X_Guide.MVVM.ViewModel
{
    internal class SettingViewModel : ViewModelBase, INotifyDataErrorInfo
    {
       
        public ICommand SaveCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand ConnectServerCommand { get; set; }



        public Setting setting;

        private readonly ErrorViewModel _errorViewModel;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        //SettingViewModel properties 
        private string _machineID;
        public string MachineID
        {
            get { return _machineID; }
            set
            {
                _machineID = value;
                if (string.IsNullOrEmpty(value))
                {
                    _errorViewModel.AddError(nameof(MachineID), "MachineID cannot be empty");
                }
                else
                {
                    _errorViewModel.RemoveError(nameof(MachineID), "MachineID cannot be empty");
                }
                OnPropertyChanged(nameof(MachineID));
            }
        }





        private string _machineDescription;
        public string MachineDescription
        {
            get { return _machineDescription; }
            set
            {
                _machineDescription = value;
                OnPropertyChanged(nameof(MachineDescription));
            }
        }

        private string _softwareRevision;
        public string SoftwareRevision
        {
            get { return _softwareRevision; }
            set
            {
                _softwareRevision = value;
                OnPropertyChanged(nameof(SoftwareRevision));
            }
        }


        private string[] _robotIP;
        public string[] RobotIP
        {
            get { return _robotIP; }
            set
            {
                _robotIP = value;
                OnPropertyChanged(nameof(RobotIP));
            }
        }

        private string _robotPort;
        public string RobotPort
        {
            get { return _robotPort; }
            set
            {
                _robotPort = value;
                OnPropertyChanged(nameof(RobotPort));
            }
        }

        private string _shiftStartTime;
        public string ShiftStartTime
        {
            get { return _shiftStartTime; }
            set
            {
                _shiftStartTime = value;
                OnPropertyChanged(nameof(ShiftStartTime));
            }
        }

        private string[] _visionIP;
        public string[] VisionIP
        {
            get { return _visionIP; }
            set
            {
                _visionIP = value;
                OnPropertyChanged(nameof(VisionIP));
            }
        }

        private string _visionPort;
        public string VisionPort
        {
            get { return _visionPort; }
            set
            {
                _visionPort = value;
                OnPropertyChanged(nameof(VisionPort));
            }
        }

        private string _maxScannerCapTime;
        public string MaxScannerCapTime
        {
            get { return _maxScannerCapTime; }
            set
            {
                _maxScannerCapTime = value;
                OnPropertyChanged(nameof(MaxScannerCapTime));
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



        public SettingViewModel(Setting setting)
        {

            SaveCommand = new SaveSettingCommand(this, setting);
            ConnectServerCommand = new ConnectServerCommand("192.168.10.90", 7930);
            this.setting = setting;
            _errorViewModel = new ErrorViewModel();
            _errorViewModel.ErrorsChanged += OnErrorChanged;
            UpdateSettingUI();
        }

        public bool HasErrors => _errorViewModel.HasErrors;
        private void OnErrorChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorViewModel.GetErrors(propertyName);
        }

        public void UpdateSettingUI()
        {
            MachineID = setting.MachineID;
            MachineDescription = setting.MachineDescription;
            SoftwareRevision = setting.SoftwareRevision;

            RobotIP = setting.RobotIP.Split('.');
            RobotPort = setting.RobotPort;
            ShiftStartTime = setting.ShiftStartTime;
            VisionIP = setting.VisionIP.Split('.');
            VisionPort = setting.VisionPort;
            MaxScannerCapTime = setting.MaxScannerCapTime;
            LogFilePath = setting.LogFilePath;
        }

      
    }







}



