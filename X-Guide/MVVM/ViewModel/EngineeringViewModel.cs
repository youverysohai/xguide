using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.MVVM.ViewModel
{
    public class EngineeringViewModel : ViewModelBase
    {
        private ObservableCollection<MyList> _myList;

        public ObservableCollection<MyList> MList
        {
            get { return _myList; }
            set { _myList = value;
                OnPropertyChanged();
            }
        }

        public EngineeringViewModel() {
            ObservableCollection<MyList> myList = new ObservableCollection<MyList>
            {
                new MyList { Header = "Item 1", Content = "This is the content for item 1" },
                new MyList { Header = "Item 2", Content = "This is the content for item 2" },
                new MyList { Header = "Item 3", Content = "This is the content for item 3" }
            };

        }
    }
    public class MyList
    {
        public string Header { get; set; }
        public string Content { get; set; }
    }
}
