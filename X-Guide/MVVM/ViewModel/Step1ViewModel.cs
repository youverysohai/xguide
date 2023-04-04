﻿using AutoMapper;
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

        private CalibrationViewModel _calib;

        public CalibrationViewModel Calib
        {
            get { return _calib; }
            set { _calib = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<ManipulatorViewModel> _manipulators;
        public ObservableCollection<ManipulatorViewModel> Manipulators
        {
            get { return _manipulators; }
            set
            {
                _manipulators = value;
                OnPropertyChanged();
            }
        }


        public IManipulatorDb _manipulatorDb { get; }
        private IMapper _mapper { get; }
        public IServerService _serverService { get; }

        #endregion
        public Step1ViewModel(IManipulatorDb manipulatorDb, IMapper mapper, CalibrationViewModel calib)
        {
            _manipulatorDb = manipulatorDb;
            _mapper = mapper;
            _calib = calib;
            LoadManipulator();
        }

        private async void LoadManipulator()
        {
            var models = await _manipulatorDb.GetAllManipulator();
            var viewModels = models.Select(x => _mapper.Map<ManipulatorViewModel>(x));
            Manipulators = new ObservableCollection<ManipulatorViewModel>(viewModels);

        }

    }
}
