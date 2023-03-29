﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    public class ViewModelLocator : IViewModelLocator
    {
        private readonly IContainer _dependencyResolver;

        public ViewModelLocator(IContainer dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }




        public ViewModelBase Create<T>() where T : ViewModelBase
        {
            try
            {
                return _dependencyResolver?.Resolve<T>();
            }
            catch
            {
                return null;
            }
        }

        public ViewModelBase CreateStep1(CalibrationViewModel setting)
        {
            return _dependencyResolver?.Resolve<Step1ViewModel>(new TypedParameter(typeof(CalibrationViewModel), setting));
        }

        public ViewModelBase CreateStep2(CalibrationViewModel setting)
        {
            return _dependencyResolver?.Resolve<Step2ViewModel>(new TypedParameter(typeof(CalibrationViewModel), setting));
        }

        public ViewModelBase CreateStep3(CalibrationViewModel setting)
        {
            return _dependencyResolver?.Resolve<Step3ViewModel>(new TypedParameter(typeof(CalibrationViewModel), setting));
        }
        public ViewModelBase CreateStep4(CalibrationViewModel setting)
        {
            return _dependencyResolver?.Resolve<Step4ViewModel>(new TypedParameter(typeof(CalibrationViewModel), setting));
        }        
        public ViewModelBase CreateStep5(CalibrationViewModel setting)
        {
            return _dependencyResolver?.Resolve<Step5ViewModel>(new TypedParameter(typeof(CalibrationViewModel), setting));
        }
        public ViewModelBase CreateStep6(CalibrationViewModel setting)
        {
            return _dependencyResolver?.Resolve<Step6ViewModel>(new TypedParameter(typeof(CalibrationViewModel), setting));
        }

        public ViewModelBase CreateCalibrationMainViewModel(string name)
        {
            return _dependencyResolver?.Resolve<CalibrationMainViewModel>(new TypedParameter(typeof(string), name));

        }

  

        public ViewModelBase CreateCalibrationWizardStart(NavigationStore navigationStore)
        {
            return _dependencyResolver?.Resolve<CalibrationWizardStartViewModel>(new TypedParameter(typeof(NavigationStore), navigationStore));
        }
    }
}