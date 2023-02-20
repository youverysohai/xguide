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
    public class IPValidation : ValidationRule
    {
     
        public string MustEndWith { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            var str = value as string;
            string pattern = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";

            Regex rg = new Regex(pattern);

            if (string.IsNullOrEmpty(str))
            {
                return new ValidationResult(false, "The field cannot be empty!");

            }
            else if(!Regex.Match(str, pattern).Success)
            {
                return new ValidationResult(false, "The value is not in IPV4 format");
            }
            return new ValidationResult(true, null);
        }
    }
}
