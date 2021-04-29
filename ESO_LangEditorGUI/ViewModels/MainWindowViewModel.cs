using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using ESO_LangEditor.GUI.NetClient;
using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.EventAggres;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.Services.AccessServer;
using ESO_LangEditorGUI.Views;
using ESO_LangEditorGUI.Views.UserControls;
using MaterialDesignThemes.Wpf;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ESO_LangEditorGUI.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly string version = " " + App.LangConfig.LangEditorVersion;

        private string _windowTitle;
        private string _searchInfo;
        private string _selectedInfo;
        private string _progressInfo;
        private Visibility _progessbarVisibility;
        private string _keyword;
        private ObservableCollection<LangTextDto> _gridData;
        private ClientConnectStatus _connectStatus;
        private AccountService _accountService;
        private string _avatarPath = App.UserAvatarPath;
        private bool firstTime = true;

        public ICommand MainviewCommand { get; }
        public ICommand SearchLangCommand { get; }
        public ICommand ExportToLangCommand { get; }
        //public ICommand RunDialogCommand => new LangOpenDialogCommand(ExecuteRunDialog);
        public ICommand OpenDoalogCommand => new LangOpenDialogCommand(ExportToLang);
        //public ICommand DataGridDoubleClickCommand => new LangOpenDialogCommand(OpenLangEditor);
        public ExcuteViewModelMethod ExportLuaToStrCommand => new ExcuteViewModelMethod(ExportLuaToStr);


        public string WindowTitle
        {
            get { return _windowTitle; }
            set { SetProperty(ref _windowTitle, value); }
            //set { _windowTitle = value; NotifyPropertyChanged(); }
        }


        public string SearchInfo
        {
            get { return _searchInfo; }
            set { SetProperty(ref _searchInfo, "查询到 " + value + " 条文本"); }
                //set { _searchInfo = "查询到 " + value + " 条文本"; ; NotifyPropertyChanged(); }
            }

        public string SelectedInfo
        {
            get { return _selectedInfo; }
            set { SetProperty(ref _selectedInfo, value); }
        }

        public string ProgressInfo
        {
            get { return _progressInfo; }
            set { SetProperty(ref _progressInfo, value); }
        }

        public Visibility ProgressbarVisibility
        {
            get { return _progessbarVisibility; }
            set { SetProperty(ref _progessbarVisibility, value); }
        }

        public string Keyword
        {
            get { return _keyword; }
            set { SetProperty(ref _keyword, value); }
        }

        public ObservableCollection<LangTextDto> GridData
        {
            get { return _gridData; }
            set { SetProperty(ref _gridData, value); }
        }

        public ClientConnectStatus ConnectStatus
        {
            get { return _connectStatus; }
            set { SetProperty(ref _connectStatus, value); }
        }

        public string AvatarPath
        {
            get { return _avatarPath; }
            //set { SetProperty(ref _connectStatus, value); }
        }

        public LangTextDto GridSelectedItem { get; set; }

        public List<LangTextDto> GridSelectedItems { get; set; }

        //public DelegateCommand GetCommand

        IEventAggregator _ea;
        public SnackbarMessageQueue MainWindowMessageQueue { get; }
        public event EventHandler OnRequestClose;
        public event EventHandler CloseDrawerHostEvent;

        public MainWindowViewModel(IEventAggregator ea)
        {
            LoadMainView();
            GridData = new ObservableCollection<LangTextDto>();
            
            //_langTextSearch = new LangTextSearchService(App.RepoClient, App.Mapper);

            _ea = ea;
            _ea.GetEvent<LangtextPostToMainDataGrid>().Subscribe(LangtextDataReceived);
            _ea.GetEvent<MainDataGridSelectedItemsToMainWindowVM>().Subscribe(LangtextDataSelected);
            _ea.GetEvent<SendMessageQueueToMainWindowEventArgs>().Subscribe(ShowMessageQueueWithString);
            _ea.GetEvent<CloseMainWindowDrawerHostEvent>().Subscribe(CloseDrawerHost);
            _ea.GetEvent<ConnectProgressString>().Subscribe(UpdateProgressInfo);
            _ea.GetEvent<LoginRequiretEvent>().Subscribe(ShowLoginUC);
            _ea.GetEvent<ConnectStatusChangeEvent>().Subscribe(ChangeConnectStatus);
            _ea.GetEvent<UploadLangtextZhUpdateEvent>().Subscribe(UploadLangtextZhUpdate);
            _ea.GetEvent<UploadLangtextZhListUpdateEvent>().Subscribe(UploadLangtextZhUpdate);
            _ea.GetEvent<InitUserRequired>().Subscribe(OpenUserProfileSettingWindow);
            _ea.GetEvent<DatabaseUpdateEvent>().Subscribe(ShowImportDbRevUC);
            _ea.GetEvent<UserAvatarDownloadCompleteEvent>().Subscribe(UserAvatarDownloadComplete);
            _ea.GetEvent<LogoutEvent>().Subscribe(UserLogout);

            MainWindowMessageQueue = new SnackbarMessageQueue();
            _accountService = new AccountService(_ea);

            Debug.WriteLine(AvatarPath);

            //ConnectStatus = ClientConnectStatus.Login;
            //_ea.GetEvent<DataGridSelectedItemCount>().Subscribe(DataGridSelectedCount);

            //_mainWindow.RootDialogWindow.Loaded += RootDialogWindow_Loaded;
            //

            //ExportToLangCommand = new ExportToFileCommand(this);
        }

        private void UserLogout()
        {
            App.LangConfig.UserAuthToken = "";
            App.LangConfig.UserRefreshToken = "";
            AppConfigClient config = App.LangConfig;
            AppConfigClient.Save(config);

            ConnectStatus = ClientConnectStatus.Logout;
        }

        private void UserAvatarDownloadComplete(string filename)
        {
            if (filename == App.UserAvatarPath)
            {
                App.UserAvatarPath = filename;
            }
        }

        private void OpenUserProfileSettingWindow()
        {
            new UserProfileSetting().Show();
        }

        private async void ChangeConnectStatus(ClientConnectStatus connectStatus)
        {
            ConnectStatus = connectStatus;

            if (connectStatus == ClientConnectStatus.Login && firstTime)
            {
                TimerForRefreshToken();
                
                if (App.User.UserAvatarPath != null && App.User.UserAvatarPath != "")
                {
                    if(!Directory.Exists(App.WorkingDirectory + "/_tmp"))
                    {
                        Directory.CreateDirectory(App.WorkingDirectory + "/_tmp");
                    }

                    var avatarPath = App.WorkingDirectory + "/_tmp/" + App.User.UserAvatarPath;
                    Debug.WriteLine("avatar path : {0}" + avatarPath);

                    if (App.UserAvatarPath != avatarPath)
                    {
                        //var filename = App.User.UserAvatarPath;
                        Debug.WriteLine("download avatar file: {0}", App.User.UserAvatarPath);

                        await _accountService.UserAvatarDownload(App.User);

                        App.LangConfig.UserAvatarPath = App.User.UserAvatarPath;
                        AppConfigClient config = App.LangConfig;
                        AppConfigClient.Save(config);

                    }
                }

                firstTime = false;
            }

            if(connectStatus == ClientConnectStatus.Login)
            {
                App.OnlineMode = true;
            }
            else
            {
                App.OnlineMode = false;
            }
        }

        private async void UploadLangtextZhUpdate(LangTextForUpdateZhDto langTextUpdateZhDto)
        {
            LangtextNetService apiLangtext = new LangtextNetService(App.ServerPath);
            LangTextRepoClientService langTextRepoClient = new LangTextRepoClientService();
            var code = await apiLangtext.UpdateLangtextZh(langTextUpdateZhDto, App.LangConfig.UserAuthToken);

            Debug.WriteLine("langID: {0}, langZh: {1}", langTextUpdateZhDto.Id, langTextUpdateZhDto.TextZh);
            
            if (code == System.Net.HttpStatusCode.OK || 
                code == System.Net.HttpStatusCode.Accepted ||
                code == System.Net.HttpStatusCode.Created)
            {
                langTextUpdateZhDto.IsTranslated = 3;
                await langTextRepoClient.UpdateLangtextZh(langTextUpdateZhDto);
            }

            MainWindowMessageQueue.Enqueue("状态码：" + code.ToString());

        }

        private async void UploadLangtextZhUpdate(List<LangTextForUpdateZhDto> langTextForUpdateZhDtoList)
        {
            LangtextNetService apiLangtext = new LangtextNetService(App.ServerPath);
            LangTextRepoClientService langTextRepoClient = new LangTextRepoClientService();
            var code = await apiLangtext.UpdateLangtextZh(langTextForUpdateZhDtoList, App.LangConfig.UserAuthToken);

            if (code == System.Net.HttpStatusCode.OK ||
                code == System.Net.HttpStatusCode.Accepted ||
                code == System.Net.HttpStatusCode.Created)
            {
                foreach(var lang in langTextForUpdateZhDtoList)
                {
                    lang.IsTranslated = 3;
                }
                await langTextRepoClient.UpdateLangtextZh(langTextForUpdateZhDtoList);
            }

            MainWindowMessageQueue.Enqueue("状态码：" + code.ToString());

        }

        private void UpdateProgressInfo(string str)
        {
            ProgressInfo = str;
        }

        public void DataGridSelectedCount(List<LangTextDto> langtextList)
        {
            SelectedInfo = "已选择 " + langtextList.Count + " 条文本";
        }

        public void LangtextDataSelected(List<LangTextDto> obj)
        {
            //SelectedInfo = "已选择 " + obj.Count + " 项";
        }

        public async void RootDialogWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await GuidVaildStartupCheck();
        }

        public async Task GuidVaildStartupCheck()
        {
            var startCheck = new StartupConfigCheck(_ea);
            await startCheck.TryToGetServerRespondAndConfig();

            

            
        }

        private async void ShowLoginUC()
        {
            var view = new UC_Login(_accountService);

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        public async void ShowImportDbRevUC(bool isDbRev)
        {
            var view = new ImportDbRevProgressDialog();

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        private void LoadMainView()
        {
            WindowTitle = "ESO文本查询编辑器" + version;
            SelectedInfo = "无选择条目";
            //SearchInfo = "暂无查询";

            //_keyword = "148ed451-bf19-43e9-a8d3-55f922cd349e";

        }

        private async void TimerForRefreshToken()
        {
            while (ConnectStatus == ClientConnectStatus.Login)
            {
                await Task.Delay(TimeSpan.FromMinutes(10));

                await Task.Run(() =>
                {
                     _accountService.LoginByToken();
                });
                
            }
        }

        private async void ExportToLang(object o)
        {
            var view = new ProgressDialog();

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

        }

        public async void OpenLangEditor(LangTextDto langtext)
        {
            var view = new LangtextEditor(langtext)
            {
                Owner = Application.Current.MainWindow
            };
            view.Show();

            //show the dialog
           // var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

        }


        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }



        private async void ExportLuaToStr(object o)
        {
            var readDb = new LangTextRepoClientService();
            var export = new ExportDbToFile();

            var langlua = await readDb.GetAlltLangTexts(1);
            export.ExportLua(langlua);

            MessageBox.Show("导出完成！");
        }

        //private bool CompareDatabaseRev()
        //{
        //    return App.LangConfig.DatabaseRev != App.LangConfigServer.LangDatabaseRevised;
        //}

        private void LangtextDataReceived(List<LangTextDto> langtexList)
        {
            if (GridData.Count > 0)
                GridData.Clear();

            GridData.AddRange(langtexList);
            SearchInfo = langtexList.Count.ToString();
        }

        private void ShowMessageQueueWithString(string str)
        {
            MainWindowMessageQueue.Enqueue(str);
        }

        private void CloseDrawerHost()
        {
            CloseDrawerHostEvent(this, new EventArgs());
        }

        public void EventHookTester()
        {
            MessageBox.Show("Respond From EventHook");
        }

        //public void EventHookWithParmTester(List<LangTextDto> langtextList)
        //{
        //    var list = o as List<LangTextDto>;

        //    SelectedInfo = langtextList.Count.ToString();

        //    MessageBox.Show("Respond From EventHook, parm: {0}", langtextList.Count.ToString());

        //}

        //private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        //{
        //    Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        //}

        //private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        //{
        //    if ((bool)eventArgs.Parameter == false) return;

        //    eventArgs.Cancel();

        //}


        //public void GetLangSearchInfo(int count)
        //{
        //    SearchInfo = "查询到 " + count.ToString() + " 条文本";
        //}

    }
}
