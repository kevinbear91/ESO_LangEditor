using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.Core.RequestParameters;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.EventAggres;
using ESO_LangEditor.GUI.Services;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Convert;

namespace ESO_LangEditor.GUI.ViewModels
{
    public class MainWindowSearchbarViewModel : BindableBase
    {

        private SearchPostion _selectedSearchPostion;
        private SearchTextType _selectedSearchTextType;
        private SearchTextType _selectedSearchTextTypeSecond;
        private string _keyword;
        private string _keywordSecond;
        private bool _doubleKeyWordSearch;
        private ClientConnectStatus _connectStatus;
        private bool _isLoadJp;
        private Dictionary<string, string> _jpLangDict;
        private ObservableCollection<ClientPageModel> _pageInfo;

        public SearchPostion SelectedSearchPostion
        {
            get => _selectedSearchPostion;
            set => SetProperty(ref _selectedSearchPostion, value);
        }

        public IEnumerable<SearchPostion> SearchPostionCombox
        {
            get { return Enum.GetValues(typeof(SearchPostion)).Cast<SearchPostion>(); }
            //set { _searchPostion =  }
        }

        public IEnumerable<SearchTextType> SearchTextTypeCombox
        {
            get { return Enum.GetValues(typeof(SearchTextType)).Cast<SearchTextType>(); }
            //set { _searchTextType = value; NotifyPropertyChanged(); }
        }

        public SearchTextType SelectedSearchTextType
        {
            get => _selectedSearchTextType;
            set => SetProperty(ref _selectedSearchTextType, value);
        }

        public SearchTextType SelectedSearchTextTypeSecond
        {
            get => _selectedSearchTextTypeSecond;
            set => SetProperty(ref _selectedSearchTextTypeSecond, value);
        }

        public string Keyword
        {
            get => _keyword;
            set => SetProperty(ref _keyword, value);
        }

        public string KeywordSecond
        {
            get => _keywordSecond;
            set => SetProperty(ref _keywordSecond, value);
        }

        public bool DoubleKeyWordSearch
        {
            get => _doubleKeyWordSearch;
            set => SetProperty(ref _doubleKeyWordSearch, value);
        }

        public bool ServerSideSearch
        {
            get => App.LangConfig.AppSetting.IsServerSideSearch;
            set => App.LangConfig.AppSetting.IsServerSideSearch = value;
        }

        public bool AskExit
        {
            get => App.LangConfig.AppSetting.IsAskToExit;
            set => App.LangConfig.AppSetting.IsAskToExit = value;
        }

        public ClientConnectStatus ConnectStatus
        {
            get => _connectStatus;
            set => SetProperty(ref _connectStatus, value);
        }

        public int SelectedPageSize
        {
            get => IsSelectedPageSizeNumberInList();
            set => App.LangConfig.AppSetting.ServerSideSearchPageSize = value;
        }

        public ObservableCollection<ClientPageModel> PageInfo
        {
            get => _pageInfo;
            set => SetProperty(ref _pageInfo, value);
        }

