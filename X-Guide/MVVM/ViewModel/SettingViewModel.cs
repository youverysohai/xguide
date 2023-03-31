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


        private readonly IManipulatorDb _manipulatorDb;
        private readonly IVisionDb _visionDb;
        private readonly IMapper _mapper;
        private readonly ErrorViewModel _errorViewModel;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => false;

        private bool _canEdit;

        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                _canEdit = value;
                OnPropertyChanged();
            }
        }
        private ManipulatorViewModel _cache;

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

        public SettingViewModel(IManipulatorDb machineDb, IVisionDb visionDb, IMapper mapper)
        {
            _manipulatorDb = machineDb;
            _visionDb = visionDb;
            _mapper = mapper;
            GetAllMachine();

            ManipulatorTypeList = EnumHelperClass.GetAllIntAndDescriptions(typeof(ManipulatorType));
            TerminatorList = EnumHelperClass.GetAllValuesAndDescriptions(typeof(Terminator));

            ManipulatorCommand = new RelayCommand(OnManipulatorChangeEvent);
            SaveCommand = new RelayCommand(SaveSetting);
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

            Manipulator = ((ManipulatorViewModel)obj).Clone() as ManipulatorViewModel;
   
        }


        public IEnumerable GetErrors(string propertyName)
        {
            return _errorViewModel.GetErrors(propertyName);
        }

    }



}



