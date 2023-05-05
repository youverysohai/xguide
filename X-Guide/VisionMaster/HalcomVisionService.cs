using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VM.Core;
using VMControls.Interface;
using HalconDotNet;
using System.Threading;

namespace X_Guide.VisionMaster
{
    internal class HalcomVisionService : IVisionService
    {
        HTuple row, col;
        public HalcomVisionService()
        {

        }
        public void ConnectServer()
        {
            throw new NotImplementedException();
        }

        public List<VmProcedure> GetAllProcedures()
        {
            throw new NotImplementedException();
        }

        public VmModule GetCameras()
        {
            throw new NotImplementedException();
        }

        public List<VmModule> GetModules(VmProcedure vmProcedure)
        {
            throw new NotImplementedException();
        }

        public VmProcedure GetProcedure(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Point> GetVisCenter()
        {
            Task<Point> result = null;


            new Point
            {
                X = 1,
                Y = 0,
                Angle = 0,
                IsFound = false,
            };
            return result;
        }

        public Task ImportSol(string filepath)
        {
            throw new NotImplementedException();
        }

        public void RunOnceAndSaveImage()
        {
            throw new NotImplementedException();
        }

        public Task<IVmModule> RunProcedure(string name, bool continuous = false)
        {
            throw new NotImplementedException();
        }

        public void snap()
        {
            HTuple acqHandle = new HTuple();
            HTuple hv_AcqHandle = new HTuple();
            HObject hImage = new HObject();
            HObject hRegion = new HObject();
            HObject hSelectedRegion = new HObject();
            HTuple row = new HTuple();
            HTuple col = new HTuple();
            HTuple area = new HTuple();
            HOperatorSet.OpenFramegrabber("USB3Vision", 0, 0, 0, 0, 0, 0, "progressive",
    -1, "default", -1, "false", "default", "0E7015_ToshibaTeli_BU130",
    0, -1, out hv_AcqHandle);

            HOperatorSet.GrabImageStart(hv_AcqHandle, -1);

            if (hv_AcqHandle != null)
            {

                Thread devicethread = new Thread(() =>
                {


                    HOperatorSet.GrabImageAsync(out hImage, hv_AcqHandle, -1);

                    HOperatorSet.CloseFramegrabber(hv_AcqHandle);
                    HOperatorSet.Threshold(hImage, out hRegion, 0, 236);
                    HOperatorSet.SelectShape(hRegion, out hSelectedRegion, "area", "and", 15000, 99999999999);
                    HOperatorSet.AreaCenter(hSelectedRegion, out area, out row, out col);


                    Console.WriteLine("ROW " + (double)row);
                    Console.WriteLine("  ");
                    Console.WriteLine("COL " + (double)col);


                    acqHandle.Dispose();
                    hv_AcqHandle.Dispose();
                    hImage.Dispose();
                    hRegion.Dispose();

                });
                devicethread.Start();
            }
        }
    }
}