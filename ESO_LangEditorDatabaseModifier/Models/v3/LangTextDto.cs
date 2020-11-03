using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESO_LangEditorDatabaseModifier.Model.v3
{
    [Table("lang_texts")]
    public class LangTextDto_v3 : BaseDataModelClient
    {
        private Guid _id;
        private string _textId;
        private int _idType;
        private string _textEn;
        private string _textZh;
        private LangType _langType;
        private int _isTranslated;
        private string _updateStats;
        private DateTime _enLastModifyTimestamp;
        private DateTime _zhLastModifyTimestamp;
        private Guid _userId;

        [Key]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; NotifyPropertyChanged(); }
        }

        //ID+Unknown+Index 为游戏内唯一文本ID。
        public string TextId
        {
            get { return _textId; }
            set { _textId = value; NotifyPropertyChanged(); }
        }

        public int IdType
        {
            get { return _idType; }
            set { _idType = value; NotifyPropertyChanged(); }
        }

        public string TextEn
        {
            get { return _textEn; }
            set { _textEn = value; NotifyPropertyChanged(); }
        }

        public string TextZh
        {
            get { return _textZh; }
            set { _textZh = value; NotifyPropertyChanged(); }
        }

        public LangType LangLuaType
        {
            get { return _langType; }
            set { _langType = value; NotifyPropertyChanged(); }
        }


        //是否翻译标记 -- 
        // 0 = 未翻译或初始内容
        // 1 = 已翻译（导出标记）
        // 2 = 导入的已翻译内容
        public int IsTranslated
        {
            get { return _isTranslated; }
            set { _isTranslated = value; NotifyPropertyChanged(); }
        }

        public string UpdateStats
        {
            get { return _updateStats; }
            set { _updateStats = value; NotifyPropertyChanged(); }
        }

        public DateTime EnLastModifyTimestamp
        {
            get { return _enLastModifyTimestamp; }
            set { _enLastModifyTimestamp = value; NotifyPropertyChanged(); }
        }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-DD,HH:MM}")]
        public DateTime ZhLastModifyTimestamp
        {
            get { return _zhLastModifyTimestamp; }
            set { _zhLastModifyTimestamp = value; NotifyPropertyChanged(); }
        }

        public Guid UserId
        {
            get { return _userId; }
            set { _userId = value; NotifyPropertyChanged(); }
        }
    }
}
