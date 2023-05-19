using Autofac;
using AutoMapper;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using X_Guide.Service.DatabaseProvider;
using MessageBox = HandyControl.Controls.MessageBox;

namespace X_Guide.MVVM.ViewModel
{
    public class CalibrationMainViewModel : ViewModelBase
    {
        public CalibrationViewModel Calibration { get; set; }

        public List<string> StepBarContent { get; set; }

        public bool CanExecuteNext { get; set; }

        private bool _canExecutePrev = true;

        public bool CanExecutePrev
        {
            get { return _canExecutePrev; }
            set
            {
                _canExecutePrev = value;
                WizPrevCommand?.OnCanExecuteChanged();
            }
        }

        private readonly LinkedList<ViewModelBase> _navigationHistory = new LinkedList<ViewModelBase>();

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

        public CalibrationMainViewModel(CalibrationViewModel calibration, INavigationService navigationService, IViewModelLocator viewModelLocator, IManipulatorDb manipulatorDb, IMapper mapper)
        {
            StepBarContent = new List<string>(new string[] { "Manipulator", "Orientation" + Environment.NewLine + "& Mounting", "Vision Flow", "Motion", "Jog", "Calibration", });
            _navigationService = navigationService;
            _navigationStore = _navigationService.GetNavigationStore();
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _viewModelLocator = viewModelLocator;
            WizNextCommand = new RelayCommand(WizNext, (o) => CanExecuteNext);
            WizPrevCommand = new RelayCommand(WizPrev, (o) => CanExecutePrev);
            CancelCommand = new RelayCommand(CancelCalib);
            Calibration = calibration;
            //calibration.Manipulator = mapper.Map<ManipulatorViewModel>(manipulatorDb.Get(1));

            TypedParameter calibrationConfig = new TypedParameter(typeof(CalibrationViewModel), Calibration);
            ICalibrationStep step1 = viewModelLocator.Create<Step1ViewModel>(calibrationConfig) as Step1ViewModel;
            step1.Register(OnCriticalDataChanged);
            step1.RegisterStateChange(OnStateChanged);

            _navigationService.Navigate(step1 as ViewModelBase);
            CurrentNode = _navigationHistory.AddLast(CurrentViewModel);
        }

        private void OnStateChanged(bool obj)
        {
            CanExecuteNext = obj;
            WizNextCommand?.OnCanExecuteChanged();
        }

        private void OnCriticalDataChanged()
        {
            var disposeNode = _navigationHistory.Last;
            while (CurrentNode != disposeNode)
            {
                disposeNode.Value.Dispose();
                NavigationHistory.Remove(disposeNode);
                disposeNode = _navigationHistory.Last;
            }
        }

        public void LoadCalibSetting(TypedParameter calib)
        {
            _navigationHistory.AddLast(_viewModelLocator.Create<Step2ViewModel>(calib));
            _navigationHistory.AddLast(_viewModelLocator.Create<Step3HikViewModel>(calib));
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

        public async void WizNext(object parameter)
        {
            LinkedListNode<ViewModelBase> nextNode = CurrentNode.Next;

            if (nextNode != null)
            {
                await _navigationService.NavigateAsync(nextNode.Value);
                CurrentNode = nextNode;
                CurrentStep += 1;
            }
            else
            {
                try
                {
                    await NavigateToStep(CurrentStep + 1);
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

        private async Task NavigateToStep(int currentStep)
        {
            var calibPara = new TypedParameter(typeof(CalibrationViewModel), Calibration);
            ViewModelBase viewModel = null;
          
                switch (currentStep)
                {
                    case 0: viewModel = _navigationService.Navigate<Step1ViewModel>(calibPara); break;
                    case 1: viewModel = _navigationService.Navigate<Step2ViewModel>(calibPara); break;
                    case 2: viewModel = await _navigationService.NavigateAsync<Step3HikViewModel>(calibPara); break;
                    case 3: viewModel = _navigationService.Navigate<Step4ViewModel>(calibPara); break;
                    case 4: viewModel = await _navigationService.NavigateAsync<Step5ViewModel>(calibPara); break;
                    case 5: viewModel = _navigationService.Navigate<Step6ViewModel>(calibPara); break;
                    default: throw new Exception("Page does not exist!");
                }
           
            (viewModel as ICalibrationStep).Register(OnCriticalDataChanged);
            (viewModel as ICalibrationStep).RegisterStateChange(OnStateChanged);
        }

        private void CancelCalib(object obj)
        {
            _navigationService.Navigate<CalibrationWizardStartViewModel>();
        }

        private void OnCanExecuteChange(object sender, bool e)
        {
            CanExecuteNext = e;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}