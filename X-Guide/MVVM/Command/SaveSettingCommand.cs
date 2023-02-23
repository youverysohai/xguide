using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel;
using X_Guide.Service.UserProviders;

namespace X_Guide.MVVM.Command
{
    internal class SaveSettingCommand : CommandBase
    {
        private readonly SettingViewModel settingViewModel;

        public override bool CanExecute(object parameter)
        {
            return !settingViewModel.HasErrors;
        }
        public SaveSettingCommand(SettingViewModel settingViewModel)
        {
            this.settingViewModel = settingViewModel;
  
        }

       
        public override void Execute(object parameter)
        {
            


            string robotIP = string.Join(".", settingViewModel.RobotIPS1, settingViewModel.RobotIPS2, settingViewModel.RobotIPS3, settingViewModel.RobotIPS4);
            MessageBox.Show(robotIP);
            string visionIP = string.Join(".", settingViewModel.VisionIP);
            var setting = new Setting(settingViewModel.MachineID, settingViewModel.MachineDescription, settingViewModel.SoftwareRevision, robotIP,
                settingViewModel.RobotPort, settingViewModel.ShiftStartTime, visionIP,
                settingViewModel.VisionPort, settingViewModel.MaxScannerCapTime, settingViewModel.LogFilePath);
            setting.WriteToXML(ConfigurationManager.AppSettings["SettingPath"]);
            MessageBox.Show("Setting saved! Please restart the application for the setting to take effect.");
        }
       


    }
}
