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
    public class EngineeringViewModel : ViewModelBase
    {

        public CalibrationViewModel Setting { get => setting; set
            {
                setting = value;
                OnPropertyChanged();
            }
        }

        public LinkedList<ViewModelBase> _navigationHistory = new LinkedList<ViewModelBase>();
        public LinkedList<ViewModelBase> NavigationHistory => _navigationHistory;

        // Define the data list for the StepBar control
        public NavigationStore _navigationStore;
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
                var _navigationService = new NavigationService(_navigationStore, null);
                _navigationService.Navigate(node.Value);
                CurrentNode = node;
                CurrentStep = e.Info;
            }
            else
            {
                StepIndex = CurrentStep;
                MessageBox.Show("Page not found");

            }
        }





        public EngineeringViewModel(IMachineService machineService, IMapper mapper, string name, IServerService serverService)
        {


            _navigationStore = new NavigationStore();
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            WizNextCommand = new WizNextCommand(this, _navigationStore);
            WizPrevCommand = new WizPrevCommand(this, _navigationStore);

            Setting = new CalibrationViewModel
            {
                Name = name
            };

            _navigationStore.CurrentViewModel = new Step1ViewModel(machineService, mapper, Setting, serverService);
            CurrentNode = _navigationHistory.AddLast(CurrentViewModel);
        }



        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

    }


}
