using Autofac;
using AutoMapper;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using X_Guide.MessageToken;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Store;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service;
using XGuideSQLiteDB;
using XGuideSQLiteDB.Models;
using MessageBox = HandyControl.Controls.MessageBox;

namespace X_Guide.MVVM.ViewModel
{
    public class CalibrationStep
    {
    }

    [SupportedOSPlatform("windows")]
    public class CalibrationMainViewModel : ViewModelBase, IRecipient<CalibrationStateChanged>
    {
        public CalibrationViewModel Calibration { get; set; }

        private readonly IMessenger _messenger;
        private ILifetimeScope _lifeTimeScope;

        public List<string> StepBarContent { get; set; }

        private readonly IMapper _mapper;
        private bool _canExecuteNext;

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
        private readonly IRepository<Calibration> _repository;
        private readonly IViewModelLocator _viewModelLocator;

        public RelayCommand WizNextCommand { get; set; }
        public RelayCommand WizPrevCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public int StepIndex { get; set; }

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

        public LinkedListNode<ViewModelBase> CurrentNode { get; set; }

        public CalibrationMainViewModel(INavigationService navigationService, IViewModelLocator viewModelLocator, IMessenger messenger, ILifetimeScope lifeTimeScope, IRepository<Calibration> repository, IMapper mapper, string name = "Lorem Ipsum", int cid = -1)
        {
            StepBarContent = new List<string>(new string[] { "Manipulator", "Orientation" + Environment.NewLine + "& Mounting", "Vision Flow", "Motion", "Jog", "Calibration", });
            _mapper = mapper;
            _navigationService = navigationService;
            _repository = repository;
            _navigationStore = _navigationService.GetNavigationStore();
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _viewModelLocator = viewModelLocator;
            WizNextCommand = new RelayCommand(WizNext, (o) => CanExecuteNext);
            WizPrevCommand = new RelayCommand(WizPrev, (o) => CanExecutePrev);
            CancelCommand = new RelayCommand(CancelCalib);
            _messenger = messenger;
            messenger.Register(this);
            _lifeTimeScope = lifeTimeScope;

            LoadCalibSetting(cid);
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

        public override void Dispose()
        {
            _messenger.Unregister<CalibrationStateChanged>(this);
            base.Dispose();
        }

        public async void LoadCalibSetting(int cid)
        {
            if (cid != -1)
            {
                var i = await _repository.GetById(cid, new Expression<Func<Calibration, object>>[] {
                c=> c.Manipulator
            });
                Calibration = _mapper.Map<CalibrationViewModel>(i);
                _lifeTimeScope = _lifeTimeScope.BeginLifetimeScope(builder =>
               {
                   builder.RegisterInstance(Calibration).As<CalibrationViewModel>();
               });
            }
            else
            {
                _lifeTimeScope = _lifeTimeScope.BeginLifetimeScope();
            }
            _navigationService.SetScope(_lifeTimeScope);
            _navigationService.Navigate<Step1ViewModel>();
            CurrentNode = _navigationHistory.AddLast(CurrentViewModel);
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
                case 0: viewModel = _navigationService.Navigate<Step1ViewModel>(); break;
                case 1: viewModel = _navigationService.Navigate<Step2ViewModel>(); break;
                case 2: viewModel = await _navigationService.NavigateAsync<Step3ViewModel>(); break;
                case 3: viewModel = _navigationService.Navigate<Step4ViewModel>(); break;
                case 4: viewModel = await _navigationService.NavigateAsync<Step5ViewModel>(); break;
                case 5: viewModel = _navigationService.Navigate<Step6ViewModel>(); break;
                default: throw new Exception("Page does not exist!");
            }
        }

        private void CancelCalib(object obj)
        {
            _navigationService.Navigate<CalibrationWizardStartViewModel>();
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        void IRecipient<CalibrationStateChanged>.Receive(CalibrationStateChanged message)
        {
            switch (message.Value)
            {
                case PageState.Enable: CanExecuteNext = true; break;
                case PageState.Disable: CanExecuteNext = false; break;
                case PageState.Reset: OnCriticalDataChanged(); break;
                default: return;
            }
        }
    }
}