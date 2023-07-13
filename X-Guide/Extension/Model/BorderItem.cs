using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.Extension.Model
{
    public class BorderItem : ViewModelBase
    {
        public string Text { get; set; }
        public bool Status { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
