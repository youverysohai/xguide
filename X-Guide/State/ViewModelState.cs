using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.State
{
    public class ViewModelState
    {
        public Action OnStateChanged;


        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value;
                OnStateChanged?.Invoke();
            }
        }


    }
}
