using System;
using System.Threading.Tasks;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Model;
using X_Guide.Service.DatabaseProvider;
using X_Guide.VisionMaster;
using Xlent_Vision_Guided;

namespace X_Guide.Service.Communication
{
    internal class SmartCamOperationService : OperationService, IOperationService
    {
        public SmartCamOperationService(ICalibrationDb calibrationDb, IVisionService visionService, IServerService serverService) : base(calibrationDb, visionService, serverService)
        {
        }

        public async Task<object> Operation(string[] parameter)
        {
            double[] OperationData;
            CalibrationModel calib;
            Point VisCenter;
            string procedure = "";

            try
            {
                if (parameter.Length < 1) throw new Exception(StrRetriver.Get("OP000"));
                calib = await _calibrationDb.Get(parameter[1]) ?? throw new Exception(StrRetriver.Get("OP001"));

                VisCenter = await _visionService.GetVisCenter();
                if (VisCenter is null) throw new Exception(StrRetriver.Get("VI000"));
                if (VisCenter.X == 0.0 && VisCenter.Y == 0.0 && VisCenter.Angle == 0.0) throw new Exception(StrRetriver.Get("VI000"));
                OperationData = VisionGuided.EyeInHandConfig2D_Operate(VisCenter, new double[] { calib.CalibratedRzOffset, calib.CalibratedYOffset, calib.CalibratedXOffset, calib.CameraXScaling });
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