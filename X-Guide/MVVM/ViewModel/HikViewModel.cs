using HikVisionProvider;
using System;
using System.Runtime.Versioning;
using System.Windows;
using VisionProvider.Interfaces;
using VMControls.Interface;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    internal class HikViewModel : ViewModelBase, IVisionViewModel, IDisposable
    {
        private readonly HikVisionService _visionService;
        private CalibrationViewModel _calibrationConfig { get; set; }
        public IVmModule Module { get; set; }
        public Visibility Visible { get; set; } = Visibility.Collapsed;

        public HikViewModel(IVisionService visionService)
        {
            _visionService = (HikVisionService)visionService;
        }

        public void SetConfig(CalibrationViewModel calibrationConfig)
        {
            _calibrationConfig = calibrationConfig;
        }

        public void StartLiveImage()
        {
            Module = _visionService.RunProcedure(_calibrationConfig?.Procedure, true);
            Visible = Visibility.Visible;
        }

        public void ShowOutputImage()
        {
            _visionService.Procedure = _calibrationConfig.Procedure;
            _visionService.RunProcedure(_calibrationConfig.Procedure);
            IVmModule procedure = _visionService.GetProcedure(_calibrationConfig.Procedure);
            Module = procedure;
            //Modules = _visionService.GetModules(procedure);
        }

        public void StopLiveImage()
        {
            throw new NotImplementedException();
        }
    }
}