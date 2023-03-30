using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
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

        public RelayCommand SaveCommand { get; }
        public ICommand EditManipulatorNameCommand { get; set; }

        private string _name;
            
        public string Name
        {
            get { return _name; }
            set { _name = value;
                OnPropertyChanged();
                OnManipulatorChangeEvent(this, null);
            }
        }

        private readonly IMachineDbService _machineDb;
        private readonly IVisionDbService _visionDb;
        private readonly ErrorViewModel _errorViewModel;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _errorViewModel.HasErrors;

        private bool _canEdit;

        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                _canEdit = value;
                OnPropertyChanged();
            }
        } //Setting View UI properties


        private MachineModel _machine;
        public MachineModel Machine => _machine;


        //SettingViewModel properties
        private ObservableCollection<string> _machineNameList;

        public ObservableCollection<string> MachineNameList
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

        public SettingViewModel(IMachineDbService machineDb, IVisionDbService visionDb)
        {
            _machineDb = machineDb;
            _visionDb = visionDb;


            LoadMachineName();
            MachineTypeList = EnumHelperClass.GetAllValuesAndDescriptions(typeof(MachineType));
            TerminatorList = EnumHelperClass.GetAllValuesAndDescriptions(typeof(Terminator));

            SaveCommand = new RelayCommand(SaveSetting);
        }

        private void SaveSetting(object obj)
        {

        /*    string robotIP = $"{_settingViewModel.RobotIPS1}.{_settingViewModel.RobotIPS2}.{_settingViewModel.RobotIPS3}.{_settingViewModel.RobotIPS4}";
            string visionIP = $"{_settingViewModel.VisionIP[0]}.{_settingViewModel.VisionIP[1]}.{_settingViewModel.VisionIP[2]}.{_settingViewModel.VisionIP[3]}";

            var machine = new MachineModel(_settingViewModel.Machine.Id, _settingViewModel.MachineName, (int)Enum.Parse(typeof(MachineType), _settingViewModel.MachineType), _settingViewModel.MachineDescription, robotIP, _settingViewModel.RobotPort, visionIP, _settingViewModel.VisionPort, _settingViewModel.ManipulatorTerminator, _settingViewModel.VisionTerminator);

            _machineDB.SaveMachine(machine);
            _settingViewModel.UpdateManipulatorNameList(_settingViewModel.MachineName);
            *//*setting.WriteToXML(ConfigurationManager.AppSettings["SettingPath"]);*//*

            MessageBox.Show(ConfigurationManager.AppSettings["SaveSettingCommand_SaveMessage"]);*/
        }

        private async void LoadMachineName()
        {
            MachineNameList = new ObservableCollection<string>(await _machineDb.GetAllMachineName());
        }

        private void OnCommandEvent(object sender, TcpClientEventArgs e)
        {
            MessageBox.Show($"{e.TcpClient.Client.AddressFamily} : Client triggered a command!");
        }

        private void OnManipulatorChangeEvent(object sender, PropertyChangedEventArgs e)
        {
                UpdateMachine();
                UpdateMachineUI(_machine); 
        }

        public async void UpdateMachine()
        {
            
        }
      

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorViewModel.GetErrors(propertyName);
        }

        public void UpdateMachineUI(MachineModel machine)
        {

            /*MachineDescription = machine.Description;
            MachineType = Enum.GetName(typeof(MachineType), machine.Type);
            ManipulatorTerminator = machine.Terminator;
            var manipulatorIP = machine.Ip.Split('.');
            RobotIPS1 = manipulatorIP[0];
            RobotIPS2 = manipulatorIP[1];
            RobotIPS3 = manipulatorIP[2];
            RobotIPS4 = manipulatorIP[3];
            RobotPort = machine.Port;*/
            
        }

        public void UpdateManipulatorNameList(string name)
        {
            MachineNameList[MachineNameList.IndexOf(_machine.Name)] = name;
      /*      MachineName = name;*/
        }


    }



}