        public List<int> PageSizeList { get; } = new List<int> { 50, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

        private IEventAggregator _ea;
        private ILangTextRepoClient _langTextRepo;
        private ILangFile _langFile;
        private ILangTextAccess _langTextAccess;
        private IBackendService _backendService;

        public ICommand SearchLangCommand => new ExcuteViewModelMethod(SearchLangText);
        public ExcuteViewModelMethod LoadJpLangCommand => new ExcuteViewModelMethod(LoadJpLang);
        public ICommand GetPageCommand => new ExcuteViewModelMethod(GetPage);

        public MainWindowSearchbarViewModel(IEventAggregator ea, ILangTextRepoClient langTextRepoClient,
            ILangFile LangFile, IBackendService backendService, ILangTextAccess langTextAccess)
        {
            _ea = ea;
            _langTextRepo = langTextRepoClient;
            _langFile = LangFile;
            _backendService = backendService;
            _langTextAccess = langTextAccess;

            _ea.GetEvent<ConnectStatusChangeEvent>().Subscribe(ChangeConnectStatus);
            
        }

        private async void SearchLangText(object obj)
        {
            List<LangTextDto> result;

            if (string.IsNullOrWhiteSpace(Keyword))
            {
                MessageBox.Show("不支持全局搜索，请输入关键字！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (ServerSideSearch)
                {
                    result = await ServerSearch();
                }
                else
                {
                    if (DoubleKeyWordSearch)
                    {
                        result = await _langTextRepo.GetLangTextByConditionAsync(Keyword, KeywordSecond,
                            SelectedSearchTextType, SelectedSearchTextTypeSecond, SelectedSearchPostion);
                    }
                    else
                    {
                        result = await _langTextRepo.GetLangTextByConditionAsync(Keyword,
                            SelectedSearchTextType, SelectedSearchPostion);
                    }
                }
                

                if (_isLoadJp)
                {
                    foreach (var lang in result)
                    {
                        if(_jpLangDict.TryGetValue(lang.TextId, out string langJp))
                        {
                            lang.TextJp = langJp;
                        }
                    }
                }

                _ea.GetEvent<LangtextPostToMainDataGrid>().Publish(result);

            }
        }

        private void ChangeConnectStatus(ClientConnectStatus obj)
        {
            ConnectStatus = obj;
        }

        private async void LoadJpLang(object obj)
        {
            var serverConfig = await _backendService.GetServerRespondAndConfig();

            bool isChecked = (bool)obj;

            List<string> luaFileList = new List<string>
            {
                @"Data\jp_client.lua",
                @"Data\jp_pregame.lua"
            };

            if (File.Exists(@"Data\jp.lang") && App.LangConfig.LangJpSha256 == serverConfig.LangJpPackSHA256)
            {
                if (isChecked)
                {
                    LoadJpLangCommand.IsExecuting = true;

                    _ea.GetEvent<JpColumnVisibilityEvent>().Publish(Visibility.Visible);

                    _jpLangDict = await _langFile.ParseJpLangFile(@"Data\jp.lang");
                    var luaDict = await _langFile.ParseJpLuaFile(luaFileList);

                    foreach (var lua in luaDict)
                    {
                        _jpLangDict.Add(lua.Key, lua.Value);
                    }

                    _isLoadJp = true;
                    LoadJpLangCommand.IsExecuting = false;

                }
                else
                {
                    _isLoadJp = false;
                    _ea.GetEvent<JpColumnVisibilityEvent>().Publish(Visibility.Collapsed);
                }
            }
            else
            {
                var dialogResult = MessageBox.Show("没有找到日语本地化文件或有新版可用，现在是否要下载？", "错误", 
                    MessageBoxButton.YesNo, MessageBoxImage.Error);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    await DownloadJpFiles(serverConfig);
                }
            }
        }

        private void GetPage(object obj)
        {
            var para = (int)obj;


            MessageBox.Show(para.ToString());
        }

        private async Task DownloadJpFiles(AppConfigServer appConfigServer)
        {
            LoadJpLangCommand.IsExecuting = true;

            //var serverConfig = await _backendService.GetServerRespondAndConfig();

            _backendService.DownloadAndExtractComplete += SetAppConfigClientJpLangSha256;
            await _backendService.DownloadFileFromServer(App.ServerPath + appConfigServer.LangJpPackPath,
                appConfigServer.LangJpPackPath, appConfigServer.LangJpPackSHA256);
        }

        private void SetAppConfigClientJpLangSha256(object sender, string e)
        {
            _backendService.DownloadAndExtractComplete -= SetAppConfigClientJpLangSha256;

            App.LangConfig.LangJpSha256 = e;
            AppConfigClient.Save(App.LangConfig);

            LoadJpLangCommand.IsExecuting = false;
            _ea.GetEvent<ConnectProgressString>().Publish("文件下载完成！");

            Task.Run(() => LoadJpLang(true));
        }

        private int IsSelectedPageSizeNumberInList()
        {
            int pageSize = App.LangConfig.AppSetting.ServerSideSearchPageSize;

            if (PageSizeList.Contains(pageSize))
            {
                return pageSize;
            }

            return PageSizeList.FirstOrDefault();
        }

        private async Task<List<LangTextDto>> ServerSearch()
        {
            string lang = "en";
            LangTextParameters searchPara = new LangTextParameters
            {
                PageNumber = 1,
                PageSize = SelectedPageSize,
                SearchPostion = SelectedSearchPostion,
            };

            if (SelectedSearchTextType == SearchTextType.TextChineseS)
            {
                lang = "zh";
            }

            if (DoubleKeyWordSearch)
            {
                switch (SelectedSearchTextType)
                {
                    case SearchTextType.Type:
                        searchPara.IdType = ToInt32(KeywordSecond);
                        break;
                    case SearchTextType.UpdateStatus:
                        searchPara.GameVersionInfo = KeywordSecond;
                        break;
                    case SearchTextType.ByUser:
                        searchPara.UserId = new Guid(KeywordSecond);
                        break;
                }
            }

            var langtext = await _langTextAccess.GetLangTexts(lang, searchPara, Keyword);

            GetPageInfoFromServer(langtext.PageData);

            return langtext;

        }

        private void GetPageInfoFromServer(PageData pageData)
        {
            PageInfo = null;
            PageInfo = new ObservableCollection<ClientPageModel>();
            //PageInfo = new List<ClientPageModel>();

            for (int i = 1; i <= pageData.TotalPages; i++)
            {
                if (pageData.CurrentPage == i)
                {
                    PageInfo.Add(new ClientPageModel { IsCurrentPage = true, PageNumber = i, GetPageCommand = GetPageCommand });
                }
                else
                {
                    PageInfo.Add(new ClientPageModel { IsCurrentPage = false, PageNumber = i, GetPageCommand = GetPageCommand });
                }

            }

            //if (pageData.TotalPages > 1)
            //{
            //    if (pageData.CurrentPage > 4)
            //    {
            //        PageInfo.Add(new ClientPageModel { IsChecked = false, PageNumber = 1 });

            //        for (int i = 1; i > 5; i++)
            //        {
            //            if (pageData.CurrentPage == i)
            //            {
            //                PageInfo.Add(new ClientPageModel { IsChecked = true, PageNumber = i });
            //            }
            //            else
            //            {
            //                PageInfo.Add(new ClientPageModel { IsChecked = false, PageNumber = 1 });
            //            }

            //        }
            //    }





            //}




        }
    }
}
