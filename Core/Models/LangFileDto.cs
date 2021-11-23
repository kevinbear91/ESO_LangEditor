using Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class LangFileDto
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public int Unknown { get; set; }
        public int Index { get; set; }
        public int Offset { get; set; }
        public string TextEn { get; set; }
        public string TextZh { get; set; }
        public LangType LangTextType { get; set; }
    }
}
