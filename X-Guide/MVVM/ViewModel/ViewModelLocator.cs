using Autofac;
using Autofac.Core;
using System;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;

namespace X_Guide.MVVM.ViewModel
{
    public class ViewModelLocator : IViewModelLocator
    {
        private readonly ILifetimeScope _lifeTimeScope;

        public ViewModelLocator(ILifetimeScope lifetimeScope)
        {
            _lifeTimeScope = lifetimeScope;
        }

        public ViewModelBase Create<T>(string scopeName = null, params Parameter[] parameters) where T : ViewModelBase
        {
            ILifetimeScope scope = _lifeTimeScope;
            try
            {
                if (scopeName != null) scope = _lifeTimeScope.BeginLifetimeScope(scopeName);
                return scope?.Resolve<T>(parameters);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (scope != _lifeTimeScope) scope.Dispose();
            }
        }

        public ViewModelBase CreateStep1(CalibrationViewModel setting)
        {
            return _lifeTimeScope?.Resolve<Step1ViewModel>(new TypedParameter(typeof(CalibrationViewModel), setting));
        }

        public ViewModelBase CreateStep2(CalibrationViewModel setting)
        {
            return _lifeTimeScope?.Resolve<Step2ViewModel>(new TypedParameter(typeof(CalibrationViewModel), setting));
        }

        public ViewModelBase CreateStep3(CalibrationViewModel setting)
        {
            return _lifeTimeScope?.Resolve<Step3ViewModel>(new TypedParameter[]
            {
                new TypedParameter(typeof(CalibrationViewModel), setting),
            });
        }

        public ViewModelBase CreateStep4(CalibrationViewModel setting)
        {
            return _lifeTimeScope?.Resolve<Step4ViewModel>(new TypedParameter(typeof(CalibrationViewModel), setting));
        }

        public ViewModelBase CreateStep5(CalibrationViewModel setting)
        {
            return _lifeTimeScope?.Resolve<Step5ViewModel>(new TypedParameter(typeof(CalibrationViewModel), setting));
        }

        public ViewModelBase CreateStep6(CalibrationViewModel setting)
        {
            return _lifeTimeScope?.Resolve<Step6ViewModel>(new TypedParameter(typeof(CalibrationViewModel), setting));
        }

        public ViewModelBase CreateCalibrationMainViewModel(CalibrationViewModel setting)
        {
            return _lifeTimeScope?.Resolve<CalibrationMainViewModel>(new TypedParameter(typeof(CalibrationViewModel), setting));
        }

        public ViewModelBase CreateCalibrationWizardStart(NavigationStore navigationStore)
        {
            return _lifeTimeScope?.Resolve<CalibrationWizardStartViewModel>(new TypedParameter(typeof(NavigationStore), navigationStore));
        }
    }
}