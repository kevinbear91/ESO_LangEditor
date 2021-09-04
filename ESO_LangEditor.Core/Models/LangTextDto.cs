using ESO_LangEditor.Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class LangTextDto
    {
        public Guid Id { get; set; }
        public string TextId { get; set; }
        public int IdType { get; set; }
        public string TextEn { get; set; }
        public string TextZh { get; set; }
        public string? TextJp { get; set; }
        public LangType LangTextType { get; set; }
        public byte IsTranslated { get; set; }
        public string UpdateStats { get; set; }
        public DateTime EnLastModifyTimestamp { get; set; }
        public DateTime ZhLastModifyTimestamp { get; set; }
        public Guid UserId { get; set; }
        public Guid ReviewerId { get; set; }
        public int Revised { get; set; }
        public DateTime ReviewTimestamp { get; set; }
    }
}
