using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.View.UserControls;
using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Models.Client.Enum;
using ESO_LangEditorLib.Services.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class ImportTranslateWindowViewModel : BaseViewModel
    {
        private Dictionary<string, string> _fileList = new Dictionary<string, string>();
        private KeyValuePair<string, string> _selectedItemFilePath;
        private string _fileListCount;
        private string _fileSelectedInfo;
        private string _importFileCount;
        private bool _ImportAllFileCheckbox;

        public KeyValuePair<string, string> SelectedItemFilePath
        {
            get { return _selectedItemFilePath; }
            set { _selectedItemFilePath = value; NotifyPropertyChanged(); }
        }


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

        public bool ImportAllFileCheckbox
        {
            get { return _ImportAllFileCheckbox; }
            set { _ImportAllFileCheckbox = value; NotifyPropertyChanged(); }
        }

        private ParseLangFile parseLangFile = new ParseLangFile();

        public ICommand ImportTraslate => new ExcuteViewModelMethod(ImportLangs);
        public ImportTranslateOpenFileCommand OpenFileCommand { get; }
        public UC_LangDataGrid LangDataGrid { get; }

        public ImportTranslateWindowViewModel(UC_LangDataGrid langdatagrid)
        {

            LangDataGrid = langdatagrid;

            //ImportTraslate = new ImportTranslateCommand(this);
            OpenFileCommand = new ImportTranslateOpenFileCommand(this);
            //FileList = new Dictionary<string, string>();
        }

        private void ImportLangs(object o)
        {
            
            if (ImportAllFileCheckbox)
            {
                foreach (var path in FileList)
                {
                    if (path.Key.EndsWith(".json"))
                    {
                        ImportDataToDb(parseLangFile.JsonToDtoReader(path.Key));
                    }    
                        
                    //else
                    //    importData = parseLangFile.LangTextReaderToListAsync(path.Key);
                }
            }
            else
            {
                if (SelectedItemFilePath.Key.EndsWith(".json"))
                    ImportDataToDb(parseLangFile.JsonToDtoReader(SelectedItemFilePath.Key));
                //else
                //    importData = parseLangFile.LangTextReaderToListAsync(SelectedItemFilePath);
            }
        }

        private void ImportDataToDb(JsonDto json)
        {
            var db = new LangTextRepository();

            switch (json.ChangeType)
            {
                case LangChangeType.Added:
                    db.AddNewLangs(json.LangTexts);
                    break;
                case LangChangeType.ChangedEN:
                    db.UpdateLangsEN(json.LangTexts);
                    break;
                case LangChangeType.ChangedZH:
                    db.UpdateLangsZH(json.LangTexts);
                    break;
                case LangChangeType.Removed:
                    db.DeleteLangs(json.LangTexts);
                    break;
            }
        }
    }
}
