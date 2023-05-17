using System;
using VM.Core;
using X_Guide.Service.Communation;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class OperationViewModel : ViewModelBase, IDisposable
    {
        private readonly ServerCommand _serverCommand;
        private readonly IVisionService _visionService;

        public HikViewModel VisionView { get; set; }

        public OperationViewModel(ServerCommand serverCommand, IVisionService visionService, IVisionViewModel viewModel)
        {
            _serverCommand = serverCommand;
            _visionService = visionService;
            VisionView = (HikViewModel)viewModel;
            _serverCommand.OnOperationCalled += _serverCommand_OnOperationCalled;
        }

        private void _serverCommand_OnOperationCalled(object sender, string e)
        {
            VmModule procedure = _visionService.GetProcedure(e);
            VisionView.Module = procedure;
            /* VisionView.Modules = _visionService.GetModules(procedure);*/
        }
    }
}