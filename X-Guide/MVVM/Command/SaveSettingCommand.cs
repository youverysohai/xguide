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
        private IUserService _userProvider;

        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public SaveSettingCommand(SettingViewModel settingViewModel, IUserService userProvider)
        {
            this.settingViewModel = settingViewModel;
            _userProvider = userProvider;
        }

        public override void Execute(object parameter)
        {
            _userProvider.CreateUser(new UserModel("Zhen Chun", "ongzc-pm19@student.tarc.edu.my", "123"));
            
            TestingAsync();
            /*  string robotIP = string.Join(".", settingViewModel.RobotIPS1, settingViewModel.RobotIPS2, settingViewModel.RobotIPS3, settingViewModel.RobotIPS4);
              MessageBox.Show(robotIP);
              string visionIP = string.Join(".", settingViewModel.VisionIP);
              var setting = new Setting(settingViewModel.MachineID, settingViewModel.MachineDescription, settingViewModel.SoftwareRevision, robotIP,
                  settingViewModel.RobotPort, settingViewModel.ShiftStartTime, visionIP,
                  settingViewModel.VisionPort, settingViewModel.MaxScannerCapTime, settingViewModel.LogFilePath);

               _userProvider.CreateUser(new UserModel("Zhen Chun", "ongzc-pm19@student.tarc.edu.my", "123"));
              setting.WriteToXML(ConfigurationManager.AppSettings["SettingPath"]);
              MessageBox.Show("Setting saved! Please restart the application for the setting to take effect.");*/
        }
        public async void TestingAsync()
        {
            var i = await _userProvider.GetAllUsersAsync();
            foreach (var item in i)
            {
                MessageBox.Show(item.Email + " " + item.PasswordHash + " " + item.Username);
            }
        }


    }
}
