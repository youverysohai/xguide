using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using X_Guide.MVVM.View.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    public class EngineeringViewModel : ViewModelBase
    {
        // Define the data list for the StepBar control
        public ObservableCollection<DataItem> DataList { get; set; } = new ObservableCollection<DataItem>
        {
            
            new DataItem {  Content = "Manipulator"      , Icon = "RobotIndustrialOutline"       },
            new DataItem {  Content = "Camera"           , Icon = "CameraOutline"          },
            new DataItem {  Content = "Vision Flow"      , Icon = "ViewListOutline"       },
            new DataItem {  Content = "Robot Motion"     , Icon = "MotionOutline"        } ,
            new DataItem {  Content = "Jog Robot"        , Icon = "GamepadOutline"       } ,
            new DataItem {  Content = "Start Calibration", Icon = "CheckOutline"  } ,
        };                                                                

        public EngineeringViewModel() {


        }


    }
    public class DataItem
    {
    
        public string Content { get; set; }
        public string Icon { get; set; }
    }

}
