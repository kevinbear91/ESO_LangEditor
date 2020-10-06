using ESO_LangEditorLib.Models.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorGUI.ViewModels
{
    public class CompareWindowViewModel : BaseViewModel
    {
        private string _updateVersionText;

        public string UpdateVersionText
        {
            get { return _updateVersionText; }
            set { _updateVersionText = value; NotifyPropertyChanged(); }
        }

        public List<LangTextDto> Added { get; set; }

        public List<LangTextDto> Changed { get; set; }

        public List<LangTextDto> NonChanged { get; set; }

        public List<LangTextDto> RemovedList { get; set; }

        public Dictionary<string, LangTextDto> RemovedDict { get; set; }

        public List<string> FileList { get; set; }


    }
}
