using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;
using X_Guide.VisionMaster;
using Xlent_Vision_Guided;
using Timer = System.Timers.Timer;

namespace X_Guide.Service.Communation
{
    public class ServerCommand : IDisposable
    {
        public Queue<string> commandQeueue = new Queue<string>();
        private readonly IServerService _serverService;
        private readonly IClientService _clientService;
        private readonly IVisionService _visionService;
        private readonly ICalibrationDb _calibDb;

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
                if (parameter.Length < 3) throw new Exception(StrRetriver.Get("OP000"));
                calib = await _calibDb.Get(parameter[1]) ?? throw new Exception(StrRetriver.Get("OP001"));
                try { await _visionService.ImportSol(String.Format(@"{0}", calib.Vision.Filepath)); }
                catch { throw new Exception(StrRetriver.Get("OP002")); };
                _ = await _visionService.RunProcedure($"{parameter[2]}", true) ?? throw new Exception(StrRetriver.Get("OP003"));
                VisCenter = await _visionService.GetVisCenter();
                OperationData = VisionGuided.EyeInHandConfig2D_Operate(VisCenter, new double[] { calib.CXOffset, calib.CYOffset, calib.CRZOffset, calib.CameraXScaling });
                string Mode = calib.Mode ? "GLOBAL" : "TOOL";
                await _serverService.ServerWriteDataAsync($"XGUIDE,{calib.Mode},{OperationData[0]},{OperationData[1]},{OperationData[2]}");
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
