using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SNN.Validation
{
    public class MembranePotentialValidationRule : ValidationRule
    {
        public ValidationParametersMP Parameters { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
           // var stringValue = value as string;
            if (double.TryParse((string)value, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                if (Parameters.SelectedConnectionType == -1 && result >= -1 && result < 0)
                {
                    return ValidationResult.ValidResult;
                }
                else if (Parameters.SelectedConnectionType == 1 && result >= 0 && result <= Parameters.MinValue)
                {
                    return ValidationResult.ValidResult;
                }
            }
            return new ValidationResult(false, $"Недопустимое значение мембранного потенциала. Допустимые диапазоны: " +
                                                $"{(Parameters.SelectedConnectionType == -1 ? "[-1, 0)" : $"[0, {Parameters.MinValue})")}.");
        }
    }
}
