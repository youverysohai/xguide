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
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.Command
{
    internal class SaveSettingCommand : CommandBase
    {
        private readonly SettingViewModel _settingViewModel;
        private readonly IMachineService _machineDB;

        public override bool CanExecute(object parameter)
        {
            return !_settingViewModel.HasErrors;
        }
        public SaveSettingCommand(SettingViewModel settingViewModel, IMachineService machineDB)
        {
            _settingViewModel = settingViewModel;
            _machineDB = machineDB;
        }

       
        public override void Execute(object parameter)
        {

        

            string robotIP = string.Join(".", _settingViewModel.RobotIPS1, _settingViewModel.RobotIPS2, _settingViewModel.RobotIPS3, _settingViewModel.RobotIPS4);
            string visionIP = string.Join(".", _settingViewModel.VisionIP);
            var machine = new MachineModel(_settingViewModel.Machine.Id, _settingViewModel.MachineName,(int)Enum.Parse(typeof(MachineType),_settingViewModel.MachineType), _settingViewModel.MachineDescription, robotIP, _settingViewModel.RobotPort, visionIP, _settingViewModel.VisionPort, _settingViewModel.ManipulatorTerminator, _settingViewModel.VisionTerminator);
            _machineDB.SaveMachine(machine);
            _settingViewModel.UpdateComboBox(_settingViewModel.MachineName);
            
            /*setting.WriteToXML(ConfigurationManager.AppSettings["SettingPath"]);*/

            MessageBox.Show("Setting saved! Please restart the application for the setting to take effect.");
        }
       


    }
}
