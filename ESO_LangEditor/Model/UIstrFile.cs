using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using System.ComponentModel;

namespace ESO_Lang_Editor.Model
{
    public class UIstrFile : INotifyPropertyChanged
    {
        private string _UI_Table { get; set; }

        private string _UI_ID { get; set; }

        private string _UI_EN { get; set; }

        private string _UI_ZH { get; set; }

        private int _UI_Version { get; set; }

        private int _RowStats { get; set; }

        private int _isTranslated { get; set; }

        private string _UpdateStats { get; set; }

        public string UI_Table
        {
            get { return _UI_Table; }
            set
            {
                _UI_Table = value;
                NotifyPropertyChanged("UI_Table");
            }
        }

        public string UI_ID
        {
            get { return _UI_ID; }
            set
            {
                _UI_ID = value;
                NotifyPropertyChanged("UI_ID");
            }
        }

        public string UI_EN
        {
            get { return _UI_EN; }
            set
            {
                _UI_EN = value;
                NotifyPropertyChanged("UI_EN");
            }
        }

        public string UI_ZH
        {
            get { return _UI_ZH; }
            set
            {
                _UI_ZH = value;
                NotifyPropertyChanged("UI_ZH");
            }
        }

        public int UI_Version
        {
            get { return _UI_Version; }
            set
            {
                _UI_Version = value;
                NotifyPropertyChanged("UI_Version");
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

        public int isTranslated
        {
            get { return _isTranslated; }
            set
            {
                _isTranslated = value;
                NotifyPropertyChanged("isTranslated");
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
