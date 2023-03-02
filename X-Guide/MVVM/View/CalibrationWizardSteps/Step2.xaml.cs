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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for Step2.xaml
    /// </summary>
    public partial class Step2 : UserControl
    {


        public Step2()
        {
            InitializeComponent();
            loadImage();



        }

        public async void loadImage()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(@"C:\Users\Admin\source\repos\X-Guide\X-Guide\Style\ImageSource\hamster-cute.gif");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(img, image);
            await Task.Run(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    ImageBehavior.SetAnimatedSource(img, image);
                }));
            });

        }

    }
}
