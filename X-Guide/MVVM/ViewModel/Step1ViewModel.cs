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
using X_Guide.Communication.Service;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step1ViewModel : ViewModelBase
    {
        #region properties

        public Action SelectedItemChangedEvent;

        private ManipulatorViewModel _selectedItem;

        public ManipulatorViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value;
                OnPropertyChanged();
                OnSelectedItemChanged(value);
            }
        }

        private CalibrationViewModel _setting;

 

        private ObservableCollection<ManipulatorViewModel> _manipulators;
        public ObservableCollection<ManipulatorViewModel> Manipulators
        {
            get { return _manipulators; }
            set { _manipulators = value;
                OnPropertyChanged();
            }
        }

        private void OnSelectedItemChanged(ManipulatorViewModel manipulator)
        {


            _setting.Manipulator = manipulator;
            SelectedItemChangedEvent?.Invoke();
             
        }

        public ICommand ShoutCommand { get; set; }
        public IManipulatorDb _manipulatorDb { get; }
        private IMapper _mapper { get; }
        public IServerService _serverService { get; }

        private readonly IClientService _clientService;
        #endregion
        public Step1ViewModel(IManipulatorDb manipulatorDb, IMapper mapper, CalibrationViewModel setting, IServerService serverService, IClientService clientService)
        {
            

             _manipulatorDb = manipulatorDb;
            LoadMachineName();
            _mapper = mapper; 
           _serverService = serverService;
            _clientService = clientService;
            _setting = setting;
            
        }

        private async void LoadMachineName()
        {
            var models = await _manipulatorDb.GetAllManipulator();
            var viewModels = models.Select(x => _mapper.Map<ManipulatorViewModel>(x));
            Manipulators = new ObservableCollection<ManipulatorViewModel>(viewModels);
  
        }

    }
}
