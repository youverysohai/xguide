using System;
using System.Collections.Generic;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.Model;
using X_Guide.Service.DatabaseProvider;
using X_Guide.VisionMaster;
using Xlent_Vision_Guided;

namespace X_Guide.Service.Communation
{
    public class ServerCommand : IDisposable
    {
        public Queue<string> commandQeueue = new Queue<string>();
        private readonly IServerService _serverService;
        private readonly IClientService _clientService;
        private readonly IVisionService _visionService;
        private readonly ICalibrationDb _calibDb;

        public event EventHandler<string> OnOperationCalled;

        public ServerCommand(IServerService serverService, IClientService clientService, IVisionService visionService, ICalibrationDb calibDb)
        {
            _serverService = serverService;
            _clientService = clientService;
            _visionService = visionService;
            _calibDb = calibDb;
            _serverService._dataReceived += ValidateSyntax;
        }

        public void ValidateSyntax(object sender, NetworkStreamEventArgs network)
        {
            string[] data = network.Data;

            switch (data[0].Trim().ToLower())
            {
                case "xguide": Operation(data); break;
                default: break;
            }
        }

        private async void Operation(string[] parameter)
        {
            double[] OperationData;
            CalibrationModel calib;
            Point VisCenter;
            try
            {
                if (parameter.Length < 2) throw new Exception(StrRetriver.Get("OP000"));
                calib = await _calibDb.Get(parameter[1]) ?? throw new Exception(StrRetriver.Get("OP001"));

                string procedure = parameter[2];

                OnOperationCalled?.Invoke(this, procedure);
                ((HikVisionService)_visionService).Procedure = procedure;

                VisCenter = await _visionService.GetVisCenter();
                if (VisCenter is null) throw new Exception(StrRetriver.Get("VI000"));
                OperationData = VisionGuided.EyeInHandConfig2D_Operate(VisCenter, new double[] { calib.CXOffset, calib.CYOffset, calib.CRZOffset, calib.CameraXScaling });
                string Mode = calib.Mode ? "GLOBAL" : "TOOL";
                await _serverService.ServerWriteDataAsync($"XGUIDE,{Mode},{OperationData[0]},{OperationData[1]},{OperationData[2]}");
            }
            catch (Exception ex)
            {
                await _serverService.ServerWriteDataAsync($"XGUIDE, {ex.Message}");
                return;
            }
        }

        public void Dispose()
        {
            _serverService._dataReceived -= ValidateSyntax;
        }
    }
}