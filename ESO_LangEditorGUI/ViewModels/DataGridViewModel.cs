using ESO_LangEditorLib.Models.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorGUI.ViewModels
{
    public class DataGridViewModel : BaseViewModel
    {
        private List<LangTextDto> _gridData;

        public List<LangTextDto> GridData
        {
            get { return _gridData; }
            set { _gridData = value; NotifyPropertyChanged(); }
        }
    }
}
