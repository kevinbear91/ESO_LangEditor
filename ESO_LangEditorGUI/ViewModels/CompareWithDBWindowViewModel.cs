using AutoMapper;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.NetClient;
using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.Views;
using ESO_LangEditorGUI.Views.UserControls;
using Microsoft.Win32;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class CompareWithDBWindowViewModel : BindableBase
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
        private List<LangTextDto> _added;
        private List<LangTextDto> _changed;
        private List<LangTextDto> _nonChanged;
        private List<LangTextDto> _removedList;
        private Dictionary<string, LangTextDto> _removedDict;
        private ObservableCollection<LangTextDto> _gridData;
        private IMapper _mapper;
        private LangtextNetService _langtextNetService;
        private string _selectedKey;

        public string UpdateVersionText
        {
            get { return _updateVersionText; }
            set { SetProperty(ref _updateVersionText, value); }
        }

        public string AddedTag
        {
            get { return _addedTag; }
            set { SetProperty(ref _addedTag, "新增(" + value + ")"); }
        }

        public string ChangedTag
        {
            get { return _changedTag; }
            set { SetProperty(ref _changedTag, "修改(" + value + ")"); }
        }

        public string RemovedTag
        {
            get { return _removedTag; }
            set { SetProperty(ref _removedTag, "移除(" + value + ")"); }
        }

        public string FileCount
        {
            get { return _fileCount; }
            set { SetProperty(ref _fileCount, value); }
        }

        public string LoadButtonContent
        {
            get { return _loadButtonContent; }
            set { SetProperty(ref _loadButtonContent, value); }
        }

        public string FilePathTooltip
        {
            get { return _filePathTooltip; }
            set { SetProperty(ref _filePathTooltip, value); }
        }

        public string InfoText
        {
            get { return _infoText; }
            set { SetProperty(ref _infoText, value); }
        }

        public bool UpdatedVersionInputEnable
        {
            get { return _updatedVersionInputEnable; }
            set { SetProperty(ref _updatedVersionInputEnable, value); }
        }

        public bool SaveButtonEnable
        {
            get { return _saveButtonEnable; }
            set { SetProperty(ref _saveButtonEnable, value); }
        }

        private LangTextRepoClientService _langTextRepository = new LangTextRepoClientService();

        public List<LangTextDto> Added 
        {
            get { return _added; }
            set { SetProperty(ref _added, value); }
        }

        public List<LangTextDto> Changed
        {
            get { return _changed; }
            set { SetProperty(ref _changed, value); }
        }

        public List<LangTextDto> NonChanged
        {
            get { return _nonChanged; }
            set { SetProperty(ref _nonChanged, value); }
        }

        public List<LangTextDto> RemovedList
        {
            get { return _removedList; }
            set { SetProperty(ref _removedList, value); }
        }

        public Dictionary<string, LangTextDto> RemovedDict
        {
            get { return _removedDict; }
            set { SetProperty(ref _removedDict, value); }
        }

        public ObservableCollection<LangTextDto> GridData
        {
            get { return _gridData; }
            set { SetProperty(ref _gridData, value); }
        }

        public List<string> FileList { get; set; }

        public ExcuteViewModelMethod OpenFileCommand => new ExcuteViewModelMethod(OpenFileWindow);
        public ExcuteViewModelMethod CompareListSelecteCommand => new ExcuteViewModelMethod(SelecteOption);
        public ExcuteViewModelMethod SaveToDbCommand => new ExcuteViewModelMethod(SaveChanges);
        public ExcuteViewModelMethod SaveToServerCommand => new ExcuteViewModelMethod(UploadResultToServer);
        public ExcuteViewModelMethod CompareFilesCommand => new ExcuteViewModelMethod(CompreWithDb);

        //public Dictionary<string, LangTextDto> DbDict { get; set; }
        //public Dictionary<string, LangTextDto> CsvDict { get; set; }

        public CompareWithDBWindow compareWithDBWindow;

        public CompareWithDBWindowViewModel()
        {
            _addedTag = "新增";
            _changedTag = "修改";
            _removedTag = "移除";

            FileList = new List<string>();
            SaveToDbCommand.IsExecuting = true;
            SaveToServerCommand.IsExecuting = true;
            //CompareListSelecteCommand.IsExecuting = true;

            UpdatedVersionInputEnable = true;
            GridData = new ObservableCollection<LangTextDto>();
            _mapper = App.Mapper;
            _langtextNetService = new LangtextNetService(App.ServerPath);
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
                        var filename = saveFileToDisk.ExportLangTextsAsJson(Changed, LangChangeType.ChangedEN);
                        Debug.WriteLine(filename);
                    }
                }

                if (RemovedList.Count >= 1)   //判断移除内容是否为空
                {
                    InfoText = "正在删除移除内容……";
                    //await Task.Run(() => db.DeleteLangs(RemovedList));

                    if (IsAdminGuid())
                    {
                        var filename = saveFileToDisk.ExportLangTextsAsJson(RemovedList, LangChangeType.Removed);
                        Debug.WriteLine(filename);
                    }
                }

                SaveButtonEnable = true;
            }

            
        }

        public void UploadResultToServer(object o)
        {
            Debug.WriteLine("Upload: " + _selectedKey);
            switch (_selectedKey)
            {
                case"Added":
                    MakeAddedListToServer();
                    break;
                case "Changed":
                    MakeEnChangedListToServer();
                    break;
                case "Removed":
                    MakeDeletedListToServer();
                    break;
            }
                
        }

        private async void MakeAddedListToServer()
        {
            List<LangTextForCreationDto> langTextForCreationDtos;
            if (Added.Count >= 1)
            {
                langTextForCreationDtos = _mapper.Map<List<LangTextForCreationDto>>(Added);

                try
                {
                    InfoText = "正在上传新增文本，等待服务器处理并返回结果中……";
                    var respondCode = await _langtextNetService.CreateLangtextListAsync(langTextForCreationDtos, 
                        App.LangConfig.UserAuthToken);

                    if (respondCode == HttpStatusCode.OK)
                    {
                        InfoText = "新增文本上传成功";
                    }
                    else
                    {
                        InfoText = "新增文本上传失败，" + respondCode.ToString();
                    }
                }
                catch(HttpRequestException ex)
                {
                    InfoText = ex.Message;
                }
            }

        }

        private async void MakeEnChangedListToServer()
        {
            List<LangTextForUpdateEnDto> langTextForUpdateEnDtos;
            if (Changed.Count >= 1)
            {
                langTextForUpdateEnDtos = _mapper.Map<List<LangTextForUpdateEnDto>>(Changed);

                try
                {
                    InfoText = "正在上传英文变化文本，等待服务器处理并返回结果中……";
                    var respondCode = await _langtextNetService.UpdateLangtextEnList(langTextForUpdateEnDtos,
                        App.LangConfig.UserAuthToken);

                    if (respondCode == HttpStatusCode.OK)
                    {
                        InfoText = "英文变化文本上传成功";
                    }
                    else
                    {
                        InfoText = "英文变化文本上传失败，" + respondCode.ToString();
                    }
                }
                catch (HttpRequestException ex)
                {
                    InfoText = ex.Message;
                }
            }

        }

        private async void MakeDeletedListToServer()
        {
            List<Guid> langTextForDeletedList;
            if (RemovedList.Count >= 1)
            {
                langTextForDeletedList = RemovedList.Select(lang => lang.Id).ToList();

                try
                {
                    InfoText = "正在上传删除列表，等待服务器处理并返回结果中……";
                    var respondCode = await _langtextNetService.DeleteLangtextListAsync(langTextForDeletedList,
                        App.LangConfig.UserAuthToken);

                    if (respondCode == HttpStatusCode.OK)
                    {
                        InfoText = "删除列表上传成功";
                    }
                    else
                    {
                        InfoText = "删除列表上传失败，" + respondCode.ToString();
                    }
                }
                catch (HttpRequestException ex)
                {
                    InfoText = ex.Message;
                }
            }

        }


        private void OpenFileWindow(object o)
        {
            OpenFileDialog dialog = new OpenFileDialog { Multiselect = true };
            //dialog.Filter = "csv (*.csv)|.csv";
            if (dialog.ShowDialog(compareWithDBWindow) == true)
            {
                if (dialog.FileName.EndsWith(".csv") || dialog.FileName.EndsWith(".lua"))
                {
                    foreach (var file in dialog.FileNames)
                    {
                        FileList.Add(file);
                        //filepath.Add(file);
                    }
                    if (FileList.Count == 3)
                    {
                        FileCount = FileList.Count.ToString();
                        PathTooltip();
                        //CompreWithDb();
                    }
                }
                else
                {
                    MessageBox.Show("仅支持读取 .csv 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }

        private async void CompreWithDb(object o)
        {
            OpenFileCommand.IsExecuting = true;      //禁用浏览文件窗口按钮
            SaveToDbCommand.IsExecuting = true;
            SaveToServerCommand.IsExecuting = true;
            UpdatedVersionInputEnable = false;
            CompareFilesCommand.IsExecuting = true;

            var langfileParser = new LangFileParser();
            var luaList = new List<string>();

            Dictionary<string, LangTextDto> fileContent = new Dictionary<string, LangTextDto>();
            InfoText = "正在读取数据库……";

            var DbDict = await Task.Run(() => _langTextRepository.GetAlltLangTextsDictionaryAsync());

            InfoText = "正在分析文件……";
            foreach (var file in FileList)
            {
                if (file.EndsWith(".lua"))
                    luaList.Add(file);
                else
                    fileContent = await langfileParser.CsvParserToDictionaryAsync(file);
            }
            Dictionary<string, LangTextDto> lualist = await langfileParser.LuaParser(luaList);

            foreach (var item in lualist)
            {
                fileContent.Add(item.Key, item.Value);
            }
            //fileContent = await parseLangFile.LuaParser(luaList);

            var CsvDict = fileContent;

            CompareDicts(DbDict, CsvDict);
            
        }

        public void CompareDicts(Dictionary<string, LangTextDto> dbDict, Dictionary<string, LangTextDto> csvDict)
        {
            Debug.WriteLine("开始比较。");
            InfoText = "正在对比……";

            var _first = dbDict;
            var _second = csvDict;

            Added = new List<LangTextDto>();
            Changed = new List<LangTextDto>();
            RemovedList = new List<LangTextDto>();
            NonChanged = new List<LangTextDto>();

            RemovedDict = _first;

            foreach (var other in _second)
            {

                if (_first.TryGetValue(other.Key, out LangTextDto firstValue))
                {
                    if (firstValue.TextEn.Equals(other.Value.TextEn))
                    {
                        NonChanged.Add(firstValue);
                        RemovedDict.Remove(other.Key);
                    }
                    else
                    {
                        Changed.Add(new LangTextDto
                        {
                            Id = firstValue.Id,
                            TextId = firstValue.TextId,
                            IdType = firstValue.IdType,
                            TextEn = other.Value.TextEn,
                            TextZh = firstValue.TextZh,
                            UpdateStats = UpdateVersionText,
                            IsTranslated = firstValue.IsTranslated,
                            EnLastModifyTimestamp = DateTime.Now,
                            ZhLastModifyTimestamp = firstValue.ZhLastModifyTimestamp,
                            LangTextType = firstValue.LangTextType,
                            UserId = App.LangConfig.UserGuid,
                            //review = 2,
                            ReviewerId = firstValue.ReviewerId,
                            ReviewTimestamp = firstValue.ZhLastModifyTimestamp,
                            Revised = firstValue.Revised,
                        });
                        RemovedDict.Remove(other.Key);
                    }
                }
                else
                {
                    Added.Add(new LangTextDto
                    {
                        Id = Guid.NewGuid(),
                        TextId = other.Value.TextId,
                        IdType = other.Value.IdType,
                        TextEn = other.Value.TextEn,
                        TextZh = other.Value.TextEn,
                        UpdateStats = UpdateVersionText,
                        IsTranslated = 0,
                        EnLastModifyTimestamp = DateTime.Now,
                        ZhLastModifyTimestamp = DateTime.Now,
                        LangTextType = other.Value.LangTextType,
                        UserId = App.LangConfig.UserGuid,
                        ReviewerId = App.LangConfig.UserGuid,
                        ReviewTimestamp = DateTime.Now,
                        Revised = 0,
                    });
                    RemovedDict.Remove(other.Key);
                }
            }

            RemovedList = RemovedDict.Values.ToList();

            AddedTag = Added.Count.ToString();
            ChangedTag = Changed.Count.ToString();
            RemovedTag = RemovedList.Count.ToString();

            SaveToDbCommand.IsExecuting = false;        //启用保存按钮

            Debug.WriteLine("Added Line: {0}", Added.Count);
            Debug.WriteLine("Changed Line: {0}", Changed.Count);
            Debug.WriteLine("Removed Line: {0}", RemovedList.Count);
            InfoText = "对比完成！";
        }

        public void SelecteOption(object parameter)
        {
            _selectedKey = parameter as string;
            //var datagrid = _compareWindowViewModel.LangDataGrid.LangDataGridDC;

            if (GridData.Count > 0)
                GridData.Clear();


            //Debug.WriteLine("Selected Key: " + selectedKey);

            switch (_selectedKey)
            {
                case "Added":
                    Debug.WriteLine("Selected: " + _selectedKey);
                    GridData.AddRange(Added);
                    if (Added.Count > 1)
                    {
                        SaveToServerCommand.IsExecuting = false;
                    }
                    break;
                case "Changed":
                    Debug.WriteLine("Selected: " + _selectedKey);
                    GridData.AddRange(Changed);
                    if (Changed.Count > 1)
                    {
                        SaveToServerCommand.IsExecuting = false;
                    }
                    break;
                case "Removed":
                    Debug.WriteLine("Selected: " + _selectedKey);
                    GridData.AddRange(RemovedList);
                    if (RemovedList.Count > 1)
                    {
                        SaveToServerCommand.IsExecuting = false;
                    }
                    break;
            }
        }

        private bool IsAdminGuid()
        {
            return App.LangConfig.UserGuid == new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6")
                || App.LangConfig.UserGuid == new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D");
        }

    }
}
