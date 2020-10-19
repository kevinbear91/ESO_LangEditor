using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.View.UserControls;
using ESO_LangEditorLib.Models.Client;
using System;
using System.Collections.Generic;
using System.Text;

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

        public List<LangTextDto> Added { get; set; }

        public List<LangTextDto> Changed { get; set; }

        public List<LangTextDto> NonChanged { get; set; }

        public List<LangTextDto> RemovedList { get; set; }

        public Dictionary<string, LangTextDto> RemovedDict { get; set; }

        public List<string> FileList { get; set; }

        public OpenCsvFileCommand OpenFileCommand { get; }
        public LoadCsvAndDbCommand LoadCsvAndDbCommand { get; }
        public CompareListSelecteCommand CompareListSelecteCommand { get; }

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

    }
}
