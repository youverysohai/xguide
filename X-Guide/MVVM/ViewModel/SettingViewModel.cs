﻿using AutoMapper;
using ModernWpf.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using VM.Core;
using X_Guide.Communication.Service;
using X_Guide.CustomEventArgs;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;
using X_Guide.Validation;
using X_Guide.VisionMaster;
using Xlent_Vision_Guided;

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
        private readonly IVisionDb _visionDb;
        private readonly IMapper _mapper;
        private readonly IGeneralDb _generalDb;
        public bool Test { get; set; }
        public bool HasErrors => false;

   

        public GeneralViewModel General { get; set; }
        public ManipulatorViewModel Manipulator { get; set; }
        public VisionViewModel Vision { get; set; } 
        public ObservableCollection<ManipulatorViewModel> Manipulators { get; } = new ObservableCollection<ManipulatorViewModel>();

        public ObservableCollection<VisionViewModel> Visions { get; } = new ObservableCollection<VisionViewModel>();


        public string LogFilePath { get; set; }


        public SettingViewModel(IManipulatorDb machineDb, IVisionDb visionDb, IMapper mapper, ICalibrationDb calibrationDb, IClientService clientService, IServerService serverService, IVisionService visionService, IGeneralDb generalDb)
        {
            _manipulatorDb = machineDb;
            _visionDb = visionDb;
            _mapper = mapper;
            _generalDb = generalDb;
            General = _generalDb.Get();
            GetManipulators();
            GetVisions();
            OpenVisionFormCommand = new RelayCommand(OpenVisionForm);
            OpenManiFormCommand = new RelayCommand(OpenManiForm);
            AddVisionCommand = new RelayCommand(AddVision);
            VisionCommand = new RelayCommand(OnVisionChangeEvent);
            ManipulatorCommand = new RelayCommand(OnManipulatorChangeEvent);
            SaveManipulatorCommand = new RelayCommand(SaveManipulator);
            OpenFormCommand = new RelayCommand(OpenVisionForm);
            DeleteManipulatorCommand = new RelayCommand(DeleteManipulator);
            DeleteVisionCommand = new RelayCommand(DeleteVision);
            SaveVisionCommand = new RelayCommand(SaveVision);
            SaveGeneralCommand = new RelayCommand(SaveGeneral);
            AddManipulatorCommand = new RelayCommand(AddManipulator);
            TestCommand = new RelayCommand(test);
        }

        private async void OpenVisionForm(object obj)
        {
            Vision = new VisionViewModel();
            if (obj is ContentDialog dialog) await dialog.ShowAsync();
        
        }

        private async void OpenManiForm(object obj)
        {
            Manipulator = new ManipulatorViewModel();
            if (obj is ContentDialog dialog) await dialog.ShowAsync();
       
        }

        private void test(object obj)
        {
            Test = true;
        }

        private void SaveGeneral(object obj)
        {
            _generalDb.Update(General);
        }

        private async void DeleteVision(object obj)
        {
            await _visionDb.Delete(_mapper.Map<VisionModel>(Vision));
            GetVisions();
        }

        private async void DeleteManipulator(object obj)
        {
            await _manipulatorDb.Delete(_mapper.Map<ManipulatorModel>(Manipulator));
            GetManipulators();
        }

        private async void SaveVision(object obj)
        {
            await _visionDb.Update(_mapper.Map<VisionModel>(Vision));
            GetVisions();
        }

        private async void AddVision(object obj)
        {
            await _visionDb.Add(_mapper.Map<VisionModel>(Vision));
            GetVisions();
        }


        public async void GetVisions()
        {
            IEnumerable<VisionModel> models = await _visionDb.GetAll();
            Vision = null;
            Visions.Clear();
            foreach (var model in models)
            {
                Visions.Add(_mapper.Map<VisionViewModel>(model));
            }
        }

        private async void AddManipulator(object obj)
        {

            bool saveStatus = await _manipulatorDb.Add(_mapper.Map<ManipulatorModel>(Manipulator));
            if (saveStatus)
            {
                System.Windows.MessageBox.Show("Added New Manipulator");
            }
            else
            {
                System.Windows.MessageBox.Show("Failed to save setting!");
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
    
            Vision = ((VisionViewModel)obj).Clone() as VisionViewModel;
        }




    }



}



