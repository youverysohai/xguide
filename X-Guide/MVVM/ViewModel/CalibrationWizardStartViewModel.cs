using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;
using X_Guide.VisionMaster;
using Xlent_Vision_Guided;

namespace X_Guide.MVVM.ViewModel
{
    internal class CalibrationWizardStartViewModel : ViewModelBase
    {
        private IMapper _mapper;
        private IVisionService _visionService;
        private readonly INavigationService _navigationService;
        private readonly IViewModelLocator _viewModelLocator;
        private readonly ICalibrationDb _calibrationDb;
        private string _name;
        private ObservableCollection<CalibrationViewModel> _calibrationList;

        public ObservableCollection<CalibrationViewModel> CalibrationList
        {
            get { return _calibrationList; }
            set { _calibrationList = value; OnPropertyChanged(); }
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
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
        private IManipulatorDb _manipulatorDB { get; }

        public CalibrationWizardStartViewModel(IServerService serverService, IClientService clientService, IViewModelLocator viewModelLocator, NavigationStore navigationStore, IVisionService visionService, ICalibrationDb calibrationDb, IMapper mapper)
        {
            StartCommand = new RelayCommand(start);
            _serverService = serverService;
            _visionService = visionService;
            _navigationService = new NavigationService(navigationStore);
            _viewModelLocator = viewModelLocator;
            _calibrationDb = calibrationDb;
            _mapper = mapper;
            LoadAllCalibration();
         
        }

        private async void LoadAllCalibration()
        {
            var i = await _calibrationDb.GetAllCalibration();
            CalibrationList = new ObservableCollection<CalibrationViewModel>(i.Select(x=> _mapper.Map<CalibrationViewModel>(x)));

        }

        private void start(object arg)
        {
            /*            IsStarted = true;
                        _navigationStore.CurrentViewModel = new EngineeringViewModel(_machineDB, _mapper, _name, _serverService, _clientService);*/
            /*         FindXMoveYMove(30, 15);*/

            _navigationService.Navigate(_viewModelLocator.CreateCalibrationMainViewModel(Name));
        }

        private async void FindXMoveYMove(int jogDistance, int rotateAngle)
        {

            Point Vis_Center = await _visionService.GetVisCenter();

            TcpClientInfo tcpClientInfo = _serverService.GetConnectedClient().First().Value;
            await _serverService.SendJogCommand(tcpClientInfo, new JogCommand().SetX(jogDistance));
            await Task.Delay(2000);
            Point Vis_Positive = await _visionService.GetVisCenter();
            await Task.Delay(1000);
            await _serverService.SendJogCommand(tcpClientInfo, new JogCommand().SetX(-jogDistance));
            await _serverService.SendJogCommand(tcpClientInfo, new JogCommand().SetRZ(rotateAngle));
            await Task.Delay(2000);
            Point Vis_Rotate = await _visionService.GetVisCenter();
            await Task.Delay(1000);
            await _serverService.SendJogCommand(tcpClientInfo, new JogCommand().SetRZ(-rotateAngle));
            double[] MoveOffset = VisionGuided.FindEyeInHandXYMoves(Vis_Center, Vis_Positive, Vis_Rotate, jogDistance, rotateAngle);
            await Task.Delay(1000);
            await _serverService.SendJogCommand(tcpClientInfo, new JogCommand().SetX(MoveOffset[0]).SetY(MoveOffset[1]));


        }
        private List<string> _itemList;

        public List<string> ItemList
        {
            get { return _itemList; }
            set { _itemList = value; OnPropertyChanged(); }
        }
        public void DeleteCalibration(CalibrationViewModel calibration)
        {
            CalibrationList.Remove(calibration);
        }
    }
}
