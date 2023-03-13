using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Store;
using X_Guide.Service.Communation;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    internal class CalibrationWizardStartViewModel :ViewModelBase
    {
        private IMapper _mapper;

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private bool _isStarted;
        private readonly ServerCommand _serverCommand;

        public bool IsStarted
        {
            get { return _isStarted; }
            set { _isStarted = value; OnPropertyChanged(); }
        }

        public ICommand StartCommand { get; set; }
        public NavigationStore _navigationStore { get; }
        private  IMachineService _machineDB { get; }

        public CalibrationWizardStartViewModel(NavigationStore navigationStore, IMachineService machineService, IMapper mapper, ServerCommand serverCommand) { 
            StartCommand = new RelayCommand(start);
            _navigationStore = navigationStore;
            _machineDB = machineService;
            _serverCommand = serverCommand;
        }
        private void start(object arg)
        {
            IsStarted = true;
            _navigationStore.CurrentViewModel = new EngineeringViewModel(_machineDB, _mapper, _name, _serverCommand);
        }
    
    }
}
