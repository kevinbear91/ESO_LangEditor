using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditorServer.Models
{
    public class LangLuaDto
    {
        public Guid Id { get; set; }
        public string TextId { get; set; }
        public string TextEn { get; set; }
        public string TextZh { get; set; }
        public int IsTranslated { get; set; }
        public int DataEnum { get; set; }
        public string UpdateStats { get; set; }
        public Guid UserId { get; set; }
    }
}
