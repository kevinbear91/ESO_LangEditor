using ESO_LangEditor.Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class LangTextForCreationDto : LangTextDto
    {
        public Guid ReviewerId { get; set; }
        public DateTime ReviewTimestamp { get; set; }
        public ReviewReason ReasonFor { get; set; }
    }
}
