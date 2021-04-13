using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.Views.UserControls;
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

        private LangFileParser parseLangFile = new LangFileParser();

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

        private async void ImportDataToDb(JsonFileDto json)
        {
            var db = new LangTextRepoClientService();
            IMapper mapper = App.Mapper;

            switch (json.ChangeType)
            {
                case LangChangeType.Added:
                    var listForAdd = mapper.Map<List<LangTextClient>>(json.LangTexts);
                    await db.AddLangtexts(listForAdd);
                    break;
                case LangChangeType.ChangedEN:
                    var listForEnChanged = mapper.Map<List<LangTextForUpdateEnDto>>(json.LangTexts);
                    await db.UpdateLangtextEn(listForEnChanged);
                    break;
                case LangChangeType.ChangedZH:
                    var listForZhChanged = mapper.Map<List<LangTextForUpdateZhDto>>(json.LangTexts);
                    await db.UpdateLangtextZh(listForZhChanged);
                    break;
                case LangChangeType.Removed:
                    var listForDelete = mapper.Map<List<LangTextClient>>(json.LangTexts);
                    await db.DeleteLangtexts(listForDelete);
                    break;
            }
        }
    }
}
