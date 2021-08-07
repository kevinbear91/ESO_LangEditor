using System;
using System.Globalization;
using System.Windows.Controls;

namespace ESO_LangEditor.GUI.Services
{
    public class GuidValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return Guid.TryParse((string)value, out Guid x) & (string)value != Guid.Empty.ToString() ? ValidationResult.ValidResult
                : new ValidationResult(false, "请输入有效Guid！");
        }
    }
}
