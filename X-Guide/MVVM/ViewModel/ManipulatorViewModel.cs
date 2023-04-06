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
    public class ManipulatorViewModel : ViewModelBase, ICloneable
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }


        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private int _type;

        public int Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

   



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