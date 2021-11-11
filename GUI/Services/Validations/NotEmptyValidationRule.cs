using System.Globalization;
using System.Windows.Controls;

namespace GUI.Services.Validations
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "字段不得为空！")
                : ValidationResult.ValidResult;
        }
    }
}
