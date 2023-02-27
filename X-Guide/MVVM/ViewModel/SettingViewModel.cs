﻿using System;
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
using X_Guide.Service.DatabaseProvider;
using X_Guide.Validation;

namespace X_Guide.MVVM.ViewModel
{
    internal class SettingViewModel : ViewModelBase, INotifyDataErrorInfo
    {

        public SaveSettingCommand SaveCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand ConnectServerCommand { get; set; }



        private IMachineService _machineDB;

        private readonly ErrorViewModel _errorViewModel;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private List<MachineModel> _machines;
        private MachineModel _machine;
        public  MachineModel Machine => _machine;


        //SettingViewModel properties
        private List<string> _machineNameList;

        public List<string> MachineNameList
        {
            get { return _machineNameList; }
            set
            {
                _machineNameList = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<ValueDescription> _machineTypeList;

        public IEnumerable<ValueDescription> MachineTypeList
        {
            get { return _machineTypeList; }
            set
            {
                _machineTypeList = value;
                OnPropertyChanged();
            }
        }

        private string _machineName;
        public string MachineName
        {
            get { return _machineName; }
            set
            {

                _machineName = value;
                /*if (string.IsNullOrEmpty(value))
                {
                    _errorViewModel.AddError("Please enter a valid value for this field. This field cannot be left blank.");
                }
                else
                {
                    _errorViewModel.RemoveError("Please enter a valid value for this field. This field cannot be left blank.");
                }

                if (value.Length > 30)
                {
                    _errorViewModel.AddError("The field must not exceed 30 characters");
                }
                else
                {
                    _errorViewModel.RemoveError("The field must not exceed 30 characters");
                }*/
                OnPropertyChanged();
            }
        }


        private string _machineDescription;
        public string MachineDescription
        {
            get { return _machineDescription; }
            set
            {

                _machineDescription = value;
                if (string.IsNullOrEmpty(value))
                {
                    _errorViewModel.AddError("Please enter a valid value for this field. This field cannot be left blank.");
                }
                else
                {
                    _errorViewModel.RemoveError("Please enter a valid value for this field. This field cannot be left blank.");
                }
                OnPropertyChanged(nameof(MachineDescription));
            }
        }

        private string _machineType;
        public string MachineType
        {
            get { return _machineType; }
            set
            {
                _machineType = value;
                OnPropertyChanged(nameof(MachineType));
            }
        }


        private string _robotIPS1;
        public string RobotIPS1
        {
            get { return _robotIPS1; }
            set
            {

                _robotIPS1 = value;
                if (!IPValidation.ValidateIPSegment(value)) _errorViewModel.AddError("Please enter a valid value for this field.");
                else _errorViewModel.RemoveError("Please enter a valid value for this field.");
                OnPropertyChanged();
            }
        }
        private string _robotIPS2;
        public string RobotIPS2
        {
            get { return _robotIPS2; }
            set
            {

                _robotIPS2 = value;
                if (!IPValidation.ValidateIPSegment(value)) _errorViewModel.AddError("Please enter a valid value for this field.");
                else _errorViewModel.RemoveError("Please enter a valid value for this field.");
                OnPropertyChanged();
            }
        }
        private string _robotIPS3;
        public string RobotIPS3
        {
            get { return _robotIPS3; }
            set
            {

                _robotIPS3 = value;
                if (!IPValidation.ValidateIPSegment(value)) _errorViewModel.AddError("Please enter a valid value for this field.");
                else _errorViewModel.RemoveError("Please enter a valid value for this field.");
                OnPropertyChanged();
            }
        }
        private string _robotIPS4;
        public string RobotIPS4
        {
            get { return _robotIPS4; }
            set
            {

                _robotIPS4 = value;
                if (!IPValidation.ValidateIPSegment(value)) _errorViewModel.AddError("Please enter a valid value for this field.");
                else _errorViewModel.RemoveError("Please enter a valid value for this field.");
                OnPropertyChanged();
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

        private string _manipulatorTerminator;

        public string ManipulatorTerminator
        {
            get { return _manipulatorTerminator; }
            set
            {
                _manipulatorTerminator = value;
                OnPropertyChanged();
            }
        }

        private string _visionTerminator;

        public string VisionTerminator
        {
            get { return _visionTerminator; }
            set
            {
                _visionTerminator = value;
                OnPropertyChanged();
            }
        }


        public SettingViewModel(IMachineService machineDB)
        {
            PropertyChanged += ChangeEventHandler;
            _errorViewModel = new ErrorViewModel();

            _machineDB = machineDB;

            _machines = _machineDB.GetAllMachine().ToList();
            MachineNameList = _machines.Select(r => r.Name).ToList();

            MachineTypeList = EnumHelperClass.GetAllValuesAndDescriptions(typeof(MachineType));
            TerminatorList = EnumHelperClass.GetAllValuesAndDescriptions(typeof(Terminator));
            MachineName = "Kibo";


            SaveCommand = new SaveSettingCommand(this, _machineDB);


            var command = (CommandBase)SaveCommand;



            _errorViewModel.ErrorsChanged += OnErrorChanged;

        }



        private void ChangeEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MachineName))
            {
                _machines = _machineDB.GetAllMachine().ToList();
                _machine = _machines.First(r => { return r.Name == MachineName; });
                UpdateSettingUI(_machine);
            }
        }

        public bool HasErrors => _errorViewModel.HasErrors;
        private void OnErrorChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            (SaveCommand as CommandBase)?.OnCanExecutedChanged();

        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorViewModel.GetErrors(propertyName);
        }

        public void UpdateSettingUI(MachineModel machine)
        {

            MachineDescription = machine.Description;





            MachineType = Enum.GetName(typeof(MachineType), machine.Type);
            VisionTerminator = machine.VisionTerminator;
            ManipulatorTerminator = machine.ManipulatorTerminator;
            var manipulatorIP = machine.ManipulatorIP.Split('.');
            RobotIPS1 = manipulatorIP[0];
            RobotIPS2 = manipulatorIP[1];
            RobotIPS3 = manipulatorIP[2];
            RobotIPS4 = manipulatorIP[3];

            RobotPort = machine.ManipulatorPort;

            VisionIP = machine.VisionIP.Split('.');
            VisionPort = machine.VisionPort;
        }


    }



}



