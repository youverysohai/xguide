using System;
using System.Linq;
using System.Threading.Tasks;
using VisionGuided;
using X_Guide.Communication.Service;
using X_Guide.VisionMaster;
using XGuideSQLiteDB;
using XGuideSQLiteDB.Models;

namespace X_Guide.Service.Communication
{
    internal class SmartCamOperationService : OperationService, IOperationService
    {
        public SmartCamOperationService(IRepository repository, IVisionService visionService, IServerService serverService) : base(repository, visionService, serverService)
        {
        }

        public async Task<object> Operation(string[] parameter)
        {
            double[] OperationData;
            Calibration calib;
            Point VisCenter;
            string procedure = "";

            try
            {
                if (parameter.Length < 1) throw new Exception(StrRetriver.Get("OP000"));
                calib = _repository.Find<Calibration>(q => q.Name.Equals(parameter[1])).FirstOrDefault() ?? throw new Exception(StrRetriver.Get("OP001"));

                VisCenter = await _visionService.GetVisCenter();
                if (VisCenter is null) throw new Exception(StrRetriver.Get("VI000"));
                if (VisCenter.X == 0.0 && VisCenter.Y == 0.0 && VisCenter.Angle == 0.0) throw new Exception(StrRetriver.Get("VI000"));

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