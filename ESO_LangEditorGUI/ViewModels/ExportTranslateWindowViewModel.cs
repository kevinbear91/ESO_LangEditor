using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.View.UserControls;
using ESO_LangEditorModels;
using ESO_LangEditorModels.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class ExportTranslateWindowViewModel : BaseViewModel
    {
        private string _searchResultInfo;
        private string _selectedInfo;
        private List<LangTextDto> _selectedItems;
        private string _mdNotifyContent;

        public UC_LangDataGrid LangDataGrid { get; set; }
        public ICommand ExportTranslateCommand { get; }


        public string SearchResultInfo
        {
            get { return _searchResultInfo; }
            set { _searchResultInfo = "总计搜索到 " + value + " 条结果。"; NotifyPropertyChanged(); }
        }

        public string SelectedInfo
        {
            get { return _selectedInfo; }
            set { _selectedInfo = "已选择 " + value + " 条文本。"; NotifyPropertyChanged(); }
        }

        public List<LangTextDto> SelectedItems
        {
            get { return _selectedItems; }
            set { _selectedItems = value; NotifyPropertyChanged(); }
        }

        public string MdNotifyContent
        {
            get { return _mdNotifyContent; }
            set { _mdNotifyContent = value; NotifyPropertyChanged(); }
        }

        public bool IsExportSelectedItems { get; set; }

        public ExportTranslateWindowViewModel(UC_LangDataGrid langdatagrid)
        {
            LangDataGrid = langdatagrid;
            LangDataGrid.exportTranslateWindowViewModel = this;
            LangDataGrid.LangDatGridinWindow = LangDataGridInWindow.ExportTranslateWindow;

            ExportTranslateCommand = new ExportTranslateCommand(langdatagrid, this);
        }


        //private void SearchTranslatedLangs()
        //{

        //}
    }
}
