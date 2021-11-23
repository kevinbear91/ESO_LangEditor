using Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class LangtextBase
    {
        //ID+Unknown+Index 为游戏内唯一文本ID。
        public string TextId { get; set; }

        //文本类型，Lua统一为100。
        public int IdType { get; set; }
        [ForeignKey("IdType")]
        public LangTypeCatalog LangCatalog { get; set; }    

        //英文文本
        public string TextEn { get; set; }

        //中文文本
        public string TextZh { get; set; }

        //文本类型
        public LangType LangTextType { get; set; }

        //英文加入或修改时的版本
        public int GameApiVersion { get; set; }

        [ForeignKey("GameApiVersion")]
        public GameVersion GameVersion { get; set; }

        //英文最后修改时间戳
        public DateTime EnLastModifyTimestamp { get; set; }

        //中文最后修改时间戳
        public DateTime ZhLastModifyTimestamp { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User UserForModify { get; set; }

        public Guid ReviewerId { get; set; }
        [ForeignKey("ReviewerId")]
        public User UserForReview { get; set; }

        //文本修订号
        public int Revised { get; set; }
    }
}
