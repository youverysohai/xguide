using System;
using System.Windows;
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
        public Visibility Visible { get; set; } = Visibility.Collapsed;

        public HikViewModel(IVisionService visionService)
        {
            _visionService = (HikVisionService)visionService;
        }

        public void Dispose()
        {
        }

        public void SetConfig(CalibrationViewModel calibrationConfig)
        {
            _calibrationConfig = calibrationConfig;
        }

        private void BindModuleToRenderControl(IVmModule module)
        {
            Module = module;
        }

        public async void StartLiveImage()
        {
            Module = await _visionService.RunProcedure(_calibrationConfig.Procedure, true);
            Visible = Visibility.Visible;
        }

        public async void ShowOutputImage()
        {
            _visionService.Procedure = _calibrationConfig.Procedure;
            await _visionService.RunProcedure(_calibrationConfig.Procedure);
            IVmModule procedure = _visionService.GetProcedure(_calibrationConfig.Procedure);
            Module = procedure;
            //Modules = _visionService.GetModules(procedure);
        }
    }
}