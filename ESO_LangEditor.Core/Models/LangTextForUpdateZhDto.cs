using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class LangTextForUpdateZhDto
    {
        public Guid Id { get; set; }

        public string TextZh { get; set; }

        public byte IsTranslated { get; set; }

        public DateTime ZhLastModifyTimestamp { get; set; }

        public Guid UserId { get; set; }

    }
}
