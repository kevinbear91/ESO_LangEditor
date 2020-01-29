using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ESO_Lang_Editor.Model;

namespace ESO_Lang_Editor.View
{
    public class LangSearchModel : INotifyPropertyChanged
    {
        private string _ID_Table;
        private int _ID_IndexDB;
        private string _ID_Type;
        private int _ID_Unknown;
        private int _ID_Index;
        private string _Text_EN;
        private string _Text_SC;
        private int _isTranslated;

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

        public string ID_Type
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

        /*
        public List<LangSearchModel> SearchLang(string SearchBarText)
        {
            var DBFile = new SQLiteController();

            var lang = DBFile.SearchData(SearchBarText);

            //foreach (var data in LangSearch)
            //{
            //    Debug.WriteLine(data.Text_EN);
            //}

            return lang;
        }
        */

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
