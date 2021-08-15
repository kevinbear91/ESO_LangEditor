using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESO_LangEditor.Core.Entities
{
    public class GameVersion
    {
        [Key]
        public int GameApiVersion { get; set; }
        public string Version_EN { get; set; }
        public string Version_ZH { get; set; }
    }
}
