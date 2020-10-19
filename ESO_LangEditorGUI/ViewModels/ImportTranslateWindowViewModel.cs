using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.View.UserControls;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorGUI.ViewModels
{
    public class ImportTranslateWindowViewModel : BaseViewModel
    {
        private Dictionary<string, string> _fileList = new Dictionary<string, string>();
        private string _selectedItemFilePath;
        private string _fileListCount;
        private string _fileSelectedInfo;
        private string _importFileCount;

        public string FileListCount
        {
            get { return _fileListCount; }
            set { _fileListCount = "共有 " + _fileList.Count + " 项"; NotifyPropertyChanged(); }
        }

        public string FileSelectedInfo
        {
            get { return _fileSelectedInfo; }
            set { _fileSelectedInfo = "已选择 " + value + " 项"; NotifyPropertyChanged(); }
        }

        public string ImportFileCount
        {
            get { return _importFileCount; }
            set { _importFileCount = value; NotifyPropertyChanged(); }
        }

        public Dictionary<string, string> FileList 
        { 
            get { return _fileList; } 
            set { _fileList = value; NotifyPropertyChanged(); } 
        }

        public bool ImportAllFileCheckbox { get; set; }

        public ImportTranslateCommand ImportTraslate { get; }
        public ImportTranslateOpenFileCommand OpenFileCommand { get; }
        public UC_LangDataGrid LangDataGrid { get; }

        public ImportTranslateWindowViewModel(UC_LangDataGrid langdatagrid)
        {

            LangDataGrid = langdatagrid;

            ImportTraslate = new ImportTranslateCommand(this);
            OpenFileCommand = new ImportTranslateOpenFileCommand(this);
            //FileList = new Dictionary<string, string>();
        }
    }
}
