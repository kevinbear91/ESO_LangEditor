using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ESO_LangEditor.Core.EnumTypes
{
    public enum SearchPostion : ushort
    {
        [Description("包含全文")]
        Full = 1,
        [Description("仅包含开头")]
        OnlyOnFront = 2,
        [Description("仅包含结尾")]
        OnlyOnEnd = 3,
    }
}
