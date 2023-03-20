using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Store;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;
using Xlent_Vision_Guided;

namespace X_Guide.MVVM.ViewModel
{
    internal class CalibrationWizardStartViewModel :ViewModelBase
    {
        private IMapper _mapper;
        private IClientService _clientService;
        private VisionGuided _visAlgorithm;
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private bool _isStarted;
        private readonly IServerService _serverService;

        public bool IsStarted
        {
            get { return _isStarted; }
            set { _isStarted = value; OnPropertyChanged(); }
        }

        public ICommand StartCommand { get; set; }
        public NavigationStore _navigationStore { get; }
        private  IMachineService _machineDB { get; }

        public CalibrationWizardStartViewModel(NavigationStore navigationStore, IMachineService machineService, IMapper mapper, IServerService serverService, IClientService clientService, VisionGuided visAlgorithm) { 
            StartCommand = new RelayCommand(start);
            _navigationStore = navigationStore;
            _machineDB = machineService;
            _serverService = serverService;
            _mapper = mapper;
            _clientService = clientService;
            _visAlgorithm = visAlgorithm;
        }
        private async void start(object arg)
        {
            IsStarted = true;
            _navigationStore.CurrentViewModel = new EngineeringViewModel(_machineDB, _mapper, _name, _serverService);
/*            Point Vis_Center = await _clientService.GetVisCenter();
            Point Vis_Positive = new Point();
            _visAlgorithm.FindEyeInHandXYMoves(Vis_Center, Vis_Positive);*/
        }
    
    }
}
