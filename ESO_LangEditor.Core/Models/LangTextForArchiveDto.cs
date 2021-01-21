using ESO_LangEditor.Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class LangTextForArchiveDto
    {
        public Guid UserId { get; set; }
        public DateTime ArchiveTimestamp { get; set; }
        public ReviewReason ReasonFor { get; set; }
    }
}
