using System.Threading.Tasks;
using VM.Core;
using VMControls.Interface;
using X_Guide.Service.Communication;
using X_Guide.VisionMaster;
using static X_Guide.Service.Communication.HikOperationService;

namespace X_Guide.MVVM.ViewModel
{
    internal class OperationViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IServerCommand _serverCommand;
        private readonly IVisionService _visionService;
        public HikViewModel VisionView { get; set; }

        public OperationViewModel(IServerCommand serverCommand, IVisionService visionService, IVisionViewModel viewModel, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _serverCommand = serverCommand;
            _visionService = visionService;
            VisionView = viewModel as HikViewModel;
            _eventAggregator.Subscribe<Procedure>(DisplayOutputImage);
        }

        private void DisplayOutputImage(Procedure e)
        {
            VisionView.Module = e.procedure;
        }

        public override void Dispose()
        {
            _eventAggregator.Unsubscribe<Procedure>(DisplayOutputImage);
            base.Dispose();
        }
    }
}