using System;
using System.Collections.Generic;
using System.Configuration;
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
            string robotIP = string.Join(".", settingViewModel.RobotIP);
            string visionIP = string.Join(".", settingViewModel.VisionIP);
            var setting = new Setting(settingViewModel.MachineID, settingViewModel.MachineDescription, settingViewModel.SoftwareRevision, robotIP,
                settingViewModel.RobotPort, settingViewModel.ShiftStartTime, visionIP,
                settingViewModel.VisionPort, settingViewModel.MaxScannerCapTime, settingViewModel.LogFilePath);
 
            
            setting.WriteToXML(ConfigurationManager.AppSettings["SettingPath"]);

        }



    }
}
