using ESO_LangEditor.Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class JsonFileDto
    {
        public List<LangTextDto> LangTexts { get; set; }
        public string Version { get; set; }
        public DateTime ExportTime { get; set; }
        public LangChangeType ChangeType { get; set; }
    }
}
