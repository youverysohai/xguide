using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Vision.Common.Models
{
    public class RateResult:BindableBase
    {
        private bool isPassRate;
        private int totalUnit;
        private string? color;

        private int currentCount;

        public int CurrentCount
        {
            get { return currentCount; }
            set { currentCount = value; RaisePropertyChanged(); }
        }

        public bool IsPassRate
        {
            get { return isPassRate; }
            set { isPassRate = value; RaisePropertyChanged(); }
        }

        public int TotalUnit
        {
            get { return totalUnit; }
            set { totalUnit = value; RaisePropertyChanged(); }
        }

        public string Color
        {
            get { return color; }
            set { color = value; RaisePropertyChanged(); }
        }

        private int percentageValue;

        public int PercentageValue
        {
            get { return percentageValue; }
            set { percentageValue = value; RaisePropertyChanged(); }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }

    }
}
