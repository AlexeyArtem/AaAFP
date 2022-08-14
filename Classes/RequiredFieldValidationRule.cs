using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AaAFP2
{
    class RequiredFieldValidationRule : ValidationRule
    {
        public RequiredFieldValidationRule() 
        {
            TextError = "Поле должно быть заполнено";
        }

        public string TextError { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null && value.ToString().Length > 0)
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, TextError);
            }
        }
    }
}
