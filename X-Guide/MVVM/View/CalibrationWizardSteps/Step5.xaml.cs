
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
using System.Windows.Shapes;
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

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for Step5.xaml
    /// </summary>
    /// 

    public partial class Step5 : UserControl
    {

        VmRenderControl vmControl;
        Step5ViewModel dataContext;

        public Step5()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            /*           VmRenderControl vmControl = (DataContext as Step5ViewModel).VmRenderControl;*/
            dataContext = (DataContext as Step5ViewModel);
           /* vmControl = dataContext.VmRenderControl;
            container.Children.Add(vmControl);
            Grid.SetRowSpan(vmControl, 3);
            Grid.SetColumnSpan(vmControl, 3);*/
            /*            bool IsLoaded = VmSolution.Instance._importPath != null ? true : false;*/
            bool IsLoaded = false;
     
            if (IsLoaded) /*LoadModuleSource()*/;
            else
            {
                (DataContext as Step5ViewModel).VmImportCompleted -= LoadModuleSource;
                (DataContext as Step5ViewModel).VmImportCompleted += LoadModuleSource;
            }


        }

       
        private async void LoadModuleSource(object sender, VmProcedure p)
        {
            await Task.Run(() => Dispatcher.InvokeAsync(() => p_box.ModuleSource = p));
            loadingCircle.Visibility = Visibility.Collapsed;
            CenterBox.Visibility = Visibility.Visible;
            Mouse.OverrideCursor = null;
            var i = p_box.ImageSource;

        }



        private void p_box_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*
                  p_box.LoadFrontendSource();

                  p_box.AutoChangeSize();*/
        }

        private void p_box_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }


}
