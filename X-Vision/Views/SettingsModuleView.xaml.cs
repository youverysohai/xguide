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
using X_Vision.Common.Models;

namespace X_Vision.Views
{
    /// <summary>
    /// Interaction logic for SettingsModuleView.xaml
    /// </summary>
    public partial class SettingsModuleView : UserControl
    {

        private readonly static MenuBar[] menuBars = new MenuBar[]
{
            new MenuBar() { Icon = "HomeOutline", Title = "Personalize", NameSpace = "SkinView" },
            new MenuBar() { Icon = "CameraOutline", Title = "Settings", NameSpace = "" }             ,
            new MenuBar() { Icon = "Connection", Title = "About", NameSpace = "About" },
            };
        public SettingsModuleView()
        {
            InitializeComponent();

            menuBar.ItemsSource = menuBars;
        }
    }
}
