using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace ESO_LangEditorLib.DatabaseModels
{
    /// <summary>
    /// 创建LangData数据表
    /// 所有字段不允许为空
    /// 每个字段的定义查看 Models/LangData.cs 类
    /// </summary>
    [SugarTable("lang_data")]
    public class LangDataTableCreate
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string UniqueID { get; set; }
        
        [SugarColumn(IsNullable = false)]
        public int ID { get; set; }
        
        [SugarColumn(IsNullable = false)]
        public int Unknown { get; set; }
        
        [SugarColumn(IsNullable = false)]
        public int Lang_Index { get; set; }
        
        [SugarColumn(IsNullable = false)]
        public string EN_Text { get; set; }
        
        [SugarColumn(IsNullable = false)]
        public string ZH_Text { get; set; }
        
        [SugarColumn(IsNullable = false)]
        public string UpdateStats { get; set; }

        [SugarColumn(IsNullable = false)]
        public int IsTranslated { get; set; }

        [SugarColumn(IsNullable = false)]
        public int RowStats { get; set; }
    }
}
