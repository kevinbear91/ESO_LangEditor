using ESO_LangEditor.Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class LangTextForReviewZhDto
    {
        public Guid UserId { get; set; }
        public Guid ReviewerId { get; set; }
        public ReviewReason ReviewReasonFor { get; set; }
    }
}
