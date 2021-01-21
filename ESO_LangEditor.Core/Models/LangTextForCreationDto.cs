using ESO_LangEditor.Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class LangTextForCreationDto
    {
        public string TextId { get; set; }

        //文本类型，Lua统一为100。
        public int IdType { get; set; }

        //英文文本
        public string TextEn { get; set; }

        //中文文本
        public string TextZh { get; set; }

        //文本类型
        public LangType LangTextType { get; set; }

        //英文加入或修改时的版本
        public string UpdateStats { get; set; }

        public Guid UserId { get; set; }
    }
}
