using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using VM.Core;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step3ViewModel : ViewModelBase
    {
        public ICommand OpenFileCommand { get; }

        public ICommand NavigateCommand { get; }


        private CalibrationViewModel _setting;
        private readonly IServerService _serverService;
        private readonly IClientService _clientService;
        private readonly IVisionService _visionService;
        public event EventHandler<bool> CanExecuteChange;
        private ObservableCollection<string> _visionFlow;


        private VisionViewModel _vision;

        public VisionViewModel Vision
        {
            get { return _vision; }
            set
            {
                _vision = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<VisionViewModel> _visions;

        public ObservableCollection<VisionViewModel> Visions
        {
            get { return _visions; }
            set
            {
                _visions = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> VisionFlow
        {
            get { return _visionFlow; }
            set
            {
                _visionFlow = value;
                OnPropertyChanged();
            }
        }

        private string _selectedItem;

        public string SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value;
                OnPropertyChanged();
                OnItemChanged(value);
            }
        }

  

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged();
                OpenFile();
            }
        }

        public MainViewModel DataContext { get; private set; }

        public Step3ViewModel(CalibrationViewModel setting, IServerService serverService, IClientService clientService, IVisionService visionService)
        {
            _setting = setting;
            _serverService = serverService;
            _clientService = clientService;
            _visionService = visionService;
   
             
            Visions = new ObservableCollection<VisionViewModel> { new VisionViewModel
            {
                Name = "Vision1",
                Ip = new ObservableCollection<string>(new string[] { "192", "168", "11", "90" }),
                Port = 8000,
                Terminator = ""

            } , new VisionViewModel
            {
                Name = "Hik",
                Ip = new ObservableCollection<string>(new string[]{"192", "163","11","33"}),
                Port=8000,
                Terminator="\r"

            } , new VisionViewModel
            {
                Name = "Vision2",
                Ip = new ObservableCollection<string>(new string[]{"192", "128","21","90"}),
                Port=8000,
                Terminator="\r\n"

            }  };
            //NavigateCommand = new RelayCommand(Navigate);
        }

  

        private void OnItemChanged(string value)
        {
            _setting.VisionFlow = value;
        }
        private async void OpenFile()
        {
            try
            {
                await _visionService.ImportSol(FilePath);
/*                ProcessInfoList i = VmSolution.Instance.GetAllProcedureList();
                List<ProcessInfo> procedureList = i.astProcessInfo.Where(x => x.strProcessName != null).ToList();*/
                VisionFlow = new ObservableCollection<string>(_visionService.GetAllProcedureName());
                _setting.VisionFilePath = FilePath;
                
                CanExecuteChange?.Invoke(this, true);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

   
    }
}
