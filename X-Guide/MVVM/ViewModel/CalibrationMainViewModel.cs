using Autofac;
using AutoMapper;
using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using X_Guide.Communication.Service;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Store;

using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.Communication;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    public class CalibrationMainViewModel : ViewModelBase
    {
        private CalibrationViewModel _calib;
        public CalibrationViewModel Calib
        {
            get => _calib; 
            set
            {
                _calib = value;
                OnPropertyChanged();
            }
        }

        private bool _canExecuteNext = true;

        public bool CanExecuteNext
        {
            get { return _canExecuteNext; }
            set
            {
                _canExecuteNext = value;
                WizNextCommand?.OnCanExecuteChanged();

            }
        }

        private bool _canExecutePrev = true;

        public bool CanExecutePrev
        {
            get { return _canExecutePrev = true; }
            set
            {
                _canExecutePrev = value;
                WizPrevCommand?.OnCanExecuteChanged();
            }
        }


        public LinkedList<ViewModelBase> _navigationHistory = new LinkedList<ViewModelBase>();


        public LinkedList<ViewModelBase> NavigationHistory => _navigationHistory;

        // Define the data list for the StepBar control
        public NavigationStore _navigationStore;
        private readonly INavigationService _navigationService;
        private readonly IViewModelLocator _viewModelLocator;

        public RelayCommand WizNextCommand { get; set; }
        public RelayCommand WizPrevCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;


        private int _stepIndex;

        private int _currentStep;
   

        public int StepIndex
        {
            get { return _stepIndex; }
            set
            {
                _stepIndex = value;
                OnPropertyChanged();
            }
        }


        public int CurrentStep
        {
            get { return _currentStep; }
            set
            {
                _currentStep = value;
                StepIndex = _currentStep;
            }
        }
        public LinkedListNode<ViewModelBase> CurrentNode { get; set; }

       public void LoadCalibSetting(TypedParameter calib)
        {
            _navigationHistory.AddLast(_viewModelLocator.Create<Step2ViewModel>(calib));
            _navigationHistory.AddLast(_viewModelLocator.Create<Step3ViewModel>(calib));
            _navigationHistory.AddLast(_viewModelLocator.Create<Step4ViewModel>(calib));
            _navigationHistory.AddLast(_viewModelLocator.Create<Step5ViewModel>(calib));
            _navigationHistory.AddLast(_viewModelLocator.Create<Step6ViewModel>(calib));
        }

        public void OnIndexChanged(FunctionEventArgs<int> e)
        {
            if (e.Info == CurrentStep) return;
            LinkedListNode<ViewModelBase> node = _navigationHistory.First;
            for (int i = 0; i < e.Info; i++)
            {
                node = node?.Next;
            }

            if (node != null)
            {
                CurrentNode = node;
                CurrentStep = e.Info;
                _navigationService.Navigate(node.Value);
            }
            else
            {
                StepIndex = CurrentStep;
                MessageBox.Show("Page not found");

            }
        }



        public void WizNext(object parameter)
        {
            LinkedListNode<ViewModelBase> nextNode = CurrentNode.Next;

            if (nextNode != null)
            {
                _navigationService.Navigate(nextNode.Value);
                CurrentNode = nextNode;
                CurrentStep += 1;
            }
            else
            {
                try
                {
                    NavigateToStep(CurrentStep + 1);
                    CurrentNode = NavigationHistory.AddLast(_navigationStore.CurrentViewModel);
                    CurrentStep += 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }  
            }

        }
    
        public void WizPrev(object parameter)
        {
            LinkedListNode<ViewModelBase> prevNode = CurrentNode.Previous;

            if (prevNode != null)
            {
                CurrentNode = prevNode;
                CurrentStep -= 1;
                _navigationService.Navigate(prevNode.Value); 
            }

            else
            {
                MessageBox.Show("This is the first page!");
            }

        }

        private void NavigateToStep(int currentStep)
        {
            var calibPara = new TypedParameter(typeof(CalibrationViewModel), _calib);
            
            switch (currentStep)
            {
                case 0: _navigationService.Navigate<Step1ViewModel>(calibPara); break;
                case 1: _navigationService.Navigate<Step2ViewModel>(calibPara); break;
                case 2: _navigationService.Navigate<Step3ViewModel>(calibPara); break;
                case 3:  _navigationService.Navigate<Step4ViewModel>(calibPara); break;
                case 4:  _navigationService.Navigate<Step5ViewModel>(calibPara); break;
                case 5:  _navigationService.Navigate<Step6ViewModel>(calibPara); break;
                default: throw new Exception("Page does not exist!");
            }

        }

        public CalibrationMainViewModel(CalibrationViewModel calib, INavigationService navigationService, IViewModelLocator viewModelLocator)
        {


            _navigationService = navigationService;
            _navigationStore = _navigationService.GetNavigationStore();
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _viewModelLocator = viewModelLocator;
            WizNextCommand = new RelayCommand(WizNext, (o) => CanExecuteNext);
            WizPrevCommand = new RelayCommand(WizPrev, (o) => CanExecutePrev);
            CancelCommand = new RelayCommand(CancelCalib);

            Calib = calib;
            var calibPara = new TypedParameter(typeof(CalibrationViewModel), _calib);
            Step1ViewModel Step1 = viewModelLocator.Create<Step1ViewModel>(calibPara) as Step1ViewModel;
            Step1.SelectedItemChangedEvent += OnSelectedItemChangedEvent;
            _navigationService.Navigate(Step1);
            CurrentNode = _navigationHistory.AddLast(CurrentViewModel);
        }

        private void CancelCalib(object obj)
        {
            _navigationService.Navigate<CalibrationWizardStartViewModel>();
        }

        private void OnCanExecuteChange(object sender, bool e)
        {
            CanExecuteNext = e;
        }

        private void OnSelectedItemChangedEvent()
        {
            NavigationHistory.Clear();
            CurrentNode = NavigationHistory.AddLast(CurrentViewModel);
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

    }


}
