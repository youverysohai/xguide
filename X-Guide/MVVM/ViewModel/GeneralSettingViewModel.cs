using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.MVVM.ViewModel
{
    public class GeneralSettingViewModel  : ViewModelBase
    {

		private ObservableCollection<string> _ip;

		private int _port;

		private string _terminator;

		public string Terminator
		{
			get { return _terminator; }
			set { _terminator = value; OnPropertyChanged(); }
		}


		public int Port
		{
			get { return _port; }
			set { _port = value; OnPropertyChanged(); }
		}

		public ObservableCollection<string> Ip	
		{
			get { return _ip; }
			set { _ip = value; OnPropertyChanged(); }
		}

		

	}
}
