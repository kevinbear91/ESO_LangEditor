using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESO_LangEditorLib.Models
{
    [Table("lua_str_ui")]
    public class LuaUIData
    {
        [Key]
        [Column(TypeName = "TEXT")]
        public string UniqueID { get; set; }

        [Column(TypeName = "TEXT")]
        public string Text_EN { get; set; }

        [Column(TypeName = "TEXT")]
        public string Text_ZH { get; set; }


        //属于哪个数据表
        //1 - PreGame
        //2 - Client
        //3 - 两者皆有
        //0 - Error
        public int DataEnum { get; set; }

        [Column(TypeName = "TEXT")]
        public string UpdateStats { get; set; }

        public int IsTranslated { get; set; }

        public int RowStats { get; set; }
    }
}
