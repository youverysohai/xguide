using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.MVVM.ViewModel
{
    
    public class GeneralViewModel : ViewModelBase
    {
        public string Filepath { get; set; }
        public string Terminator { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public bool Debug { get; set; }
    }
}
