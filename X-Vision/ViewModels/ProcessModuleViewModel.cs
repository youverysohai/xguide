using HandyControl.Controls;
using HikVisionProvider;
using IMVSCircleFindModuCs;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VisionProvider.Interfaces;
using VM.Core;
using VMControls.Interface;
using X_Vision.Common.Models;
using X_Vision.Event;

namespace X_Vision.ViewModels
{
    public class ProcessModuleViewModel:BindableBase, IConfirmNavigationRequest
    {
        VmProcedure vmProcedure;
        const int increment = 1;
        private readonly IEventAggregator aggregator;
        
        private IVmModule _module;

        public IVmModule Module
        {
            get { return _module; }
            set { _module = value; RaisePropertyChanged(); }
        }

        public DelegateCommand StartCommand { get; private set; }
        public DelegateCommand ContinueCommand { get; private set; }
        public DelegateCommand StopCommand { get; private set; }
        public DelegateCommand ResetCommand { get; private set; }

        public ProcessModuleViewModel(IEventAggregator aggregator)
        {
           
            //IVisionService visionService = new HikVisionService(@"C:\Users\tung\Downloads\findCircle.sol",null,null);
            RateResults = new ObservableCollection<RateResult>();
            //StartCommand = new DelegateCommand(StartCapture);
            StartCommand = new DelegateCommand(StartRunOnce);
            ResetCommand = new DelegateCommand(ResetResult);
            ContinueCommand = new DelegateCommand(RunContinuous);
            StopCommand = new DelegateCommand(StopContinuous);
            CreateRateResult();
            CreateLogData();
            this.aggregator = aggregator;
        }

       
        private void StartRunOnce()
        {
            VmSolution.Load(@"C:\Users\tung\Downloads\findCircle.sol");
            vmProcedure = (VmProcedure)VmSolution.Instance["Flow1"];

            //IMVSCircleFindModuTool circleTool = (IMVSCircleFindModuTool)VmSolution.Instance["Flow1.Circle Search1"];
            ////vmProcedure.Run();
            //var i = circleTool.ModuResult;
            //Module = vmProcedure;
        }

        private void RunContinuous()
        {
            vmProcedure.Run();
            Total = (int)vmProcedure.ExecuteCount;
            IMVSCircleFindModuTool circleTool = (IMVSCircleFindModuTool)VmSolution.Instance["Flow1.Circle Search1"];
            //vmProcedure.Run();
            var i = circleTool.ModuResult;
            if (i.ModuStatus != 1)
            {
                RateResults.ElementAt(1).CurrentCount += increment;
                double val = (double)RateResults.ElementAt(1).CurrentCount / (double)Total * (double)100;
                RateResults.ElementAt(1).PercentageValue = (int)val;

            }
            else
            {
                RateResults.ElementAt(0).CurrentCount += increment;
                double val = (double)RateResults.ElementAt(0).CurrentCount / (double)Total * (double)100;
                RateResults.ElementAt(0).PercentageValue = (int)val;
            }
            Module = vmProcedure;
        }
        private void StopContinuous()
        {
            vmProcedure.ContinuousRunEnable = false;
        }
        private void ResetResult()
        {
            Total = 0;
            RateResults.ElementAt(0).PercentageValue = 0;
            RateResults.ElementAt(1).PercentageValue = 0;
            RateResults.ElementAt(1).TotalUnit = 0;
            RateResults.ElementAt(0).TotalUnit = 0;
            RateResults.ElementAt(1).CurrentCount = 0;
            RateResults.ElementAt(0).CurrentCount = 0;
            Growl.ClearGlobal();
        }
        

        private void StartCapture()
        {
            Total += increment;
            Random random = new Random();
            int randomNumber = random.Next(0, 5);
            RateResults.ElementAt(1).TotalUnit += increment;
            RateResults.ElementAt(0).TotalUnit += increment;
            if (randomNumber < 1)
            {
                RateResults.ElementAt(1).CurrentCount += increment;
                double val = (double)RateResults.ElementAt(1).CurrentCount / (double)Total * (double)100;
                RateResults.ElementAt(1).PercentageValue = (int)val;

                Growl.ErrorGlobal("NG! Unable to Recognise..");
            }
            else
            {
                RateResults.ElementAt(0).CurrentCount += increment;
                double val = (double)RateResults.ElementAt(0).CurrentCount / (double)Total * (double)100;
                RateResults.ElementAt(0).PercentageValue = (int)val;
            }
        }

        private ObservableCollection<RateResult> rateResults;

        public ObservableCollection<RateResult> RateResults
        {
            get { return rateResults; }
            set { rateResults = value; RaisePropertyChanged(); }
        }

        private int total = 0;

        public int Total
        {
            get { return total; }
            set { total = value; RaisePropertyChanged(); }
        }


        void CreateRateResult()
        {
            RateResults.Add(new RateResult() { Color = "Green", TotalUnit = 0, IsPassRate = true, CurrentCount = 0, PercentageValue = 0, Title = "Pass" });
            RateResults.Add(new RateResult() { Color = "Red", TotalUnit = 0, IsPassRate = false, CurrentCount = 0, PercentageValue = 0, Title = "Fail" });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
           
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if(navigationContext.Parameters.ContainsKey("Title"))
                navigationContext.Parameters.GetValue<string>("Title");
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            //aggregator.GetEvent<MessageEvent>().Publish("Hello");

            bool result = true;
            //if(MessageBox.Show("Confirm Navigation?","Reminder",MessageBoxButton.YesNo)== MessageBoxResult.No)
            //{
            //    result = false;
            //}
            continuationCallback(result);
        }

        private ObservableCollection<LogDto> logDtos;

        public ObservableCollection<LogDto> LogDtos
        {
            get { return logDtos; }
            set { logDtos = value; RaisePropertyChanged(); }
        }

        void CreateLogData()
        {
            LogDtos = new ObservableCollection<LogDto>();

            for(int i = 0; i<10; i++)
            {
                LogDtos.Add(new LogDto() { Title ="Process Module", Content="Opened Process Module", CreateDate = DateTime.Now });
                LogDtos.Add(new LogDto() { Title = "Process Module", Content = "Start Process Module", CreateDate = new DateTime(2023, 7, 10, 10, 30, 0) 
            });
            }
        }
    }
}
