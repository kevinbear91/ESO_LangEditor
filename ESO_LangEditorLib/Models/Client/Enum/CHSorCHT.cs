using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ESO_LangEditorLib.Models.Client.Enum
{
    public enum CHSorCHT : byte
    {
        [Description("简体中文")]
        chs = 1,
        [Description("繁体中文")]
        cht,
    }
}
