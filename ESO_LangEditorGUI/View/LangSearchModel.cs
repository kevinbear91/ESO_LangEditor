using System.ComponentModel;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// <para>string ID_Table 表名</para>
    /// <para>int ID_Type 为文本ID</para>
    /// <para>int ID_Unknow 为文本位置列</para>
    /// <para>int ID_Index 为索引列</para>
    /// <para>string Text_EN 为英语原文</para>
    /// <para>string Text_SC 为译文</para>
    /// <para>int isTranslated, 0 - 未翻译， 1 - 已翻译， 2 - 导入的已翻译文本， 3 - 已修改内容已翻译</para>
    /// <para>int RowStats, 0 - 原始存在， 10 - 新增内容， 20 - 已修改内容， 30 - 已删除内容， 40 - 修改前的内容</para>
    /// <para>string UpdateStats, 哪个版本做出的修改</para>
    /// </summary>
    public class LangSearchModel : INotifyPropertyChanged
    {
        private string _ID_Table;
        private int _ID_IndexDB;
        private int _ID_Type;
        private int _ID_Unknown;
        private int _ID_Index;
        private string _Text_EN;
        private string _Text_SC;
        private int _isTranslated;
        private int _RowStats;
        private string _UpdateStats;


        public string ID_Table
        {
            get { return _ID_Table; }
            set
            {
                _ID_Table = value;
                NotifyPropertyChanged("ID_Table");
            }
        }

        public int IndexDB
        {
            get { return _ID_IndexDB; }
            set
            {
                _ID_IndexDB = value;
                NotifyPropertyChanged("IndexDB");
            }
        }

        public int ID_Type
        {
            get { return _ID_Type; }
            set
            {
                _ID_Type = value;
                NotifyPropertyChanged("ID_Type");
            }
        }

        public int ID_Unknown
        {
            get { return _ID_Unknown; }
            set
            {
                _ID_Unknown = value;
                NotifyPropertyChanged("ID_Type");
            }
        }

        public int ID_Index
        {
            get { return _ID_Index; }
            set
            {
                _ID_Index = value;
                NotifyPropertyChanged("ID_Index");
            }
        }

        public string Text_EN
        {
            get { return _Text_EN; }
            set
            {
                _Text_EN = value;
                NotifyPropertyChanged("Text_EN");
            }
        }

        public string Text_SC
        {
            get { return _Text_SC; }
            set
            {
                _Text_SC = value;
                NotifyPropertyChanged("Text_SC");
            }
        }
        public int isTranslated
        {
            get { return _isTranslated; }
            set
            {
                _isTranslated = value;
                NotifyPropertyChanged("isTranslated");
            }
        }

        public int RowStats
        {
            get { return _RowStats; }
            set
            {
                _RowStats = value;
                NotifyPropertyChanged("RowStats");
            }
        }

        public string UpdateStats
        {
            get { return _UpdateStats; }
            set
            {
                _UpdateStats = value;
                NotifyPropertyChanged("UpdateStats");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
