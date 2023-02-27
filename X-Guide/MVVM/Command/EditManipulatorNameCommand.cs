using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MVVM.Command
{
    internal class EditManipulatorNameCommand : CommandBase
    {
        public SettingViewModel settingVMProperties;
        //Constructor
        public EditManipulatorNameCommand(SettingViewModel settingViewModel) {
            settingVMProperties = settingViewModel;


        }
        public override void Execute(object parameter)
        {
            if(settingVMProperties.CanEdit)
            {
                settingVMProperties.CanEdit = false;
                settingVMProperties.EditBtnVisibility = System.Windows.Visibility.Visible;
                settingVMProperties.SaveBtnVisibility = System.Windows.Visibility.Collapsed;
                settingVMProperties.CancelBtnVisibility = System.Windows.Visibility.Collapsed;
                
            }
            else
            {
                settingVMProperties.CanEdit = true;
                settingVMProperties.EditBtnVisibility = System.Windows.Visibility.Collapsed;
                settingVMProperties.SaveBtnVisibility = System.Windows.Visibility.Visible;
                settingVMProperties.CancelBtnVisibility = System.Windows.Visibility.Visible;


            }

        }
    }
}
