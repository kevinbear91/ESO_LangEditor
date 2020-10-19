using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorGUI.ViewModels
{
    public class ProgressDialogViewModel : BaseViewModel
    {
        private string _currentExcuteText;


        public string CurrentExcuteText
        {
            get { return _currentExcuteText; }
            set { _currentExcuteText = value; NotifyPropertyChanged(); }
        }

    }
}
