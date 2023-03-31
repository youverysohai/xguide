using AutoMapper;
using HandyControl.Controls;
using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step1ViewModel : ViewModelBase
    {
        #region properties

        public Action SelectedItemChangedEvent;

        private ManipulatorModel _machineModel;

        private ManipulatorViewModel _machine;
        public ManipulatorViewModel Machine
        {
            get { return _machine; }
            set { _machine = value;
                OnPropertyChanged();
            }
        }
        private string _selectedItem;

        public string SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value;
                OnPropertyChanged();
                OnSelectedItemChanged(value);
            }
        }

        private CalibrationViewModel _setting;

 

        private ObservableCollection<string> _machineNames;
        public ObservableCollection<string> MachineNames
        {
            get { return _machineNames; }
            set { _machineNames = value;
                OnPropertyChanged();
            }
        }

        private async void OnSelectedItemChanged(string name)
        {
            _machineModel = await _manipulatorDbService.GetManipulator(name);
            Machine = /*MachineViewModel.ToViewModel(_machineModel, _mapper);*/ null;
            _setting.Manipulator = Machine;
            _serverService.SetServerReadTerminator(_manipulatorDbService.GetManipulatorDelimiter(name));
            SelectedItemChangedEvent?.Invoke();
             
        }

        public ICommand ShoutCommand { get; set; }
        public IManipulatorDbService _manipulatorDbService { get; }
        private IMapper _mapper { get; }
        public IServerService _serverService { get; }

        private readonly IClientService _clientService;
        #endregion
        public Step1ViewModel(IManipulatorDbService manipulatorDbService, IMapper mapper, CalibrationViewModel setting, IServerService serverService, IClientService clientService)
        {
            

             _manipulatorDbService = manipulatorDbService;
            LoadMachineName();
            _mapper = mapper; 
           _serverService = serverService;
            _clientService = clientService;
            _setting = setting;
            
        }

        private async void LoadMachineName()
        {

            MachineNames = new ObservableCollection<string>(await _manipulatorDbService.GetAllManipulatorName());
  
        }

    }
}
