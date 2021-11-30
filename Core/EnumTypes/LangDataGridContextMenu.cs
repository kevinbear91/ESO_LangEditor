using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.EnumTypes
{
    public enum LangDataGridContextMenu : ushort
    {
        [Description("编辑当前项")]
        EditCurrentItem = 1,
        [Description("编辑已选项")]
        EditMutilItem = 2,
        [Description("查找替换已选项")]
        SearchAndReplace = 3,
        //[Description("删除已选择待审核文本")]
        //DeleteUnderReview = 4,
    }
}
