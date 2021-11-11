using Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class LangTextForCreationDto : LangTextDto
    {
        public ReviewReason ReasonFor { get; set; }
    }
}
