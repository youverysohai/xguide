using System;
using System.Collections.Generic;
using VM.Core;
using VMControls.Interface;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class HikViewModel : ViewModelBase, IVisionViewModel, IDisposable
    {
        private readonly HikVisionService _visionService;
        private CalibrationViewModel _calibrationConfig { get; set; }
        public IVmModule Module { get; set; }
        public List<VmModule> Modules { get; set; }

        public HikViewModel(IVisionService visionService)
        {
            _visionService = (HikVisionService)visionService;
        }

        public void Dispose()
        {
        }

        public void SetConfig(CalibrationViewModel calibrationViewModel)
        {
            _calibrationConfig = calibrationViewModel;
        }

        public async void StartLiveImage()
        {
            VmProcedure procedure = await _visionService.RunProcedure("Live", true) as VmProcedure;
            Modules = _visionService.GetModules(procedure);
        }

        public void ShowOutputImage()
        {
            _visionService.Procedure = _calibrationConfig.Procedure;
            VmProcedure procedure = _visionService.GetProcedure(_calibrationConfig.Procedure);
            Modules = _visionService.GetModules(procedure);
        }
    }
}