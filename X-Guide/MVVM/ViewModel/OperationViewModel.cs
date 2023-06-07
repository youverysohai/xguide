using CommunityToolkit.Mvvm.Messaging;
using System.Threading.Tasks;
using VM.Core;
using X_Guide.Service.Communication;
using X_Guide.VisionMaster;

namespace X_Guide.MVVM.ViewModel
{
    internal class OperationViewModel : ViewModelBase, IRecipient<VmProcedure>
    {
        private readonly IMessenger _messenger;
        private readonly IServerCommand _serverCommand;
        private readonly IVisionService _visionService;
        public HikViewModel VisionView { get; set; }
        private VmProcedure procedure = null;

        public OperationViewModel(IServerCommand serverCommand, IVisionService visionService, IVisionViewModel viewModel, IMessenger messenger)
        {
            _messenger = messenger;
            _serverCommand = serverCommand;
            _visionService = visionService;
            VisionView = viewModel as HikViewModel;
            messenger.Register<VmProcedure>(this);
        }

        public override void Dispose()
        {
            _messenger.Unregister<VmProcedure>(this);
            base.Dispose();
        }

        public async void Receive(VmProcedure message)
        {
            if (procedure == message) return;
            procedure = message;
            await Task.Delay(1000);
            VisionView.Module = message;
        }
    }
}