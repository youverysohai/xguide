using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step1ViewModel : ViewModelBase
    {

        private MachineModel _machine;

        public MachineModel Machine
        {
            get { return _machine; }
            set { _machine = value; }
        }

        private ObservableCollection<string> _machineNames;

        public ObservableCollection<string> MachineNames
        {
            get { return _machineNames; }
            set { _machineNames = value; }
        }

        private string _name;
                                                      
        public string Name
        {
            get { return _name; }
            set { _name = value;
                OnNameChanged(_name);
                OnPropertyChanged();
            }
        }


        private string _type = "Unknown";

        public string Type
        {
            get { return _type; }
            set { _type = value;
            OnPropertyChanged();}
        }


        private void OnNameChanged(string name)
        {
            _machine = _machineService.GetMachine(name);
            Type = Enum.GetName(typeof(MachineType), _machine.Type);
        }

        public IMachineService _machineService { get; }

        public Step1ViewModel(IMachineService machineService)
        {
            _machineService = machineService;
            //_machine= _machineService.GetMachine();
            MachineNames = new ObservableCollection<string>(_machineService.GetAllMachineName());
        }

        public override ViewModelBase GetNextViewModel()
        {
            return new Step2ViewModel(_machine);
        }
    }
}
