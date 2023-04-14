using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace X_Guide
{
    public static class StrRetriver
    {
        public static string Get(string key)
        {
            return (string)Application.Current.Resources[key];
        }
    }
}
