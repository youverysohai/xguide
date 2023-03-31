using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.MVVM.ViewModel
{
    public class VisionViewModel : ViewModelBase
    {

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value;
                OnPropertyChanged();
            }
        }

        private string _ip;
        public string Ip
        {

            get { return _ip; }
            set
            {
                _ip = value;
                OnPropertyChanged();
            }
        }

        private int _port;

        public int Port
        {
            get { return _port; }
            set { _port = value;
                OnPropertyChanged();
            }
        }

        private string _terminator;

        public string Terminator
        {
            get { return _terminator; }
            set { _terminator = value;
                OnPropertyChanged(); }
        }







    }
}
