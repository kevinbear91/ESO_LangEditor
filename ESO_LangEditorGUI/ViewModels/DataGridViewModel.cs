using ESO_LangEditor.Core.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorGUI.ViewModels
{
    public class DataGridViewModel : BindableBase
    {
        private List<LangTextDto> _gridData;
        private LangTextDto _gridSelectedItem;
        private List<LangTextDto> _gridSelectedItems;

        public List<LangTextDto> GridData
        {
            get { return _gridData; }
            set { SetProperty(ref _gridData, value); }
        }

        public LangTextDto GridSelectedItem { get; set; }

        public List<LangTextDto> GridSelectedItems { get; set; }

        
    }
}
