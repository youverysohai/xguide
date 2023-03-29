
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Drawing.Imaging;
using VM.Core;
using VM.PlatformSDKCS;
using System.Diagnostics;
using X_Guide.MVVM.ViewModel;
using System.Windows.Threading;
using VMControls.WPF.Release;
using System.Reflection;
using IMVSCircleFindModuCs;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for Step5.xaml
    /// </summary>
    /// 

    public partial class Step5 : UserControl
    {
        private VmProcedure _p;
        public Step5()
        {
            InitializeComponent();
            Loaded += OnLoaded;

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _p = (DataContext as Step5ViewModel).p;
            if (_p != null) LoadModuleSource(this, _p);
            else
            {
                (DataContext as Step5ViewModel).VmImportCompleted += LoadModuleSource;
            }


        }

        private async void LoadModuleSource(object sender, VmProcedure p)
        {
     /*       await Task.Run(() => Dispatcher.InvokeAsync(() => p_box.ModuleSource = p, DispatcherPriority.Background));
            loadingCircle.Visibility = Visibility.Collapsed;
            CenterBox.Visibility = Visibility.Visible;
        }



        private void p_box_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void p_box_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
  
       
          /*  Type x = p_box.ModuleSource.Outputs[4].GetType();
            PropertyInfo[] properties = x.GetProperties();
            foreach(PropertyInfo property in properties)
            {
                Debug.WriteLine(property.Name + ":  " + property.GetType());
            }*/

           

    
            string tempFilePath = Path.ChangeExtension(Path.GetTempFileName(), "jpeg");
       /*     p_box.SaveRenderedImage(tempFilePath);*/
            var converter = new ByteArrayToImageConverter();
            var bArray = converter.ConvertBack(null, null, tempFilePath, null);
            List<BitmapImage> bitmapImages = new List<BitmapImage>();

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(tempFilePath);
            bitmap.EndInit();
            bitmapImages.Add(bitmap);

            test.Source = bitmap;
        }
    }


}
