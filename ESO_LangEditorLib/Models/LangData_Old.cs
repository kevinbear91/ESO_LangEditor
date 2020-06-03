using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESO_LangEditorLib.Models
{
    
    public class LangData_Old
    {

        //ID列
        //[SugarColumn(IsNullable = false)]
        [Column("ID_Type")]
        public int ID { get; set; }

        //未知列
        //[SugarColumn(IsNullable = false)]
        [Column("ID_Unknown")]
        public int Unknown { get; set; }

        //索引列，加Lang_避免和数据库内保留字段冲突
        //ID+Unknown+Index 为游戏内唯一文本ID。
        //[SugarColumn(IsNullable = false)]
        [Column("ID_Index")]
        public int Index { get; set; }

        //英文文本
        //[SugarColumn(IsNullable = false, ColumnDataType = "TEXT")]
        [Column("Text_EN", TypeName = "TEXT")]
        public string Text_EN { get; set; }

        //汉化文本（未翻译为英文原文）
        //[SugarColumn(IsNullable = false, ColumnDataType = "TEXT")]
        [Column("Text_SC", TypeName = "TEXT")]
        public string Text_SC { get; set; }

        //更新状态，基本为版本号名字, 例如Update26
        //[SugarColumn(IsNullable = false, ColumnDataType = "TEXT")]
        [Column("UpdateStats", TypeName = "TEXT")]
        public string UpdateStats { get; set; }

        //是否翻译标记 -- 
        // 0 = 未翻译或初始内容
        // 1 = 已翻译（导出标记）
        // 2 = 导入的已翻译内容
        //[SugarColumn(IsNullable = false)]
        public int isTranslated { get; set; }

        //行状态 
        // 1 = 新增与初始内容
        // 2 = 修改过内容（EN_Text）不管是否之前订正过翻译 
        // 3 = 修改过内容（EN_Text）已订正翻译（ZH_Text）
        //[SugarColumn(IsNullable = false)]
        public int RowStats { get; set; }


    }
    public class LangOldDataTable
    {
        [Column("name", TypeName = "TEXT")]
        public string name { get; set; }
    }
}
