using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESO_LangEditorLib.Models
{
    public class LuaUIDataOld
    {
        [Column(TypeName = "TEXT")]
        public string UI_ID { get; set; }
        [Column(TypeName = "TEXT")]
        public string UI_EN { get; set; }
        [Column(TypeName = "TEXT")]
        public string UI_ZH { get; set; }

        public int UI_Version { get; set; }

        public int RowStats { get; set; }

        public int isTranslated { get; set; }

        [Column(TypeName = "TEXT")]
        public string UpdateStats { get; set; }

    }
    public class LuaUIDataOldTable
    {
        [Column("name", TypeName = "TEXT")]
        public string name { get; set; }
    }
}
