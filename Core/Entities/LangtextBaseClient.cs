using Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class LangtextBaseClient
    {
        //ID+Unknown+Index 为游戏内唯一文本ID。
        public string TextId { get; set; }

        //文本类型，Lua统一为100。
        public int IdType { get; set; }

        //英文文本
        public string TextEn { get; set; }

        //中文文本
        public string TextZh { get; set; }

        public string TextZh_Official { get; set; }

        //文本类型
        public LangType LangTextType { get; set; }

        //是否翻译标记 -- 
        // 0 = 未翻译或初始内容
        // 1 = 已翻译（导出标记）
        // 2 = 导入的已翻译内容
        //public byte IsTranslated { get; set; }

        //英文加入或修改时的版本
        //public string UpdateStats { get; set; }

        public int GameApiVersion { get; set; }

        //[ForeignKey("GameApiVersion")]
        //public GameVersion GameVersion { get; set; }

        //英文最后修改时间戳
        public DateTime EnLastModifyTimestamp { get; set; }

        //中文最后修改时间戳
        public DateTime ZhLastModifyTimestamp { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public UserInClient UserForModify { get; set; }

        public Guid ReviewerId { get; set; }
        [ForeignKey("ReviewerId")]
        public UserInClient UserForReview { get; set; }

        //文本修订号
        public int Revised { get; set; }

    }
}
