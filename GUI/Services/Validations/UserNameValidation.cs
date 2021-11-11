using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GUI.Services.Validations
{
    public class UserNameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(false, "字段不得为空！");
            }
            else if (value.ToString().Length > 15)
            {
                return new ValidationResult(false, "用户名不得大于15位");
            }
            else if (value.ToString().Length < 5)
            {
                return new ValidationResult(false, "用户名不得小于5位");
            }
            else if (!Regex.IsMatch(value.ToString(), @"^[a-zA-Z0-9]+$"))
            {
                return new ValidationResult(false, "用户名仅允许英文与数字");
            }
            else
            {
                return ValidationResult.ValidResult;
            }

            //return string.IsNullOrWhiteSpace((value ?? "").ToString())
            //    ? new ValidationResult(false, "字段不得为空！")
            //    : ValidationResult.ValidResult;
        }
    }
}
