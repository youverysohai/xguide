using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MVVM.Command
{
    internal class SaveSettingCommand : CommandBase
    {
        private readonly SettingViewModel settingViewModel;
        private readonly Setting setting;

        public override bool CanExecute(object parameter)
        {
            return true; 
        }
        public SaveSettingCommand(SettingViewModel settingViewModel, Setting setting)
        {
            this.settingViewModel = settingViewModel;
            this.setting = setting;
        }

        public override void Execute(object parameter)
        {
            var setting = new Setting(settingViewModel.MachineID, settingViewModel.MachineDescription, settingViewModel.SoftwareRevision, settingViewModel.RobotIP, settingViewModel.RobotPort, settingViewModel.ShiftStartTime, settingViewModel.VisionIP, settingViewModel.VisionPort, settingViewModel.MaxScannerCapTime, settingViewModel.LogFilePath);
            setting.WriteToXML(Directory.GetCurrentDirectory() + @"\Setting.xml");

        }



    }
}
