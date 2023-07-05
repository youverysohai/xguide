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

namespace X_Vision.CustomUserControls
{
    /// <summary>
    /// Interaction logic for ProcessTab.xaml
    /// </summary>
    public partial class ProcessTab : UserControl
    {
        public ProcessTab()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int failCount =0, passCount=0;
            int totalCount = int.Parse(TotalTB.Text);
            totalCount++;
            Random random = new Random();
            int randomNumber = random.Next(0,5);
            passCount = int.Parse(PassCountTB.RateCount.ToString());
            failCount = int.Parse(FailCountTB.RateCount.ToString());
            if (randomNumber < 1)
            {
                 failCount++;
            }
            else
            {
               passCount++;

            }
            FailCountTB.RateCount = failCount;
            PassCountTB.RateCount = passCount;
            TotalTB.Text = totalCount.ToString();
        }
    }
}
