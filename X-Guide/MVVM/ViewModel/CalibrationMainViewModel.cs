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

        public CalibrationViewModel Setting
        {
            get => setting; set
            {
                setting = value;
                OnPropertyChanged();
            }
        }



        public LinkedList<ViewModelBase> _navigationHistory = new LinkedList<ViewModelBase>();
        public LinkedList<ViewModelBase> NavigationHistory => _navigationHistory;

        // Define the data list for the StepBar control
        public NavigationStore _navigationStore;
        private readonly NavigationService _navigationService;
        private readonly IViewModelLocator _viewModelLocator;

        public ICommand WizNextCommand { get; set; }
        public ICommand WizPrevCommand { get; set; }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;


        private int _stepIndex;

        private int _currentStep;
        private CalibrationViewModel setting;

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
             
                ViewModelBase viewModel = GetStepViewModel(CurrentStep + 1);
                if (viewModel != null)
                {
                    _navigationService.Navigate(viewModel);
                    CurrentNode = NavigationHistory.AddLast(viewModel);
                    CurrentStep += 1;

                }
                else
                {
                    MessageBox.Show("This is the last page!");
                }
            }

        }

        public void WizPrev(object parameter)
        {


            LinkedListNode<ViewModelBase> prevNode = CurrentNode.Previous;

            if (prevNode != null)
            {
                _navigationService.Navigate(prevNode.Value);
                CurrentNode = prevNode;
                CurrentStep -= 1;
            }
            else
            {
                MessageBox.Show("This is the first page!");
            }

        }

        private ViewModelBase GetStepViewModel(int currentStep)
        {

            switch (currentStep)
            {
                case 0: return _viewModelLocator.CreateStep1(setting);
                case 1: return _viewModelLocator.CreateStep2(setting);
                case 2: return _viewModelLocator.CreateStep3(setting);
                case 3: return _viewModelLocator.CreateStep4(setting);
                case 4: return _viewModelLocator.CreateStep5(setting);
                case 5: return _viewModelLocator.CreateStep6(setting);
                default: return null;
            }

        }

        public CalibrationMainViewModel(string name, IViewModelLocator viewModelLocator)
        {

            
            _navigationStore = new NavigationStore();
            _navigationService = new NavigationService(_navigationStore);
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _viewModelLocator = viewModelLocator;
            WizNextCommand = new RelayCommand(WizNext);
            WizPrevCommand = new RelayCommand(WizPrev);
     
            Setting = new CalibrationViewModel
            {
                Name = name,
            };

            Step1ViewModel Step1 = viewModelLocator.CreateStep1(Setting) as Step1ViewModel;
            Step1.SelectedItemChangedEvent += OnSelectedItemChangedEvent;
            _navigationService.Navigate(Step1);
            CurrentNode = _navigationHistory.AddLast(CurrentViewModel);
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
