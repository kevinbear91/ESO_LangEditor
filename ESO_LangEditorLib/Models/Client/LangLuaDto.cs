using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorLib.Models.Client
{
    //[Table("lua_str_ui")]
    public class LangLuaDto : BaseDataModelClient
    {
        private Guid _id;
        private string _LuaId;
        private string _textEn;
        private string _textZh;
        private LangType _luaType;
        private int _isTranslated;
        private string _updateStats;
        private DateTime _enLastModifyTimestamp;
        private DateTime _zhLastModifyTimestamp;
        private Guid _userId;


        public Guid Id
        {
            get { return _id; }
            set { _id = value; NotifyPropertyChanged(); }
        }

        public string LuaID
        {
            get { return _LuaId; }
            set { _LuaId = value; NotifyPropertyChanged(); }
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

        public LangType LuaType
        {
            get { return _luaType; }
            set { _luaType = value; NotifyPropertyChanged(); }
        }

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


        //[Key]
        //[Column(TypeName = "TEXT")]
        //public string UniqueID { get; set; }

        ////[Column(TypeName = "TEXT")]
        //public string Text_EN { get; set; }

        ////[Column(TypeName = "TEXT")]
        //public string Text_ZH { get; set; }


        //属于哪个数据表
        //1 - PreGame
        //2 - Client
        //3 - 两者皆有
        //0 - Error
        //public int DataEnum { get; set; }

        ////[Column(TypeName = "TEXT")]
        //public string UpdateStats { get; set; }

        //public int IsTranslated { get; set; }

        //public int RowStats { get; set; }
    }
}
