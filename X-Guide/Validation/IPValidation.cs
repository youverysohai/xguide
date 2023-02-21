using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace X_Guide.Validation
{
    public static class IPValidation
    {

        //public string? MustEndWith { get; set; }
        public static bool ValidateIPSegment(string value)
        {

            
            string pattern = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";

            if (string.IsNullOrEmpty(value))
            {
                return false;

            }
            else if (!Regex.Match(value, pattern).Success)
            {
                return false;
            }
            return true;
        }



/*        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int port;
            if (int.TryParse(value.ToString(), out port))
            {
                if (port >= 0 && port <= 65535)
                {
                    return ValidationResult.ValidResult;
                }
            }
            return new ValidationResult(false, "Please enter a valid port number between 0 and 65535.");
        }*/

    }
}