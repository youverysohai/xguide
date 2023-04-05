using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace X_Guide.MVVM.ViewModel
{
    public class VisionViewModel : ViewModelBase, ICloneable
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }


        private ObservableCollection<string> _ip;

        public ObservableCollection<string> Ip
        {
            get { return _ip; }
            set { _ip = value; OnPropertyChanged(); }
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

        private string _filepath;

        public string Filepath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }

        public object Clone()
        {
            return new VisionViewModel
            {

                Name = Name,
                Port = Port,
                Ip = new ObservableCollection<string>(Ip.ToList()),
                Terminator = Terminator,
                Filepath = Filepath,
            };

        }






    }
}
