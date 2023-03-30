using HandyControl.Controls;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

namespace X_Guide.MVVM.View.CalibrationWizardSteps
{
    /// <summary>
    /// Interaction logic for CalibrationWizardStart.xaml
    /// </summary>
    public partial class CalibrationWizardStart : UserControl
    {
        private SnackbarMessageQueue messageQueue = new SnackbarMessageQueue();
        private List<string> chipsList = new List<string> { "Circle 1", "Chip 2", "Chip 3", "Chip 4", "Chip 5", "Circle 1", "Chip 2", "Chip 3", "Chip 4", "Chip 5", "Circle 1", "Chip 2", "Chip 3", "Chip 4", "Chip 5", };
        private ItemsControl itemsControl = new ItemsControl();


        public CalibrationWizardStart()
        {
            InitializeComponent();
             
            itemsControl.ItemTemplate = (DataTemplate)this.Resources["ChipTemplate"];
            
            itemsControl.ItemsSource = chipsList;

            ChipsContainer.Children.Add(itemsControl);
            SnackbarMessage.MessageQueue = messageQueue;

        }


        private void Chip_Click(object sender, RoutedEventArgs e)
        {

            Chip chip = (Chip)sender;
            messageQueue.Enqueue(chip.Content + " is Selected", null,
                null,
                null,
                false,
                true,
                TimeSpan.FromSeconds(1.55));

        }

        private void Chip_DeleteClick(object sender, RoutedEventArgs e)
        {
            Chip chip = (Chip)sender;
            messageQueue.Enqueue(chip.Content + " is Deleted", null,
                null,
                null,
                false,
                true,
                TimeSpan.FromSeconds(1.55));
        }
    }
    public class LastCharacterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            if (string.IsNullOrEmpty(strValue))
            {
                return string.Empty;
            }
            return strValue.Substring(strValue.Length - 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
