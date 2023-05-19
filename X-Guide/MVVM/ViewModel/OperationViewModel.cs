using HandyControl.Controls;
using System;
using System.Threading.Tasks;
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

        public OperationViewModel(IServerCommand serverCommand, IVisionService visionService, IVisionViewModel viewModel)
        {
            _serverCommand = serverCommand;
            _visionService = visionService;
            VisionView = (HikViewModel)viewModel;
            _serverCommand.SubscribeOnOperationEvent(DisplayOutputImage);
        }

        private async void DisplayOutputImage(object sender, object e)
        {
            string s = e.ToString();
            VmModule procedure = _visionService.GetProcedure(s);
            await Task.Delay(500);
            VisionView.Module = procedure;
       
        }

        public override void Dispose()
        {
            _serverCommand.UnsubscribeOnOperationEvent(DisplayOutputImage);
            base.Dispose();
        }
    }
}