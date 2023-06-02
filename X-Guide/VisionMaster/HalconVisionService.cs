using HalconDotNet;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VM.Core;
using VMControls.Interface;
using X_Guide.Service;

namespace X_Guide.VisionMaster
{
    internal class HalconVisionService : IVisionService, IDisposable
    {
        private readonly HTuple acqHandle = new HTuple();
        public HTuple hv_AcqHandle = new HTuple();
        private HObject hImage = new HObject();
        public bool isLoading = false;

        public event EventHandler<HObject> OnImageReturn;

        private readonly IEventAggregator _eventAggregator;
        private readonly BackgroundService _imageGrab;

        public event EventHandler<(HObject, object)> OnOutputImageReturn;

        private readonly HObject _outputImage;

        public HalconVisionService(IEventAggregator eventAggregator, IDisposeService disposeService)
        {
            disposeService.Add(this);
            _eventAggregator = eventAggregator;
            _imageGrab = new BackgroundService(ImageGrab, true, 10);
        }

        public async void StopGrabbingImage()
        {
            _imageGrab.Stop();
            await Task.Delay(1000);
            HOperatorSet.CloseFramegrabber(hv_AcqHandle);
        }

        public void StartGrabbingImage()
        {
            try
            {
                HOperatorSet.OpenFramegrabber("USB3Vision", 0, 0, 0, 0, 0, 0, "progressive",
    -1, "default", -1, "false", "default", "0E7015_ToshibaTeli_BU130",
    0, -1, out hv_AcqHandle);

                HOperatorSet.GrabImageStart(hv_AcqHandle, -1);

                _imageGrab.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ConnectServer()
        {
        }

        private void ImageGrab()
        {
            try
            {
                HOperatorSet.GrabImageAsync(out hImage, hv_AcqHandle, -1);
                _eventAggregator.Publish(hImage);
            }
            catch
            {
                MessageBox.Show("This is nothing");
            }
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
            if (hv_AcqHandle is null) throw new Exception("hv_AcqHandle is null");

            Point point = await Task.Run(() => FindCenterPoint(hImage.Clone()));
            return point;
        }

        private Point FindCenterPoint(HObject image)
        {
            Point point = new Point();
            HObject hRegion, hConnectedRegion, hSelectedRegion, hSelectedRegion1;

            HOperatorSet.Threshold(image, out hRegion, 136, 255);
            HOperatorSet.Connection(hRegion, out hConnectedRegion);
            HOperatorSet.SelectShape(hConnectedRegion, out hSelectedRegion, "circularity", "and", 0.65, 1);
            HOperatorSet.SelectShape(hSelectedRegion, out hSelectedRegion1, "area", "and", 1000, 6500);
            HOperatorSet.AreaCenter(hSelectedRegion1, out HTuple area, out HTuple row, out HTuple col);

            point.X = row.D;
            point.Y = col.D;
            OnOutputImageReturn?.Invoke(this, (image, point));
            return point;
        }

        public Task ImportSolAsync(string filepath)
        {
            throw new NotImplementedException();
        }

        public Task<IVmModule> RunProcedure(string name, bool continuous = false)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            HOperatorSet.CloseFramegrabber(hv_AcqHandle);
            acqHandle.Dispose();
            hv_AcqHandle.Dispose();
            hImage.Dispose();
            MessageBox.Show("I have closed everything :D");
        }

        Task<List<VmProcedure>> IVisionService.GetAllProcedures()
        {
            throw new NotImplementedException();
        }
    }
}