using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class LangTextForUpdateZhDto
    {

        public string TextZh { get; set; }

        public byte IsTranslated { get; set; }

        public DateTime ZnLastModifyTimestamp { get; set; }

        public Guid UserId { get; set; }
    }
}
