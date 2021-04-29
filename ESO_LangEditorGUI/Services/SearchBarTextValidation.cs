using ESO_LangEditor.Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using static System.Convert;

namespace ESO_LangEditorGUI.Services
{
    [ContentProperty("ComparisonValue")]
    public class SearchBarTextValidation : ValidationRule
    {
        //https://stackoverflow.com/questions/3862385/wpf-validationrule-with-dependency-property/38353227#38353227
        //ValidationRule with Enum combox

        public ComparisonValue ComparisonValue { get; set; }

        //public SearchTextType ValidationType { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //Task.Delay(2000);
            //string strValue = Convert.ToString(value);

            if (string.IsNullOrWhiteSpace((string)value))
                return new ValidationResult(false, "不支持全局搜索，请输入文本。");

            bool canConvert = false;

            //throw new InvalidCastException($"{ComparisonValue.Value} is not supported");

            switch (ComparisonValue.Value)
            {

                case SearchTextType.Guid:
                    //bool boolVal = false;
                    canConvert = Guid.TryParse((string)value, out Guid x) & (string)value != Guid.Empty.ToString();
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, "请输入有效Guid！");
                case SearchTextType.UniqueID:
                    //int intVal = 0;
                    canConvert = string.IsNullOrWhiteSpace((value ?? "").ToString());
                    return canConvert ? new ValidationResult(false, "输入框不可为空！") : ValidationResult.ValidResult;
                case SearchTextType.Type:
                    //int intVal = 0;
                    canConvert = int.TryParse((string)value, out Int32 i);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, "此搜索条件必须输入数字！");
                case SearchTextType.TextEnglish:
                    //int intVal = 0;
                    canConvert = string.IsNullOrWhiteSpace((value ?? "").ToString());
                    return canConvert ? new ValidationResult(false, "输入框不可为空！") : ValidationResult.ValidResult;
                case SearchTextType.TextChineseS:
                    //double doubleVal = 0;
                    canConvert = string.IsNullOrWhiteSpace((value ?? "").ToString());
                    return canConvert ? new ValidationResult(false, "输入框不可为空！") : ValidationResult.ValidResult;
                case SearchTextType.TranslateStatus:
                    //int intVal = 0;
                    canConvert = int.TryParse((string)value, out Int32 t);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, "此搜索条件必须输入数字！");
                case SearchTextType.UpdateStatus:
                    //int intVal = 0;
                    canConvert = string.IsNullOrWhiteSpace((value ?? "").ToString());
                    return canConvert ? new ValidationResult(false, "输入框不可为空！") : ValidationResult.ValidResult;
                case SearchTextType.ByUser:
                    //bool boolVal = false;
                    canConvert = Guid.TryParse((string)value, out Guid u) & (string)value != Guid.Empty.ToString();
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, "请输入有效Guid！");
                //case "Int64":
                //    long longVal = 0;
                //    canConvert = long.TryParse(strValue, out longVal);
                //    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Int64");
                default:
                    throw new InvalidCastException($"{ComparisonValue.Value} is not supported");
            }

        }
    }
}
