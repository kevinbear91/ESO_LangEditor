using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorLib.Models.Client
{
    public class JsonDto
    {
        public List<LangTextDto> LangTexts { get; set; }
        //public List<LangLuaDto> LangLuas { get; set; }
        public string Version { get; set; }
        public DateTime ExportTime { get; set; }
        //List<>
    }
}
