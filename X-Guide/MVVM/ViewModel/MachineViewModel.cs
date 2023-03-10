using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;

namespace X_Guide.MVVM.ViewModel
{
    public class MachineViewModel : ViewModelBase
    {


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

        private string _type;

        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        public static MachineViewModel ToViewModel(MachineModel machine, IMapper mapper)
        {
            return mapper.Map<MachineViewModel>(machine);
        }

        public MachineViewModel()
        {

        }






    }
}
