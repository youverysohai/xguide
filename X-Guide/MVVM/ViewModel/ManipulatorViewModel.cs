using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;

namespace X_Guide.MVVM.ViewModel
{
    [Serializable]
    public class ManipulatorViewModel : ViewModelBase, ICloneable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }


        public int Type { get; set; }

        public object Clone()
        {
            return new ManipulatorViewModel
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Type = Type,
            };

        }

        public ManipulatorViewModel()
        {
            
        }






    }
}