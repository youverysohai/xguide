using HandyControl.Controls;
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
using X_Vision.Common.Models;
using X_Vision.Event;

namespace X_Vision.ViewModels
{
    public class ProcessModuleViewModel:BindableBase, IConfirmNavigationRequest
    {

        const int increment = 3;
        private readonly IEventAggregator aggregator;

        public DelegateCommand StartCommand { get; private set; }
        public DelegateCommand ResetCommand { get; private set; }

        public ProcessModuleViewModel(IEventAggregator aggregator)
        {
            RateResults = new ObservableCollection<RateResult>();
            StartCommand = new DelegateCommand(StartCapture);
            ResetCommand = new DelegateCommand(ResetResult);
            CreateRateResult();
            CreateLogData();
            this.aggregator = aggregator;
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
                LogDtos.Add(new LogDto() { Title ="Process Module", Content="Opened Process Module" });
                LogDtos.Add(new LogDto() { Title ="Process Module", Content="Start Process Module" });
            }
        }
    }
}
