using ESO_LangEditorModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorGUI.ViewModels
{
    public class DataGridViewModel : BaseViewModel
    {
        private List<LangTextDto> _gridData;
        private LangTextDto _gridSelectedItem;
        private List<LangTextDto> _gridSelectedItems;

        public List<LangTextDto> GridData
        {
            get { return _gridData; }
            set { _gridData = value; NotifyPropertyChanged(); }
        }

        public LangTextDto GridSelectedItem { get; set; }

        public List<LangTextDto> GridSelectedItems { get; set; }

        
    }
}
