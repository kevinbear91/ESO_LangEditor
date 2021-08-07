using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.Services;
using ESO_LangEditor.GUI.Views;
using Microsoft.Win32;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ESO_LangEditor.GUI.ViewModels
{
    public class ImportTranslateViewModel : BindableBase
    {
        private ObservableCollection<string> _fileList;
        private string _selectedItemFilePath;
        private string _fileListCount;
        private string _fileSelectedInfo;
        private string _importFileCount;
        private bool _ImportAllFileCheckbox;
        private ObservableCollection<LangTextDto> _gridData;
        private Dictionary<string, string> _files = new Dictionary<string, string>();

        public string SelectedItemFilePath
        {
            get { return _selectedItemFilePath; }
            set { SetProperty(ref _selectedItemFilePath, value); }
        }


        public string FileListCount
        {
            get { return _fileListCount; }
            set { SetProperty(ref _fileListCount, "共有 " + _fileList.Count + " 项"); }
        }

        public string FileSelectedInfo
        {
            get { return _fileSelectedInfo; }
            set { SetProperty(ref _fileSelectedInfo, "已选择 " + value + " 项"); }
        }

        public string ImportFileCount
        {
            get { return _importFileCount; }
            set { SetProperty(ref _importFileCount, value); }
        }

        public ObservableCollection<string> FileList
        {
            get { return _fileList; }
            set { SetProperty(ref _fileList, value); }
        }

        public bool ImportAllFileCheckbox
        {
            get { return _ImportAllFileCheckbox; }
            set { SetProperty(ref _ImportAllFileCheckbox, value); }
        }

        public ObservableCollection<LangTextDto> GridData
        {
            get { return _gridData; }
            set { SetProperty(ref _gridData, value); }
        }

        private LangFileParser parseLangFile = new LangFileParser();

        public ICommand ImportTraslate => new ExcuteViewModelMethod(ImportLangs);
        public ICommand OpenFileCommand => new ExcuteViewModelMethod(ParseJson);

        public ImportTranslateViewModel()
        {
            FileList = new ObservableCollection<string>();
            GridData = new ObservableCollection<LangTextDto>();
        }

        private void ImportLangs(object o)
        {

            if (ImportAllFileCheckbox)
            {
                foreach (var path in _files)
                {
                    if (path.Key.EndsWith(".json"))
                    {
                        ImportDataToDb(parseLangFile.JsonToDtoReader(path.Value));
                    }

                    //else
                    //    importData = parseLangFile.LangTextReaderToListAsync(path.Key);
                }
            }
            else
            {
                if (_files.TryGetValue(SelectedItemFilePath, out string jsonpath))
                {
                    Debug.WriteLine("json path: " + jsonpath);
                    ImportDataToDb(parseLangFile.JsonToDtoReader(jsonpath));
                }

                //else
                //    importData = parseLangFile.LangTextReaderToListAsync(SelectedItemFilePath);
            }
        }

        private async void ImportDataToDb(JsonFileDto json)
        {
            //var db = new LangTextRepoClientService();
            //IMapper mapper = App.Mapper;

            //switch (json.ChangeType)
            //{
            //    case LangChangeType.Added:
            //        var listForAdd = mapper.Map<List<LangTextClient>>(json.LangTexts);
            //        await db.AddLangtexts(listForAdd);
            //        break;
            //    case LangChangeType.ChangedEN:
            //        var listForEnChanged = mapper.Map<List<LangTextForUpdateEnDto>>(json.LangTexts);
            //        await db.UpdateLangtextEn(listForEnChanged);
            //        break;
            //    case LangChangeType.ChangedZH:
            //        var listForZhChanged = mapper.Map<List<LangTextForUpdateZhDto>>(json.LangTexts);
            //        await db.UpdateLangtextZh(listForZhChanged);
            //        break;
            //    case LangChangeType.Removed:
            //        var listForDelete = mapper.Map<List<LangTextClient>>(json.LangTexts);
            //        //await db.DeleteLangtexts(listForDelete);
            //        break;
            //}
        }

        private void ParseJson(object o)
        {
            ImportTranslate importTranslateWindow = o as ImportTranslate;

            OpenFileDialog dialog = new OpenFileDialog
            {
                Multiselect = true
            };

            if (dialog.ShowDialog(importTranslateWindow) == true)
            {
                if (dialog.FileName.EndsWith(".json") || dialog.FileName.EndsWith(".LangDB") || dialog.FileName.EndsWith(".LangUI") || dialog.FileName.EndsWith(".db") || dialog.FileName.EndsWith(".dbUI"))
                {
                    //int fileCount = _importTranslateWindowViewModel.FileList.Count;
                    if (_files.Count >= 1)
                    {
                        _files.Clear();
                    }

                    foreach (var file in dialog.FileNames)
                    {
                        _files.Add(Path.GetFileName(file), file);
                        //new ESO_LangEditorLib.Services.Client.ParseLangFile().JsonReader(file);
                    }

                    if (FileList.Count >= 1)
                    {
                        FileList.Clear();
                    }
                    else
                    {
                        FileList = new ObservableCollection<string>();
                    }

                    FileList.AddRange(_files.Keys);

                    //FileViewer.Items.Refresh();
                    //Debug.WriteLine(_importTranslateWindowViewModel.FileList.Count);
                    //FileID_listBox.ItemsSource = fileList;
                    //FileID_listBox.SelectedIndex = 0;

                    //textBlock_Info.Text = "共 " + filePath.Count + " 个文件";
                    //ImportToDB_button.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("仅支持读取 .LangDB、 .LangUI、.db、.dbUI 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    //FileID_listBox.ItemsSource = "";
                }
                //TotalFiles_textBlock.Text = "共 " + fileList.Count().ToString() + " 个文件，已选择 0 个。";
            }
        }

        public void ReadSelectedFile(string fileKey)
        {
            if (_files.TryGetValue(fileKey, out string jsonpath))
            {
                GridData.AddRange(parseLangFile.JsonToLangTextListReader(jsonpath));
            }
        }

    }
}
