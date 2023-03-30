using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.MVVM.ViewModel
{
    public class ManipulatorViewModel : ViewModelBase
    {
        ErrorViewModel _errorViewModel = new ErrorViewModel();
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {

                _name = value;
                /*if (string.IsNullOrEmpty(value))
                {
                    _errorViewModel.AddError("Please enter a valid value for this field. This field cannot be left blank.");
                }
                else
                {
                    _errorViewModel.RemoveError("Please enter a valid value for this field. This field cannot be left blank.");
                }

                if (value.Length > 30)
                {
                    _errorViewModel.AddError("The field must not exceed 30 characters");
                }
                else
                {
                    _errorViewModel.RemoveError("The field must not exceed 30 characters");
                }*/
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
           /*     if (string.IsNullOrEmpty(value))
                {
                    _errorViewModel.AddError("Please enter a valid value for this field. This field cannot be left blank.");
                }
                else
                {
                    _errorViewModel.RemoveError("Please enter a valid value for this field. This field cannot be left blank.");
                }*/
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


        private string _ip1;
        public string IP1
        {
            get { return _ip1; }
            set
            {
                _ip1 = value;
                /*  
                  if (!IPValidation.ValidateIPSegment(value)) _errorViewModel.AddError("Please enter a valid value for this field.");
                  else _errorViewModel.RemoveError("Please enter a valid value for this field.");*/
                OnPropertyChanged();
            }
        }
        private string _ip2;
        public string IP2
        {
            get { return _ip2; }
            set
            {

                _ip2 = value;
             /*   if (!IPValidation.ValidateIPSegment(value)) _errorViewModel.AddError("Please enter a valid value for this field.");
                else _errorViewModel.RemoveError("Please enter a valid value for this field.");*/
                OnPropertyChanged();
            }
        }
        private string _ip3;
        public string IP3
        {
            get { return _ip3; }
            set
            {

                _ip3 = value;
             /*   if (!IPValidation.ValidateIPSegment(value)) _errorViewModel.AddError("Please enter a valid value for this field.");
                else _errorViewModel.RemoveError("Please enter a valid value for this field.");*/
                OnPropertyChanged();
            }
        }
        private string _ip4;
        public string IP4
        {
            get { return _ip4; }
            set
            {

                _ip4 = value;
            /*    if (!IPValidation.ValidateIPSegment(value)) _errorViewModel.AddError("Please enter a valid value for this field.");
                else _errorViewModel.RemoveError("Please enter a valid value for this field.");*/
                OnPropertyChanged();
            }
        }

        private string _port;
        public string Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }

        private string _manipulatorTerminator;
        public string Terminator
        {
            get { return _manipulatorTerminator; }
            set
            {
                _manipulatorTerminator = value;
                OnPropertyChanged();
            }
        }

    }
}
