using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Model;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step2ViewModel : ViewModelBase
    {
    
        private CalibrationViewModel _setting;


        public CalibrationViewModel Setting
        {
            get { return _setting; }
            set
            {
                _setting = value;
                OnPropertyChanged();
            }
        }

        private ManipulatorViewModel _manipulator;
        private readonly IManipulatorDb _manipulatorDb;
        private readonly IMapper _mapper;

        public ManipulatorViewModel Manipulator
        {
            get { return _manipulator; }
            set { _manipulator = value;
                OnPropertyChanged();
            }
        }
            

        public Step2ViewModel(CalibrationViewModel setting, IManipulatorDb manipulatorDb, IMapper mapper)
        {
            Setting = setting;
            _manipulatorDb = manipulatorDb;
            _mapper = mapper;
            LoadManipulator();
        
        }

        private async void LoadManipulator()
        {
            var i = await _manipulatorDb.GetManipulator(Setting.ManipulatorId);
            Manipulator = _mapper.Map<ManipulatorViewModel>(i);
        }

    }
}
