using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.EventAggres;
using ESO_LangEditor.GUI.Services;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
        private bool _serverSideSearch;
        private ClientConnectStatus _connectStatus;

        private bool _isLoadJp;
        private Dictionary<string, string> _jpLangDict;

        public ICommand SearchLangCommand => new ExcuteViewModelMethod(SearchLangText);
        public ExcuteViewModelMethod LoadJpLangCommand => new ExcuteViewModelMethod(LoadJpLang);

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
            get => _serverSideSearch;
            set => SetProperty(ref _serverSideSearch, value);
        }

        public bool AskExit
        {
            get { return App.LangConfig.AppSetting.IsAskToExit; }
            set { App.LangConfig.AppSetting.IsAskToExit = value; }
        }

        public ClientConnectStatus ConnectStatus
        {
            get => _connectStatus;
            set => SetProperty(ref _connectStatus, value);
        }

        private IEventAggregator _ea;
        private ILangTextRepoClient _langTextRepo;
        private ILangFile _langFile;
        private IBackendService _backendService;

        public MainWindowSearchbarViewModel(IEventAggregator ea, ILangTextRepoClient langTextRepoClient,
            ILangFile LangFile, IBackendService backendService)
        {
            _ea = ea;
            _langTextRepo = langTextRepoClient;
            _langFile = LangFile;
            _backendService = backendService;

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

                if (_isLoadJp)
                {
                    foreach (var lang in result)
                    {
                        //string langJp;

                        if(_jpLangDict.TryGetValue(lang.TextId, out string langJp))
                        {
                            lang.TextJp = langJp;
                        }
                        
                        //lang.TextJp = _jpLangDict[lang.TextId];
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
            //MessageBox.Show(obj.ToString());

            bool isChecked = (bool)obj;

            List<string> luaFileList = new List<string>
            {
                @"Data\jp_client.lua",
                @"Data\jp_pregame.lua"
            };

            if (File.Exists(@"Data\jp.lang"))
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
                var dialogResult = MessageBox.Show("没有找到日语本地化文件，现在是否要下载？", "错误", 
                    MessageBoxButton.YesNo, MessageBoxImage.Error);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    await DownloadJpFiles();
                }
            }
        }

        private async Task DownloadJpFiles()
        {
            LoadJpLangCommand.IsExecuting = true;

            var serverConfig = await _backendService.GetServerRespondAndConfig();

            _backendService.SetSha256 += SetAppConfigClientJpLangSha256;
            await _backendService.DownloadFileFromServer(App.ServerPath + serverConfig.LangJpPackPath,
                serverConfig.LangJpPackPath, serverConfig.LangJpPackSHA256);
        }

        private void SetAppConfigClientJpLangSha256(object sender, string e)
        {
            _backendService.SetSha256 -= SetAppConfigClientJpLangSha256;

            App.LangConfig.LangJpSha256 = e;
            AppConfigClient.Save(App.LangConfig);

            LoadJpLangCommand.IsExecuting = false;
            _ea.GetEvent<ConnectProgressString>().Publish("文件下载完成！");

            Task.Run(() => LoadJpLang(true));
        }
    }
}
