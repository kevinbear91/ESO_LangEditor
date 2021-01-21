using ESO_LangEditor.Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class LangTextRevisedDto
    {
        public Guid LangtextID { get; set; }
        public int LangTextRevNumber { get; set; }
        public ReviewReason ReasonFor { get; set; }
    }
}
