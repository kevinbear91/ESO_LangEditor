using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.View.UserControls;
using ESO_LangEditorModels;
using ESO_LangEditorModels.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class CompareWindowViewModel : BaseViewModel
    {
        private string _updateVersionText;
        private string _addedTag;
        private string _changedTag;
        private string _removedTag;
        private string _fileCount;
        private string _loadButtonContent;
        private string _filePathTooltip;
        private string _infoText;
        private bool _updatedVersionInputEnable;
        private bool _saveButtonEnable;

        public string UpdateVersionText
        {
            get { return _updateVersionText; }
            set { _updateVersionText = value; NotifyPropertyChanged(); }
        }

        public string AddedTag
        {
            get { return _addedTag; }
            set { _addedTag = "新增(" + value + ")"; NotifyPropertyChanged(); }
        }

        public string ChangedTag
        {
            get { return _changedTag; }
            set { _changedTag = "修改(" + value + ")"; NotifyPropertyChanged(); }
        }

        public string RemovedTag
        {
            get { return _removedTag; }
            set { _removedTag = "移除(" + value + ")"; NotifyPropertyChanged(); }
        }

        public string FileCount
        {
            get { return _fileCount; }
            set { _fileCount = value ; NotifyPropertyChanged(); }
        }

        public string LoadButtonContent
        {
            get { return _loadButtonContent; }
            set { _loadButtonContent = value; NotifyPropertyChanged(); }
        }

        public string FilePathTooltip
        {
            get { return _filePathTooltip; }
            set { _filePathTooltip = value; NotifyPropertyChanged(); }
        }

        public string InfoText
        {
            get { return _infoText; }
            set { _infoText = value; NotifyPropertyChanged(); }
        }

        public bool UpdatedVersionInputEnable
        {
            get { return _updatedVersionInputEnable; }
            set { _updatedVersionInputEnable = value; NotifyPropertyChanged(); }
        }

        public bool SaveButtonEnable
        {
            get { return _saveButtonEnable; }
            set { _saveButtonEnable = value; NotifyPropertyChanged(); }
        }

        public List<LangTextDto> Added { get; set; }

        public List<LangTextDto> Changed { get; set; }

        public List<LangTextDto> NonChanged { get; set; }

        public List<LangTextDto> RemovedList { get; set; }

        public Dictionary<string, LangTextDto> RemovedDict { get; set; }

        public List<string> FileList { get; set; }

        public OpenCsvFileCommand OpenFileCommand { get; }
        public LoadCsvAndDbCommand LoadCsvAndDbCommand { get; }
        public CompareListSelecteCommand CompareListSelecteCommand { get; }

        public ICommand SaveToDbCommand => new ExcuteViewModelMethod(SaveChanges);

        public Dictionary<string, LangTextDto> DbDict { get; set; }
        public Dictionary<string, LangTextDto> CsvDict { get; set; }

        public UC_LangDataGrid LangDataGrid { get; set; }

        public bool IsReadMode { get; set; }

        public CompareWindowViewModel(UC_LangDataGrid langDataGrid)
        {
            _addedTag = "新增";
            _changedTag = "修改";
            _removedTag = "移除";

            LangDataGrid = langDataGrid;

            OpenFileCommand = new OpenCsvFileCommand(this);
            LoadCsvAndDbCommand = new LoadCsvAndDbCommand(this);
            CompareListSelecteCommand = new CompareListSelecteCommand(this);

            FileList = new List<string>();
            IsReadMode = true;
            LoadCsvAndDbCommand.IsExecuting = true;
            LoadButtonContent = "开始读取";

            UpdatedVersionInputEnable = true;
        }

        public void PathTooltip()
        {
            FilePathTooltip = null;

            foreach (var s in FileList)
            {
                if (FilePathTooltip == null)
                    FilePathTooltip = s;
                else
                    FilePathTooltip = FilePathTooltip + "\r\n" + s;

            }
        }

        public async void SaveChanges(object o)
        {
            var saveFileToDisk = new ExportDbToFile();

            if (UpdateVersionText == "" || UpdateVersionText == null || UpdateVersionText == "更新版本号(必填)")
            {
                MessageBox.Show("请输入新版本文本的版本号！比如“Update25”等！", "提醒",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SaveButtonEnable = false;
                UpdatedVersionInputEnable = false;

                var db = new LangTextRepository();

                if (Added.Count >= 1)    //判断新加内容是否为空
                {
                    //Debug.WriteLine("Added Count: {0}", Added.Count);
                    InfoText = "正在保存新加内容……";
                    //await Task.Run(() => db.AddNewLangs(Added));

                    if (IsAdminGuid())
                    {
                        var filename = saveFileToDisk.ExportLangTextsAsJson(Added, LangChangeType.Added);
                        Debug.WriteLine(filename);
                    }
                        
                }

                if (Changed.Count >= 1)   //判断修改内容是否为空
                {
                    InfoText = "正在应用修改内容……";
                    //await Task.Run(() => db.UpdateLangsEN(Changed));

                    if (IsAdminGuid())
                    {
                        saveFileToDisk.ExportLangTextsAsJson(Changed, LangChangeType.ChangedEN);
                    }
                }

                if (RemovedList.Count >= 1)   //判断移除内容是否为空
                {
                    InfoText = "正在删除移除内容……";
                    //await Task.Run(() => db.DeleteLangs(RemovedList));

                    if (IsAdminGuid())
                    {
                        saveFileToDisk.ExportLangTextsAsJson(RemovedList, LangChangeType.Removed);
                    }
                }

                SaveButtonEnable = true;
            }

            
        }

        private bool IsAdminGuid()
        {
            return App.LangConfig.UserGuid == new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6")
                || App.LangConfig.UserGuid == new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D");
        }

    }
}
