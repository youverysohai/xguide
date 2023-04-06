using AutoMapper;
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
            set { _calibration.Procedure = value;
                OnPropertyChanged();
                OnProcedureChanged();
            }
        }

        private VisionViewModel _vision => _calibration.Vision;
        public VisionViewModel Vision
        {
            get { return _vision; }
            set { _calibration.Vision = value;
                OnPropertyChanged();
                if(value != null)
                {
                    GetProcedures();
                }
            }
        }

    

        private void OnProcedureChanged()
        {
            MessageBox.Show(_calibration.Procedure);
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

        private ObservableCollection<string> _procedures;

        public ObservableCollection<string> Procedures
        {
            get { return _procedures; }
            set { _procedures = value;
                OnPropertyChanged();
            }
        }



        public Step3ViewModel(CalibrationViewModel calibration, IVisionService visionService, IVisionDb visionDb, IMapper mapper)
        {
            _calibration = calibration;
            _visionService = visionService;
            _visionDb = visionDb;
            _mapper = mapper;
            GetVisions();
       /*     if (!(Vision is null))
            {
                GetProcedures();
            }*/

        }

        private async void GetVisions()
        {
            IEnumerable<VisionModel> models = await _visionDb.GetVisions();
            Visions = new ObservableCollection<VisionViewModel>(models.Select(x=> _mapper.Map<VisionViewModel>(x)));
        }

        private async void GetProcedures()
        {
            await _visionService.ImportSol(_calibration.Vision.Filepath);
            Procedures = new ObservableCollection<string>(_visionService.GetProcedureNames());
        }
      

   
    }
}
