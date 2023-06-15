using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisionGuided;
using VM.Core;
using X_Guide.Communication.Service;
using X_Guide.VisionMaster;
using XGuideSQLiteDB;
using XGuideSQLiteDB.Models;

namespace X_Guide.Service.Communication
{
    internal class HikOperationService : OperationService, IOperationService
    {
        private readonly IMessenger _messenger;

        public HikOperationService(IRepository repository, IVisionService visionService, IServerService serverService, IMessenger messenger) : base(repository, visionService, serverService)
        {
            _messenger = messenger;
        }

        public struct Procedure
        {
            public VmProcedure procedure;
        }

        public async Task<object> Operation(string[] parameter)
        {
            double[] OperationData;
            Calibration calib;
            Point VisCenter;
            string procedure = "";

            try
            {
                if (parameter.Length < 2) throw new Exception(StrRetriver.Get("OP000"));
                calib = _repository.Find<Calibration>(q => q.Name.Equals(parameter[1])).FirstOrDefault() ?? throw new Exception(StrRetriver.Get("OP001"));

                procedure = parameter[2];

                ((HikVisionService)_visionService).Procedure = procedure;

                VisCenter = await _visionService.GetVisCenter();
                _messenger.Send(_visionService.GetProcedure(procedure));

                if (VisCenter is null) throw new Exception(StrRetriver.Get("VI000"));
                OperationData = VisionProcessor.EyeInHandConfig2D_Operate(VisCenter, new double[] { calib.CXOffset, calib.CYOffset, calib.CRZOffset, calib.MMPerPixel });
                string Mode = calib.Mode ? "GLOBAL" : "TOOL";
                await _serverService.ServerWriteDataAsync($"XGUIDE,{Mode},{OperationData[0]},{OperationData[1]},{OperationData[2]}");
            }
            catch (Exception ex)
            {
                await _serverService.ServerWriteDataAsync($"XGUIDE, {ex.Message}");
            }
            return procedure;
        }
    }
}