﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.Store;
using X_Guide.Service;

namespace X_Guide.MVVM.ViewModel
{
    public class ProductionViewModel : ViewModelBase
    {
        public ICommand NavigateCommand { get; }

        public ICommand BrowseFileDialogCommand { get; }

        private string _visionSolutionFilePath;

        public string VisionSolutionFilePath
        {
            get { return _visionSolutionFilePath; }
            set { _visionSolutionFilePath = value; 
            OnPropertyChanged();    
            }
        }


        public ProductionViewModel()
        {
            BrowseFileDialogCommand = new BrowseFileDialogCommand(this);
        }
    }
}
