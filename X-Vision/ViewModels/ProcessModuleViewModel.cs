using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using X_Vision.Event;

namespace X_Vision.ViewModels
{
    public class ProcessModuleViewModel:BindableBase, IConfirmNavigationRequest
    {
        public ProcessModuleViewModel(IEventAggregator aggregator)
        {
            this.aggregator = aggregator;
        }

        private string title;
        private readonly IEventAggregator aggregator;

        public string Title
        {
            get { return title; }
            set { title = value;RaisePropertyChanged(); }
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
    }
}
