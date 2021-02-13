using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESO_LangEditorDatabaseModifier.Model.v4
{
    public class LangTextRevNumber
    {
        [Key]
        public int Id { get; set; }
        public int LangTextRev { get; set; }
    }
}
