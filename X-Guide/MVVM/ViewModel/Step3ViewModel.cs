using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using VM.Core;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
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

        private ManualResetEvent _manual;
        private CalibrationViewModel _calibration;
        private readonly IVisionService _visionService;
        private readonly IVisionDb _visionDb;
        private readonly IMapper _mapper;

        public CalibrationViewModel Calibration
        {
            get { return _calibration; }
        }

        private string _procedure => _calibration.Procedure;

        public string Procedure
        {
            get { return _procedure; }
            set
            {
                _calibration.Procedure = value;
                OnPropertyChanged();
                OnProcedureChanged();
            }
        }

        private VisionViewModel _vision => _calibration.Vision;
        public VisionViewModel Vision
        {
            get { return _vision; }
            set
            {
                _calibration.Vision = value;
                OnPropertyChanged();
                if (value != null)
                {
                    /*     GetProcedures();*/
                }
            }
        }

        private void OnProcedureChanged()
        {
            MessageBox.Show(_calibration.Procedure);
        }

        public ObservableCollection<VisionViewModel> Visions { get; set; }

        public ObservableCollection<string> Procedures { get; set; }

        public override void ReadyToDisplay()
        {
            using (_manual = new ManualResetEvent(false))
            {
                _manual.WaitOne();
            }

        }

        public Step3ViewModel(CalibrationViewModel calibration, IVisionService visionService, IVisionDb visionDb, IMapper mapper)
        {
            _calibration = calibration;
            _visionService = visionService;
            _visionDb = visionDb;
            _mapper = mapper;
            InitView();

        }


        private async void InitView()
        {
            await GetVisions();
            await GetProcedures();
            _manual?.Set();

        }
        private async Task GetVisions()
        {
            IEnumerable<VisionModel> models = await _visionDb.GetAll();
            Visions = new ObservableCollection<VisionViewModel>(models.Select(x => _mapper.Map<VisionViewModel>(x)));

        }

        private async Task GetProcedures()
        {
            if (_calibration.Vision?.Filepath is null) return;
            await _visionService.ImportSol(_calibration.Vision.Filepath);
            Procedures = new ObservableCollection<string>(_visionService.GetProcedureNames());
        }



    }
}
    