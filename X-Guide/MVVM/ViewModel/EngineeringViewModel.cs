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
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.View.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.DatabaseProvider;

namespace X_Guide.MVVM.ViewModel
{
    public class EngineeringViewModel : ViewModelBase
    {



        public LinkedList<ViewModelBase> _navigationHistory = new LinkedList<ViewModelBase>();
        public LinkedList<ViewModelBase> NavigationHistory => _navigationHistory;

        // Define the data list for the StepBar control
        public NavigationStore _navigationStore;
        public ICommand WizNextCommand { get; set; }
        public ICommand WizPrevCommand { get; set; }
    
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;


        private int _stepIndex;

        public int StepIndex
        {
            get { return _stepIndex; }
            set
            {
                _stepIndex = value;
                OnPropertyChanged();
            }
        }

        private int _currentStep;

        public int CurrentStep
        {
            get { return _currentStep; }
            set
            {
                _currentStep = value;
                StepIndex = _currentStep;
            }
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

        public LinkedListNode<ViewModelBase> CurrentNode { get; set; }

                                                            

        public EngineeringViewModel(IMachineService machineService, IMapper mapper)
        {


            _navigationStore = new NavigationStore();
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            WizNextCommand = new WizNextCommand(this, _navigationStore);
            WizPrevCommand = new WizPrevCommand(this, _navigationStore);

            _navigationStore.CurrentViewModel = new Step1ViewModel(machineService, mapper);
            CurrentNode = _navigationHistory.AddLast(CurrentViewModel);
        }



        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

    }


}
