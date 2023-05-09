using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VM.Core;
using VMControls.Interface;
using X_Guide.Service;

namespace X_Guide.VisionMaster
{
    internal class HalcomVisionService : IVisionService, IDisposable
    {
        private readonly HTuple acqHandle = new HTuple();
        public HTuple hv_AcqHandle = new HTuple();
        private HObject hImage = new HObject();

        public event EventHandler<HObject> OnImageReturn;

        private readonly BackgroundService _imageGrab;

        public event EventHandler<(HObject, object)> OnOutputImageReturn;

        private readonly HObject _outputImage;

        public HalcomVisionService()
        {
            try
            {
                HOperatorSet.OpenFramegrabber("USB3Vision", 0, 0, 0, 0, 0, 0, "progressive",
    -1, "default", -1, "false", "default", "0E7015_ToshibaTeli_BU130",
    0, -1, out hv_AcqHandle);

                HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
                _imageGrab = new BackgroundService(ImageGrab, true, 10);
                _imageGrab.Start();
            }
            catch
            {
            }
        }

        public void ConnectServer()
        {
        }

        private void ImageGrab()
        {
            HOperatorSet.GrabImageAsync(out hImage, hv_AcqHandle, -1);
            OnImageReturn?.Invoke(this, hImage);
        }

        public List<VmProcedure> GetAllProcedures()
        {
            return null;
        }

        public VmModule GetCameras()
        {
            return null;
        }

        public List<VmModule> GetModules(VmProcedure vmProcedure)
        {
            return null;
        }

        public VmProcedure GetProcedure(string name)
        {
            return null;
        }

        public async Task<Point> GetVisCenter()
        {
            Point result = new Point();

            if (hv_AcqHandle is null) return result;
            await Task.Run(() =>
            {
                FindCenterPoint(result, hImage.Clone());
            });

            return result;
        }

        private void FindCenterPoint(Point point, HObject image)
        {
            HObject hRegion = new HObject();
            HObject hSelectedRegion = new HObject();
            HTuple row = new HTuple();
            HTuple col = new HTuple();
            HTuple area = new HTuple();

            HOperatorSet.Threshold(image, out hRegion, 0, 236);
            HOperatorSet.SelectShape(hRegion, out hSelectedRegion, "area", "and", 15000, 99999999999);
            HOperatorSet.AreaCenter(hSelectedRegion, out area, out row, out col);

            point.X = (double)row;
            point.Y = (double)col;
            OnOutputImageReturn?.Invoke(this, (image, point));
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
        }

        public void Dispose()
        {
            acqHandle.Dispose();
            hv_AcqHandle.Dispose();
            hImage.Dispose();
        }
    }
}