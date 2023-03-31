using AutoMapper;
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
    public class SettingViewModel : ViewModelBase, INotifyDataErrorInfo
    {

        public RelayCommand SaveCommand { get; }
        public RelayCommand ManipulatorCommand { get; set; }
        public RelayCommand VisionCommand { get; set; }


        private readonly IMachineDbService _machineDb;
        private readonly IVisionDbService _visionDb;
        private readonly IMapper _mapper;
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

        

        public VisionViewModel Vision
        {
            get { return _vision; }
            set { _vision = value; 
                OnPropertyChanged(); }
        }
        private ObservableCollection<VisionViewModel> _visions;

        public ObservableCollection<VisionViewModel> Visions
        {
            get { return _visions; }
            set { _visions = value;
                OnPropertyChanged();
            }
        }


        private MachineViewModel _manipulator;

        public MachineViewModel Manipulator
        {
            get { return _manipulator; }
            set
            {
                _manipulator = value;
                OnPropertyChanged();
            }
        }

        //SettingViewModel properties
        private ObservableCollection<MachineViewModel> _manipulators;

        public ObservableCollection<MachineViewModel> Manipulators
        {
            get { return _manipulators; }
            set
            {
                _manipulators = value;
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

        public SettingViewModel(IMachineDbService machineDb, IVisionDbService visionDb, IMapper mapper)
        {
            _machineDb = machineDb;
            _visionDb = visionDb;
            _mapper = mapper;
            GetAllMachine();
            Vision = new VisionViewModel
            {
                Name = "Chun",
                Ip = new ObservableCollection<string>(new string[]{"192", "168","11","90"}),
                Port=8000,
                Terminator="NA"
                
            };
            Visions = new ObservableCollection<VisionViewModel> { Vision , new VisionViewModel
            {
                Name = "Hik",
                Ip = new ObservableCollection<string>(new string[]{"192", "163","11","33"}),
                Port=8000,
                Terminator="NA"

            } , new VisionViewModel
            {
                Name = "Canon",
                Ip = new ObservableCollection<string>(new string[]{"192", "128","21","90"}),
                Port=8000,
                Terminator="NA"

            }  };
            Server = new GeneralSettingViewModel
            {
                Ip = new ObservableCollection<string>(new string[] { "192", "168", "11", "90" }),
                Port = 8000,
                Terminator = "NA"

            };


            MachineTypeList = EnumHelperClass.GetAllIntAndDescriptions(typeof(MachineType));
            TerminatorList = EnumHelperClass.GetAllValuesAndDescriptions(typeof(Terminator));
            VisionCommand = new RelayCommand(OnVisionChangeEvent);
            ManipulatorCommand = new RelayCommand(OnManipulatorChangeEvent);
            SaveCommand = new RelayCommand(SaveSetting);
        }

        private async void GetAllMachine()
        {
            IEnumerable<MachineModel> models = await _machineDb.GetAllMachine();
            IEnumerable<MachineViewModel> viewModels = models.Select(x => _mapper.Map<MachineViewModel>(x));
            Manipulators = new ObservableCollection<MachineViewModel>(viewModels);
        }

        private async void SaveSetting(object obj)
        {



            bool saveStatus = await _machineDb.SaveMachine(_mapper.Map<MachineModel>(Manipulator));
            if (saveStatus)
            {
                MessageBox.Show(ConfigurationManager.AppSettings["SaveSettingCommand_SaveMessage"]);
            }
            else
            {
                MessageBox.Show("Failed to save setting!");
            }
            GetAllMachine();

        }

        private void OnManipulatorChangeEvent(object obj)
        {

            Manipulator = ((MachineViewModel)obj).Clone() as MachineViewModel;
   
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



