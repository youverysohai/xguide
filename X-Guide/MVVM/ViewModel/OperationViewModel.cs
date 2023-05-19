using System;
using VM.Core;
using X_Guide.Service.Communation;
using X_Guide.Service.Communication;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class OperationViewModel : ViewModelBase, IDisposable
    {
        private readonly IServerCommand _serverCommand;
        private readonly IVisionService _visionService;
        public HikViewModel VisionView { get; set; }

        public OperationViewModel(ServerCommand serverCommand, IVisionService visionService, IVisionViewModel viewModel)
        {
            _serverCommand = serverCommand;
            _visionService = visionService;
            VisionView = (HikViewModel)viewModel;
            _serverCommand.SubscribeOnOperationEvent(DisplayOutputImage);
        }

        private void DisplayOutputImage(object sender, object e)
        {
            string data = e as string;
            VmModule procedure = _visionService.GetProcedure(data);
            VisionView.Module = procedure;
            /* VisionView.Modules = _visionService.GetModules(procedure);*/
        }

        public override void Dispose()
        {
            _serverCommand.UnsubscribeOnOperationEvent(DisplayOutputImage);
            base.Dispose();
        }
    }
}