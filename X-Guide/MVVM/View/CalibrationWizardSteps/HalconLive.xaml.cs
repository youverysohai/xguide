using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for HalconLive.xaml
    /// </summary>
    public partial class HalconLive : UserControl
    {
        private bool isLive = false;
        public HalconLive()
        {
            InitializeComponent();
        }

        private void Snap_Click(object sender, RoutedEventArgs e)
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
            isLive = true;
            if (hv_AcqHandle != null)
            {

                Thread devicethread = new Thread(() =>
                {
                    for (int i = 0; i < 200; i++)
                    {
                        HOperatorSet.GrabImageAsync(out hImage, hv_AcqHandle, -1);

                        HOperatorSet.DispObj(hImage, HalconWindow.HalconWindow);
                        if (!isLive)
                        {
                            break;
                        }

                    }
                    HOperatorSet.CloseFramegrabber(hv_AcqHandle);

                });
                devicethread.Start();

            }

        }

        private void CloseLive_Click(object sender, RoutedEventArgs e)
        {
            isLive = false;
        }
    }
}
