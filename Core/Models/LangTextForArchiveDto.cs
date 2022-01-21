using Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class LangTextForArchiveDto : LangTextDto
    {
        public DateTime ArchiveTimestamp { get; set; }
        public ReviewReason ReasonFor { get; set; }
    }
}
