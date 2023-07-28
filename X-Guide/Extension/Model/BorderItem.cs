using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionGuided;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.Extension.Model
{
    [INotifyPropertyChanged]
    public partial class BorderItem : Point
    {
        public bool Status { get; set; }
    }
}
