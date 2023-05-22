using AutoMapper;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using VM.Core;
using X_Guide.Aspect;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    public class SettingViewModel : ViewModelBase
    {
        public RelayCommand AddManipulatorCommand { get; set; }

        public RelayCommand TestCommand { get; }
        public RelayCommand SaveManipulatorCommand { get; }
        public RelayCommand OpenFormCommand { get; }
        public RelayCommand DeleteManipulatorCommand { get; }
        public RelayCommand ManipulatorCommand { get; set; }
        public RelayCommand OpenVisionFormCommand { get; }
        public RelayCommand OpenManiFormCommand { get; }

        public RelayCommand AddVisionCommand { get; }
        public RelayCommand SaveVisionCommand { get; }
        public RelayCommand DeleteVisionCommand { get; }
        public RelayCommand VisionCommand { get; set; }
        public RelayCommand OperationCommand { get; set; }
        public RelayCommand SaveGeneralCommand { get; set; }

        private readonly IManipulatorDb _manipulatorDb;
        private readonly IMapper _mapper;
        private readonly IJsonDb _jsonDb;

        public bool Test { get; set; } = false;
        public bool HasErrors => false;

        public GeneralViewModel General { get; set; }
        public HikVisionViewModel HikVision { get; set; }
        public ManipulatorViewModel Manipulator { get; set; }
        public ObservableCollection<ManipulatorViewModel> Manipulators { get; } = new ObservableCollection<ManipulatorViewModel>();

        public ObservableCollection<HikVisionViewModel> Visions { get; } = new ObservableCollection<HikVisionViewModel>();

        public string LogFilePath { get; set; }

        public SettingViewModel(IManipulatorDb machineDb, IMapper mapper, IJsonDb jsonDb)
        {
            _manipulatorDb = machineDb;
            _mapper = mapper;
            _jsonDb = jsonDb;
            General = mapper.Map<GeneralViewModel>(_jsonDb.Get<GeneralModel>());
            HikVision = mapper.Map<HikVisionViewModel>(_jsonDb.Get<HikVisionModel>());
            GetManipulators();

            OpenVisionFormCommand = new RelayCommand(OpenVisionForm);
            OpenManiFormCommand = new RelayCommand(OpenManiForm);
            VisionCommand = new RelayCommand(OnVisionChangeEvent);
            ManipulatorCommand = new RelayCommand(OnManipulatorChangeEvent);
            SaveManipulatorCommand = new RelayCommand(SaveManipulator);
            OpenFormCommand = new RelayCommand(OpenVisionForm);
            DeleteManipulatorCommand = new RelayCommand(DeleteManipulator);
            SaveVisionCommand = new RelayCommand(SaveVision);
            SaveGeneralCommand = new RelayCommand(SaveGeneral);
            AddManipulatorCommand = new RelayCommand(AddManipulator);
            TestCommand = new RelayCommand(test);
        }

        private async void OpenVisionForm(object obj)
        {
            HikVision = new HikVisionViewModel();
            if (obj is ContentDialog dialog) await dialog.ShowAsync();
        }

        private async void OpenManiForm(object obj)
        {
            Manipulator = new ManipulatorViewModel();
            if (obj is ContentDialog dialog) await dialog.ShowAsync();
        }

        private void test(object obj)
        {
            try
            {
                VmSolution.Load(@"C:\Users\Xlent_XIR02\Desktop\livecam.sol");
                var i = VmSolution.Instance["Live"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        [ApplicationRestartAspect]
        private void SaveGeneral(object obj)
        {
            _jsonDb.Update(_mapper.Map<GeneralModel>(General));
            MessageBox.Show("Saved setting. Restarting the application is required for the changes to take effect.");
        }

        private async void DeleteManipulator(object obj)
        {
            await _manipulatorDb.Delete(_mapper.Map<ManipulatorModel>(Manipulator));

            GetManipulators();
        }

        [ApplicationRestartAspect]
        private void SaveVision(object obj)
        {
            _jsonDb.Update(_mapper.Map<HikVisionModel>(HikVision));
            MessageBox.Show("Saved setting. Restarting the application is required for the changes to take effect.");
        }

        private async void AddManipulator(object obj)
        {
            bool saveStatus = await _manipulatorDb.Add(_mapper.Map<ManipulatorModel>(Manipulator));
            if (saveStatus)
            {
                MessageBox.Show("Added New Manipulator");
            }
            else
            {
                MessageBox.Show("Failed to save setting!");
            }

            GetManipulators();
        }

        private async void SaveManipulator(object obj)
        {
            bool saveStatus = await _manipulatorDb.Update(_mapper.Map<ManipulatorModel>(Manipulator));
            if (saveStatus)
            {
                System.Windows.MessageBox.Show(ConfigurationManager.AppSettings["SaveSettingCommand_SaveMessage"]);
            }
            else
            {
                System.Windows.MessageBox.Show("Failed to save setting!");
            }

            GetManipulators();
        }

        private async void GetManipulators()
        {
            IEnumerable<ManipulatorModel> models = await _manipulatorDb.GetAll();
            Manipulator = null;
            Manipulators.Clear();

            foreach (var model in models)
            {
                Manipulators.Add(_mapper.Map<ManipulatorViewModel>(model));
            }
        }

        private void OnManipulatorChangeEvent(object obj)
        {
            Manipulator = ((ManipulatorViewModel)obj).Clone() as ManipulatorViewModel;
        }

        private void OnVisionChangeEvent(object obj)
        {
            HikVision = ((HikVisionViewModel)obj).Clone() as HikVisionViewModel;
        }
    }
}