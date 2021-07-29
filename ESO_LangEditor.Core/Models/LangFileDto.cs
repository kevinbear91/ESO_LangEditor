using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class LangFileDto
    {
        public int Id { get; set; }
        public int Unknown { get; set; }
        public int Index { get; set; }
        public int Offset { get; set; }
        public string Text { get; set; }
    }
}
