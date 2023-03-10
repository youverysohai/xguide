using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
using VMControls.WPF.Release.Front;
using WpfAnimatedGif;
using XamlAnimatedGif;

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
            // loadImage();
            Uri uri = new Uri(@"Style/ImageSource/hamster-cute.gif",UriKind.Relative);
            AnimationBehavior.SetSourceUri(img,uri);


        }



    }
}
