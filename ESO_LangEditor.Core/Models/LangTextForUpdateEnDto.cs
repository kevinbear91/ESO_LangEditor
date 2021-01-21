using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class LangTextForUpdateEnDto
    {
        public string TextEn { get; set; }

        public byte IsTranslated { get; set; }

        public DateTime EnLastModifyTimestamp { get; set; }

        public string UpdateStats { get; set; }

        public Guid UserId { get; set; }
    }
}
