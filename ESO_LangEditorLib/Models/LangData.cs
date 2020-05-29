using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace ESO_LangEditorLib.Models
{
    [SugarTable("lang_data")]
    public class LangData
    {

        //主键
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string UniqueID { get; set; }
        //ID列
        [SugarColumn(IsNullable = false)]
        public int ID { get; set; }

        //未知列
        [SugarColumn(IsNullable = false)]
        public int Unknown { get; set; }

        //索引列，加Lang_避免和数据库内保留字段冲突
        //ID+Unknown+Index 为游戏内唯一文本ID。
        [SugarColumn(IsNullable = false)]
        public int Lang_Index { get; set; }

        //英文文本
        [SugarColumn(IsNullable = false)]
        public string EN_Text { get; set; }

        //汉化文本（未翻译为英文原文）
        [SugarColumn(IsNullable = false)]
        public string ZH_Text { get; set; }

        //更新状态，基本为版本号名字
        [SugarColumn(IsNullable = false)]
        public string UpdateStats { get; set; }

        //是否翻译标记 -- 
        // 0 = 未翻译或初始内容
        // 1 = 已翻译（导出标记）
        // 2 = 导入的已翻译内容
        [SugarColumn(IsNullable = false)]
        public int IsTranslated { get; set; }

        //行状态 
        // 1 = 新增与初始内容
        // 2 = 修改过内容（EN_Text）不管是否之前订正过翻译 
        // 3 = 修改过内容（EN_Text）已订正翻译（ZH_Text）
        [SugarColumn(IsNullable = false)]
        public int RowStats { get; set; }
    }
}
