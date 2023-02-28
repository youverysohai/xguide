using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MVVM.Command
{
    internal class BrowseFileDialogCommand : CommandBase
    {
        private ProductionViewModel _productionViewModel;
        public BrowseFileDialogCommand(ProductionViewModel productionViewModel) { 
                _productionViewModel = productionViewModel;
        }    
        public override void Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "solution files (*.sol)|*.sol|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            _productionViewModel.VisionSolutionFilePath = openFileDialog.SafeFileName;
            //File.ReadAllText(openFileDialog.FileName);
        }
    }
}
