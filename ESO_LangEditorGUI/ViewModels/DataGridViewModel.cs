using ESO_LangEditorLib.Models.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorGUI.ViewModels
{
    public class DataGridViewModel : BaseViewModel
    {
        private List<LangTextDto> _gridData;
        private LangTextDto _selectedData;
        private List<LangTextDto> _gridSelectedItems;

        public List<LangTextDto> GridData
        {
            get { return _gridData; }
            set { _gridData = value; NotifyPropertyChanged(); }
        }

        public LangTextDto SelectedData
        {
            get { return _selectedData; }
            set { _selectedData = value; NotifyPropertyChanged(); }
        }

        public List<LangTextDto> GridSelectedItem
        {
            get { return _gridSelectedItems; }
            set { _gridSelectedItems = value; NotifyPropertyChanged(); }
        }

        
    }
}
