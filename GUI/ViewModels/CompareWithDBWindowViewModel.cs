using AutoMapper;
using Core.Models;
using Core.EnumTypes;
using GUI;
using GUI.Views;
using GUI.Command;
using GUI.Services;
using Microsoft.Win32;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Prism.Events;
using GUI.EventAggres;
using Core.Entities;

namespace GUI.ViewModels
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
        private bool _ClearZhIfEnChangedForSkill = true;
        private bool _IsOfficialZh;
        private List<LangTextDto> _added;
        private List<LangTextDto> _changed;
        private List<LangTextDto> _nonChanged;
        private List<LangTextDto> _removedList;
        private Dictionary<string, LangTextDto> _removedDict;
        private ObservableCollection<LangTextDto> _currentGridData;
        private Dictionary<int, string> _GameVersionDict;
        private int _revisedNumber;
        private IMapper _mapper;
        private ILangFile _langfile;
        private IBackendService _backendService;
        private IGeneralAccess _generalAccess;
        private ILangTextRepoClient _langTextRepo;
        //private ILangTextAccess _langTextAccess;
        private string _selectedKey;

        public string UpdateVersionText
        {
            get => _updateVersionText;
            set => SetProperty(ref _updateVersionText, value);
        }

        public string AddedTag
        {
            get => _addedTag;
            set => SetProperty(ref _addedTag, "新增(" + value + ")");
        }

        public string ChangedTag
        {
            get => _changedTag;
            set => SetProperty(ref _changedTag, "修改(" + value + ")");
        }

        public string RemovedTag
        {
            get => _removedTag;
            set => SetProperty(ref _removedTag, "移除(" + value + ")");
        }

        public string FileCount
        {
            get => _fileCount;
            set => SetProperty(ref _fileCount, value);
        }

        public string LoadButtonContent
        {
            get => _loadButtonContent;
            set => SetProperty(ref _loadButtonContent, value);
        }

        public string FilePathTooltip
        {
            get => _filePathTooltip;
            set => SetProperty(ref _filePathTooltip, value);
        }

        public string InfoText
        {
            get => _infoText;
            set => SetProperty(ref _infoText, value);
        }

        public bool UpdatedVersionInputEnable
        {
            get => _updatedVersionInputEnable;
            set => SetProperty(ref _updatedVersionInputEnable, value);
        }

        public bool SaveButtonEnable
        {
            get => _saveButtonEnable;
            set => SetProperty(ref _saveButtonEnable, value);
        }

        public bool ClearZhIfEnChangedForSkill
        {
            get => _ClearZhIfEnChangedForSkill;
            set => SetProperty(ref _ClearZhIfEnChangedForSkill, value);
        }

        public bool IsOfficialZh
        {
            get => _IsOfficialZh;
            set => SetProperty(ref _IsOfficialZh, value);
        }

        //private ILangTextRepoClient _langTextRepository;

        public List<LangTextDto> Added
        {
            get => _added;
            set => SetProperty(ref _added, value);
        }

        public List<LangTextDto> Changed
        {
            get => _changed;
            set => SetProperty(ref _changed, value);
        }

        public List<LangTextDto> NonChanged
        {
            get => _nonChanged;
            set => SetProperty(ref _nonChanged, value);
        }

        public List<LangTextDto> RemovedList
        {
            get => _removedList;
            set => SetProperty(ref _removedList, value);
        }

        public Dictionary<string, LangTextDto> RemovedDict
        {
            get => _removedDict;
            set => SetProperty(ref _removedDict, value);
        }

        public ObservableCollection<LangTextDto> CurrentGridData
        {
            get => _currentGridData;
            set => SetProperty(ref _currentGridData, value);
        }

        public Visibility JpVisibility => Visibility.Collapsed;

        public List<string> FileList { get; set; }

        public ExcuteViewModelMethod OpenFileCommand => new ExcuteViewModelMethod(OpenFileWindow);
        public ExcuteViewModelMethod CompareListSelecteCommand => new ExcuteViewModelMethod(SelecteOption);
        public ExcuteViewModelMethod SaveToServerCommand => new ExcuteViewModelMethod(UploadResultToServer);
        public ExcuteViewModelMethod CompareFilesCommand => new ExcuteViewModelMethod(CompreWithDb);
        public ExcuteViewModelMethod CheckNewIdsCommand => new ExcuteViewModelMethod(CompareAddedIdType);
        //public ExcuteViewModelMethod SetSelectedLangZhToNullCommand => new ExcuteViewModelMethod(SetSelectedLangzhToNull);

        public CompareWithDBWindow compareWithDBWindow;
        private IEventAggregator _ea;

        public CompareWithDBWindowViewModel(IEventAggregator ea, /*IBackendService backendService,*/ ILangTextRepoClient langTextRepo,
            /*ILangTextAccess langTextAccess,*/ IMapper mapper, ILangFile langfile/*, IGeneralAccess generalAccess*/)
        {
            _addedTag = "新增";
            _changedTag = "修改";
            _removedTag = "移除";

            FileList = new List<string>();
            SaveToServerCommand.IsExecuting = true;

            UpdatedVersionInputEnable = true;
            CurrentGridData = new ObservableCollection<LangTextDto>();

            _langfile = langfile;
            //_backendService = backendService;
            _langTextRepo = langTextRepo;
            //_langTextAccess = langTextAccess;
            _mapper = mapper;
            //_generalAccess = generalAccess;
            _ea = ea;

            _ea.GetEvent<SelectGameVersionEvent>().Subscribe(SelectGameVersion);

            //_mapper = App.Mapper;
            //_langtextNetService = new LangtextNetService(App.ServerPath);
        }

        public void PathTooltip()
        {
            FilePathTooltip = null;

            foreach (var s in FileList)
            {
                FilePathTooltip = FilePathTooltip == null ? s : FilePathTooltip + "\r\n" + s;
            }
        }

        public void UploadResultToServer(object o)
        {
            Debug.WriteLine("Upload: " + _selectedKey);
            switch (_selectedKey)
            {
                case "Added":
                    //MakeAddedListToServer();
                    AddLangText();
                    break;
                case "Changed":
                    //MakeEnChangedListToServer();
                    UpdateLangText();
                    break;
                case "Removed":
                    //MakeDeletedListToServer();
                    DeleteLangText();
                    break;
            }

        }

        //private async void MakeAddedListToServer()
        //{
        //    List<LangTextForCreationDto> langTextForCreationDtos;
        //    if (Added.Count >= 1)
        //    {
        //        langTextForCreationDtos = _mapper.Map<List<LangTextForCreationDto>>(Added);

        //        try
        //        {
        //            InfoText = "正在上传新增文本，等待服务器处理并返回结果中……";
        //            var respondCode = await _langTextAccess.AddLangTexts(langTextForCreationDtos);

        //            InfoText = respondCode.Message;
        //        }
        //        catch (HttpRequestException ex)
        //        {
        //            InfoText = ex.Message;
        //        }
        //    }

        //}

        //private async void MakeEnChangedListToServer()
        //{
        //    List<LangTextForUpdateEnDto> langTextForUpdateEnDtos;
        //    if (Changed.Count >= 1)
        //    {
        //        langTextForUpdateEnDtos = _mapper.Map<List<LangTextForUpdateEnDto>>(Changed);

        //        try
        //        {
        //            InfoText = "正在上传英文变化文本，等待服务器处理并返回结果中……";
        //            var respondCode = await _langTextAccess.UpdateLangTextEn(langTextForUpdateEnDtos);

        //            InfoText = respondCode.Message;
        //        }
        //        catch (HttpRequestException ex)
        //        {
        //            InfoText = ex.Message;
        //        }
        //    }

        //}

        //private async void MakeDeletedListToServer()
        //{
        //    List<Guid> langTextForDeletedList;
        //    if (RemovedList.Count >= 1)
        //    {
        //        langTextForDeletedList = RemovedList.Select(lang => lang.Id).ToList();

        //        try
        //        {
        //            InfoText = "正在上传删除列表，等待服务器处理并返回结果中……";
        //            var respondCode = await _langTextAccess.RemoveLangTexts(langTextForDeletedList);

        //            InfoText = respondCode.Message;
        //        }
        //        catch (HttpRequestException ex)
        //        {
        //            InfoText = ex.Message;
        //        }
        //    }
        //}

        private async void AddLangText()
        {
            if (Added.Count >= 1)
            {
                var newLangtext = _mapper.Map<List<LangTextClient>>(Added);

                try
                {
                    InfoText = "正在处理新增文本……";

                    if (await _langTextRepo.AddLangtexts(newLangtext))
                    {
                        InfoText = "新增完成";
                    }

                }
                catch (HttpRequestException ex)
                {
                    InfoText = ex.Message;
                }
            }
        }


        private async void UpdateLangText()
        {
            if (Changed.Count >= 1)
            {
                //langTextForUpdateEnDtos = _mapper.Map<List<LangTextForUpdateEnDto>>(Changed);
                var changedLangtext = _mapper.Map<List<LangTextClient>>(Changed);

                try
                {
                    InfoText = "正在处理英文变化文本……";
                    //var respondCode = await _langTextAccess.UpdateLangTextEn(langTextForUpdateEnDtos);

                    if (await _langTextRepo.UpdateLangtexts(changedLangtext))
                    {
                        InfoText = "处理完成";
                    }

                }
                catch (HttpRequestException ex)
                {
                    InfoText = ex.Message;
                }
            }
        }

        private async void DeleteLangText()
        {
            List<Guid> langTextForDeletedList;
            if (RemovedList.Count >= 1)
            {
                langTextForDeletedList = RemovedList.Select(lang => lang.Id).ToList();

                try
                {
                    InfoText = "正在上传删除列表，等待服务器处理并返回结果中……";
                    //var respondCode = await _langTextAccess.RemoveLangTexts(langTextForDeletedList);

                    if (await _langTextRepo.DeleteLangtexts(langTextForDeletedList))
                    {
                        InfoText = "处理完成";
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
                if (dialog.FileName.EndsWith(".lang") || dialog.FileName.EndsWith(".csv") || dialog.FileName.EndsWith(".lua"))
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
                    MessageBox.Show("仅支持读取 .csv、.lang、.lua 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }

        private async void CompreWithDb(object o)
        {
            OpenFileCommand.IsExecuting = true;      //禁用浏览文件窗口按钮
            SaveToServerCommand.IsExecuting = true;
            UpdatedVersionInputEnable = false;
            CompareFilesCommand.IsExecuting = true;

            //var langfileParser = new LangFileParser();
            var luaList = new List<string>();

            Dictionary<string, LangTextDto> fileContent = new Dictionary<string, LangTextDto>();
            InfoText = "正在读取数据库……";

            _GameVersionDict = await _langTextRepo.GetGameVersion();
            var revised = await _langTextRepo.GetRevNumber(1);
            _revisedNumber = revised.Rev + 1;

            var DbDict = await Task.Run(() => _langTextRepo.GetAlltLangTextsDictionaryAsync(0));

            InfoText = "正在分析文件……";
            foreach (var file in FileList)
            {
                if (file.EndsWith(".lua"))
                {
                    luaList.Add(file);
                }
                else
                {
                    if (file.EndsWith(".csv"))
                    {
                        fileContent = await _langfile.ParseCsvFile(file);
                    }
                    else
                    {
                        fileContent = await _langfile.ParseLangFile(file, IsOfficialZh);
                    }
                }
            }
            Dictionary<string, LangTextDto> lualist = await _langfile.ParseLuaFile(luaList);

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

            if (IsOfficialZh)
            {
                foreach (var other in _second)
                {
                    if (_first.TryGetValue(other.Key, out LangTextDto firstValue))
                    {
                        if (firstValue.TextZh_Official != null && firstValue.TextZh_Official.Equals(other.Value.TextZh_Official))
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
                                TextEn = firstValue.TextEn,
                                TextZh = firstValue.TextZh,
                                TextZh_Official = other.Value.TextZh_Official,
                                GameApiVersion = Convert.ToInt32(UpdateVersionText),
                                LangTextType = firstValue.LangTextType,
                                UserId = new Guid("0CEFB727-3B2A-402C-88ED-5EB703BC02B5"),
                                ReviewerId = new Guid("0CEFB727-3B2A-402C-88ED-5EB703BC02B5"),
                                Revised = _revisedNumber,
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
                            TextZh_Official = other.Value.TextZh_Official,
                            GameApiVersion = Convert.ToInt32(UpdateVersionText),
                            LangTextType = other.Value.LangTextType,
                            UserId = new Guid("0CEFB727-3B2A-402C-88ED-5EB703BC02B5"),
                            ReviewerId = new Guid("0CEFB727-3B2A-402C-88ED-5EB703BC02B5"),
                            ReviewTimestamp = DateTime.UtcNow,
                            Revised = _revisedNumber,
                        });
                        RemovedDict.Remove(other.Key);
                    }
                }
            }
            else
            {
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
                                GameApiVersion = Convert.ToInt32(UpdateVersionText),
                                EnLastModifyTimestamp = DateTime.UtcNow,
                                ZhLastModifyTimestamp = firstValue.ZhLastModifyTimestamp,
                                LangTextType = firstValue.LangTextType,
                                UserId = new Guid("0CEFB727-3B2A-402C-88ED-5EB703BC02B5"),
                                //review = 2,
                                ReviewerId = new Guid("0CEFB727-3B2A-402C-88ED-5EB703BC02B5"),
                                ReviewTimestamp = firstValue.ZhLastModifyTimestamp,
                                Revised = _revisedNumber,
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
                            TextZh = null,
                            GameApiVersion = Convert.ToInt32(UpdateVersionText),
                            EnLastModifyTimestamp = DateTime.UtcNow,
                            ZhLastModifyTimestamp = DateTime.UtcNow,
                            LangTextType = other.Value.LangTextType,
                            UserId = new Guid("0CEFB727-3B2A-402C-88ED-5EB703BC02B5"),
                            ReviewerId = new Guid("0CEFB727-3B2A-402C-88ED-5EB703BC02B5"),
                            ReviewTimestamp = DateTime.UtcNow,
                            Revised = _revisedNumber,
                        });
                        RemovedDict.Remove(other.Key);
                    }
                }
            }

            

            RemovedList = RemovedDict.Values.ToList();

            if (ClearZhIfEnChangedForSkill)
            {
                FindChangedLangTextIsGearOrSkill();
            }

            AddedTag = Added.Count.ToString();
            ChangedTag = Changed.Count.ToString();
            RemovedTag = RemovedList.Count.ToString();

            Debug.WriteLine("Added Line: {0}", Added.Count);
            Debug.WriteLine("Changed Line: {0}", Changed.Count);
            Debug.WriteLine("Removed Line: {0}", RemovedList.Count);
            InfoText = "对比完成！";
        }

        public void SelecteOption(object parameter)
        {
            _selectedKey = parameter as string;
            //var datagrid = _compareWindowViewModel.LangDataGrid.LangDataGridDC;

            if (CurrentGridData.Count > 0)
                CurrentGridData.Clear();


            //Debug.WriteLine("Selected Key: " + selectedKey);

            switch (_selectedKey)
            {
                case "Added":
                    Debug.WriteLine("Selected: " + _selectedKey);
                    CurrentGridData.AddRange(Added);
                    if (Added.Count > 1)
                    {
                        SaveToServerCommand.IsExecuting = false;
                    }
                    break;
                case "Changed":
                    Debug.WriteLine("Selected: " + _selectedKey);
                    CurrentGridData.AddRange(Changed);
                    if (Changed.Count > 1)
                    {
                        SaveToServerCommand.IsExecuting = false;
                    }
                    break;
                case "Removed":
                    Debug.WriteLine("Selected: " + _selectedKey);
                    CurrentGridData.AddRange(RemovedList);
                    if (RemovedList.Count > 1)
                    {
                        SaveToServerCommand.IsExecuting = false;
                    }
                    break;
            }
        }
        private async void CompareAddedIdType(object o)
        {
            var IdList = await _generalAccess.GetIdtypeDtos();
            var ids = IdList.Select(id => id.IdType).ToList();
            List<LangTypeCatalogDto> newIds = new List<LangTypeCatalogDto>();

            foreach (var lang in Added)
            {
                if (!ids.Contains(lang.IdType))
                {
                    newIds.Add(new LangTypeCatalogDto 
                    { 
                        IdType = lang.IdType, 
                        IdTypeZH = lang.IdType.ToString() 
                    });
                }
            }

            if (newIds.Count != 0)
            {
                MessageBox.Show($"新增了 {newIds.Count} 条ID, 点击确定上传", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                var result = await _generalAccess.UploadIdTypeDto(newIds);

                MessageBox.Show(result.Message);
            }
            else
            {
                MessageBox.Show("无新增ID", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void FindChangedLangTextIsGearOrSkill()
        {
            List<LangTextDto> _tempList = new List<LangTextDto>();
            if (Changed.Count >= 1)
            {
                foreach (var lang in Changed)
                {
                    if (lang.IdType == 132143172 || lang.IdType == 50040644)
                    {
                        lang.TextZh = null;
                        _tempList.Add(lang);
                    }
                    else
                    {
                        _tempList.Add(lang);
                    }
                }
                Changed = _tempList;
            }
        }

        private void SelectGameVersion(int obj)
        {
            UpdateVersionText = obj.ToString();
        }

        private async void SetSelectedLangzhToNull(object obj)
        {
            var selectedLangtext = (List<LangTextDto>)obj;

            //var langUpdate = _mapper.Map<List<LangTextForUpdateZhDto>>(selectedLangtext);

            //foreach(var lang in langUpdate)
            //{
            //    lang.UserId = App.User.Id;
            //    lang.TextZh = null;
            //}

            //await _backendService.UploadlangtextUpdateZh(langUpdate);
        }

    }
}
