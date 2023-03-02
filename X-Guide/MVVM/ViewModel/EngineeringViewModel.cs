using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.View.CalibrationWizardSteps;
using X_Guide.Service;

namespace X_Guide.MVVM.ViewModel
{
    public class EngineeringViewModel : ViewModelBase
    {
        

        public LinkedList<ViewModelBase> _navigationHistory = new LinkedList<ViewModelBase>();
        public LinkedList<ViewModelBase> NavigationHistory => _navigationHistory;

        // Define the data list for the StepBar control
        public NavigationStore _navigationStore;
        public ICommand WizNextCommand { get; set; }
        public ICommand WizPrevCommand { get; set; }
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public LinkedListNode<ViewModelBase> CurrentNode { get; set; } 

        public ObservableCollection<DataItem> DataList { get; set; } = new ObservableCollection<DataItem>
        {
            
            new DataItem {  Content = "Manipulator"      , Icon = "RobotIndustrialOutline"       },
            new DataItem {  Content = "Camera"           , Icon = "CameraOutline"          },
            new DataItem {  Content = "Vision Flow"      , Icon = "ViewListOutline"       },
            new DataItem {  Content = "Robot Motion"     , Icon = "MotionOutline"        } ,
            new DataItem {  Content = "Jog Robot"        , Icon = "GamepadOutline"       } ,
            new DataItem {  Content = "Start Calibration", Icon = "CheckOutline"  } ,
        };                                                                

        public EngineeringViewModel(NavigationStore navigationStore) {

         
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            WizNextCommand = new WizNextCommand(this, _navigationStore);
            WizPrevCommand = new WizPrevCommand(this, _navigationStore);

            _navigationStore.CurrentViewModel = new Step1ViewModel();
            CurrentNode = _navigationHistory.AddLast(CurrentViewModel);
            

                
        }

        

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

    }
    public class DataItem
    {
    
        public string Content { get; set; }
        public string Icon { get; set; }
    }

}
