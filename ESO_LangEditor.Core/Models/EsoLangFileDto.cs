using ESO_LangEditor.Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class EsoLangFileDto
    {
        public uint Id { get; set; }
        public uint Uknown { get; set; }
        public uint Index { get; set; }
        public uint Offset { get; set; }
        public string Text { get; set; }
        public LangType LangTextType { get; set; }
    }
}
