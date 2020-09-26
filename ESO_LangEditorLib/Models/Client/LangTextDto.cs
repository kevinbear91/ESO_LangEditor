using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESO_LangEditorGUI.Model
{
    public class LangTextDto1: INotifyPropertyChanged
    {
        private Guid id;
        private string textId;
        private int idType;
        private string textEn;
        private string textZh;
        private int isTranslated;
        private string updateStats;
        private DateTime enLastModifyTimestamp;
        private DateTime zhLastModifyTimestamp;
        private Guid userId;

        public Guid Id
        {
            get { return id; }

            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string TextId 
        {
            get { return textId; }

            set
            {
                textId = value;
                OnPropertyChanged("TextId");
            }
        }

        public int IdType 
        {
            get { return idType; }

            set
            {
                idType = value;
                OnPropertyChanged("IdType");
            }
        }

        public string TextEn 
        {
            get { return textEn; }

            set
            {
                textEn = value;
                OnPropertyChanged("TextEn");
            }
        }

        public string TextZh 
        {
            get { return textZh; }

            set
            {
                textZh = value;
                OnPropertyChanged("TextZh");
            }
        }

        public int IsTranslated 
        {
            get { return isTranslated; }

            set
            {
                isTranslated = value;
                OnPropertyChanged("IsTranslated");
            }
        }

        public string UpdateStats 
        {
            get { return updateStats; }

            set
            {
                updateStats = value;
                OnPropertyChanged("UpdateStats");
            }
        }

        public DateTime EnLastModifyTimestamp 
        {
            get { return enLastModifyTimestamp; }

            set
            {
                enLastModifyTimestamp = value;
                OnPropertyChanged("EnLastModifyTimestamp");
            }
        }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-DD,HH:MM}")]
        public DateTime ZhLastModifyTimestamp 
        {
            get { return zhLastModifyTimestamp; }

            set
            {
                zhLastModifyTimestamp = value;
                OnPropertyChanged("ZhLastModifyTimestamp");
            }
        }

        public Guid UserId 
        {
            get { return userId; }

            set
            {
                userId = value;
                OnPropertyChanged("UserId");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
