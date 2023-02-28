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



            string robotIP = $"{_settingViewModel.RobotIPS1}.{_settingViewModel.RobotIPS2}.{_settingViewModel.RobotIPS3}.{_settingViewModel.RobotIPS4}";
            string visionIP = $"{_settingViewModel.VisionIP[0]}.{_settingViewModel.VisionIP[1]}.{_settingViewModel.VisionIP[2]}.{_settingViewModel.VisionIP[3]}";

            var machine = new MachineModel(_settingViewModel.Machine.Id, _settingViewModel.MachineName, (int)Enum.Parse(typeof(MachineType), _settingViewModel.MachineType), _settingViewModel.MachineDescription, robotIP, _settingViewModel.RobotPort, visionIP, _settingViewModel.VisionPort, _settingViewModel.ManipulatorTerminator, _settingViewModel.VisionTerminator);

            _machineDB.SaveMachine(machine);
            _settingViewModel.UpdateManipulatorNameList(_settingViewModel.MachineName);
            /*setting.WriteToXML(ConfigurationManager.AppSettings["SettingPath"]);*/

            MessageBox.Show(ConfigurationManager.AppSettings["SaveSettingCommand_SaveMessage"]);
        }



    }
}
