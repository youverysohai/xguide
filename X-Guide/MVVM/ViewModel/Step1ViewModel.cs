using AutoMapper;
using HandyControl.Controls;
using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using X_Guide.MVVM.Model;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step1ViewModel : ViewModelBase
    {

        public event EventHandler OnSelectedItemChangedEvent;

        private MachineModel _machineModel;

        private MachineViewModel _machine;
        public MachineViewModel Machine
        {
            get { return _machine; }
            set { _machine = value;
                OnPropertyChanged();
            }
        }
        private string _selectedItem;

        public string SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value;
                OnPropertyChanged();
                OnSelectedItemChanged(value);
            }
        }

        private ObservableCollection<string> _machineNames;
        public ObservableCollection<string> MachineNames
        {
            get { return _machineNames; }
            set { _machineNames = value;
            }
        }

        private void OnSelectedItemChanged(string name)
        {
            _machineModel = _machineService.GetMachine(name);
            Machine = MachineViewModel.ToViewModel(_machineModel, _mapper);
            OnSelectedItemChangedEvent?.Invoke(this, EventArgs.Empty);
        }

        public ICommand ShoutCommand { get; set; }
        public IMachineService _machineService { get; }
        private IMapper _mapper { get; }

        public Step1ViewModel(IMachineService machineService, IMapper mapper)
        {
            _machineService = machineService;
            _mapper = mapper;
            ShoutCommand = new RelayCommand(Test);
            MachineNames = new ObservableCollection<string>(_machineService.GetAllMachineName());
        }
        public void Test(object obj)
        {
            OnSelectedItemChangedEvent?.Invoke(this, EventArgs.Empty);
        }
        public override ViewModelBase GetNextViewModel()
        {
            return new Step2ViewModel(Machine,  ref OnSelectedItemChangedEvent);
        }
    }
}
