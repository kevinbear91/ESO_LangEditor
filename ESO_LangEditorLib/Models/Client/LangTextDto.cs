using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ESO_LangEditorLib.Models.Client
{
    public class LangTextDto : BaseDataModelClient
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

        public Guid Id 
        { 
            get { return _id; } 
            set { _id = value; NotifyPropertyChanged(); } 
        }

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

        public LangType LangType
        {
            get { return _langType; }
            set { _langType = value; NotifyPropertyChanged(); }
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

        //public override string ToString()
        //{
        //    //return ;
        //}
    }
}
