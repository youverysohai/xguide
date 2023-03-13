using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using VM.Core;
using X_Guide.MVVM.Command;
using X_Guide.MVVM.ViewModel.CalibrationWizardSteps;
using X_Guide.Service.Communation;

namespace X_Guide.MVVM.ViewModel
{
    internal class Step3ViewModel : ViewModelBase
    {
        public ICommand OpenFileCommand { get; }

        private CalibrationViewModel _setting;
        private readonly ServerCommand _serverCommand;
        private ObservableCollection<string> _visionFlow;

        public ObservableCollection<string> VisionFlow
        {
            get { return _visionFlow; }
            set
            {
                _visionFlow = value;
                OnPropertyChanged();
            }
        }

        private string _selectedItem;

        public string SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value;
                OnPropertyChanged();
                OnItemChanged(value);
            }
        }

  

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged();
                OpenFile();
            }
        }


        public Step3ViewModel(CalibrationViewModel setting, ServerCommand serverCommand)
        {
            _setting = setting;
            _serverCommand = serverCommand;
        }

        private void OnItemChanged(string value)
        {
            _setting.VisionFlow = value;
        }
        private void OpenFile()
        {
            try
            {
                VmSolution.Import(FilePath, "happy");
                ProcessInfoList i = VmSolution.Instance.GetAllProcedureList();
                List<ProcessInfo> procedureList = i.astProcessInfo.Where(x => x.strProcessName != null).ToList();
                VisionFlow = new ObservableCollection<string>(procedureList.Select(x => x.strProcessName));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public override ViewModelBase GetNextViewModel()
        {
            return new Step4ViewModel(_setting, _serverCommand);
        }
    }
}
