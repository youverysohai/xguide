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
using X_Guide.Service.DatabaseProvider;
using X_Guide.Validation;

namespace X_Guide.MVVM.ViewModel
{
    internal class SettingViewModel : ViewModelBase, INotifyDataErrorInfo
    {

        public SaveSettingCommand SaveCommand { get; }
        public ICommand NavigateCommand { get; }

        public ICommand EditManipulatorNameCommand { get; set; }
        public ICommand ConnectServerCommand { get; set; }



        private IMachineService _machineDB;
        public MachineModel _machine;

        private readonly ErrorViewModel _errorViewModel;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private Visibility _cancelBtnVisibility;

        public Visibility CancelBtnVisibility
        {
            get { return _cancelBtnVisibility; }
            set { _cancelBtnVisibility = value;
                OnPropertyChanged();
            }
        }


        private Visibility _saveBtnVisibility;

        public Visibility SaveBtnVisibility
        {
            get { return _saveBtnVisibility; }
            set { _saveBtnVisibility = value;
                OnPropertyChanged();
            }
        }



        private Visibility _editBtnVisibility;

        public Visibility EditBtnVisibility
        {
            get { return _editBtnVisibility; }
            set { _editBtnVisibility = value;
                OnPropertyChanged();
            }
        }


        private bool _canEdit;

        public bool CanEdit
        {
            get { return _canEdit; }
            set { _canEdit = value;
                OnPropertyChanged();
            }
        }

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
                }
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



        public SettingViewModel(IMachineService machineDB)
        {


            _machineDB = machineDB;

            _machine = _machineDB.GetMachine("Limpeh"); //debug
            SaveCommand = new SaveSettingCommand(this, _machineDB);
            EditManipulatorNameCommand = new EditManipulatorNameCommand(this);

            _errorViewModel = new ErrorViewModel();
            var command = (CommandBase)SaveCommand;
            

            _errorViewModel.ErrorsChanged += OnErrorChanged;
            UpdateSettingUI();
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

        public void UpdateSettingUI()
        {
            MachineID = _machine.Name;
            MachineDescription = _machine.Description;
            SoftwareRevision = _machine.Type.ToString();

            var manipulatorIP = _machine.ManipulatorIP.Split('.');
            RobotIPS1 = manipulatorIP[0];
            RobotIPS2 = manipulatorIP[1];
            RobotIPS3 = manipulatorIP[2];
            RobotIPS4 = manipulatorIP[3];
            RobotPort = _machine.ManipulatorPort;
     
            VisionIP = _machine.VisionIP.Split('.');
            VisionPort = _machine.VisionPort;

            SaveBtnVisibility = Visibility.Collapsed;
            CancelBtnVisibility = Visibility.Collapsed;
        }


    }




    /* public void CreateUser()
        {
            _userProvider.CreateUser(new UserModel("Zhen Chun", "ongzc-pm19@student.tarc.edu.my", "123"));
        }
        public async void TestingAsync()
        {
            var i = await _userProvider.GetAllUsersAsync();
            foreach (var item in i)
            {
                MessageBox.Show(item.Email + " " + item.PasswordHash + " " + item.Username);
            }
        }*/


}



