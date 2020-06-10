using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESO_LangEditorLib.Models
{
    [Table("lang_data")]
    public class LangData
    {

        //主键
        //[SugarColumn(IsNullable = false, IsPrimaryKey = true, ColumnDataType = "TEXT")]
        [Key]
        [Column(TypeName = "TEXT")]
        public string UniqueID { get; set; }
        //ID列
        //[SugarColumn(IsNullable = false)]
        public int ID { get; set; }

        //未知列
        //[SugarColumn(IsNullable = false)]
        public int Unknown { get; set; }

        //索引列，加Lang_避免和数据库内保留字段冲突
        //ID+Unknown+Index 为游戏内唯一文本ID。
        //[SugarColumn(IsNullable = false)]
        public int Lang_Index { get; set; }

        //英文文本
        //[SugarColumn(IsNullable = false, ColumnDataType = "TEXT")]
        [Column(TypeName = "TEXT")]
        public string Text_EN { get; set; }

        //汉化文本（未翻译为英文原文）
        //[SugarColumn(IsNullable = false, ColumnDataType = "TEXT")]
        [Column(TypeName = "TEXT")]
        public string Text_ZH { get; set; }

        //更新状态，基本为版本号名字, 例如Update26
        //[SugarColumn(IsNullable = false, ColumnDataType = "TEXT")]
        [Column(TypeName = "TEXT")]
        public string UpdateStats { get; set; }

        //是否翻译标记 -- 
        // 0 = 未翻译或初始内容
        // 1 = 已翻译（导出标记）
        // 2 = 导入的已翻译内容
        //[SugarColumn(IsNullable = false)]
        public int IsTranslated { get; set; }

        //行状态
        // 0 = 初始内容
        // 1 = 新增内容
        // 2 = EN_Text修改过内容, 不管是否之前订正过翻译 
        // 3 = 已翻译的初始内容与新增内容
        // 4 = EN_Text修改过内容, ZH_Text已订正翻译
        //[SugarColumn(IsNullable = false)]
        public int RowStats { get; set; }

    }
    
}
