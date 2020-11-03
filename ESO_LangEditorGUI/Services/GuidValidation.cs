using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace ESO_LangEditorGUI.Services
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
