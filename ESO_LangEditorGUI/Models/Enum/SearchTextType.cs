using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ESO_LangEditorGUI.Models.Enum
{
    public enum SearchTextType : ushort
    {
        [Description("文本Guid")]
        Guid = 1,
        [Description("文本唯一ID")]
        UniqueID = 2,
        [Description("类型")]
        Type = 3,
        [Description("英文")]
        TextEnglish = 4,
        [Description("译文")]
        TextChineseS = 5,
        //[Description("搜译文")]
        //TextVersion = 6,
        [Description("已翻译条目")]
        TranslateStatus = 7,
        [Description("游戏版本")]
        UpdateStatus = 8,
        [Description("审核状态")]
        ReviewStatus = 9,
        [Description("用户")]
        ByUser = 10,
    }
}
