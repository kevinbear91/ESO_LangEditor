using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EnumTypes
{
    public enum ProcessDbUpdateResult : byte
    {
        Success = 1,
        UnableToFindDbFile,
        UnableToExportLangText,
        UnableToExportLangLua,
    }
}
