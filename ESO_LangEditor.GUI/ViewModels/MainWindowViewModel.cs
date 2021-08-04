using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.EventAggres;
using ESO_LangEditor.GUI.Services;
using ESO_LangEditor.GUI.Views;
using ESO_LangEditor.GUI.Views.UserControls;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ESO_LangEditor.GUI.ViewModels
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
        //private AccountService _accountService;
        private IEventAggregator _ea;
        private ILogger _logger;
        private IStartupCheck _startupCheck;
        private IBackendService _backendService;
        

        public ICommand MainviewCommand { get; }
        public ICommand SearchLangCommand { get; }
        public ICommand ExportToLangCommand { get; }
        //public ICommand RunDialogCommand => new LangOpenDialogCommand(ExecuteRunDialog);
        public ICommand OpenDoalogCommand => new LangOpenDialogCommand(ExportToLang);
        //public ICommand DataGridDoubleClickCommand => new LangOpenDialogCommand(OpenLangEditor);
        public ExcuteViewModelMethod ExportLuaToStrCommand => new ExcuteViewModelMethod(ExportLuaToStr);


        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value); 
            //set { _windowTitle = value; NotifyPropertyChanged(); }
        }


        public string SearchInfo
        {
            get => _searchInfo;
            set => SetProperty(ref _searchInfo, "查询到 " + value + " 条文本"); 
                //set { _searchInfo = "查询到 " + value + " 条文本"; ; NotifyPropertyChanged(); }
            }

        public string SelectedInfo
        {
            get => _selectedInfo;
            set => SetProperty(ref _selectedInfo, value);
        }

        public string ProgressInfo
        {
            get => _progressInfo;
            set => SetProperty(ref _progressInfo, value);
        }

        public Visibility ProgressbarVisibility
        {
            get => _progessbarVisibility;
            set => SetProperty(ref _progessbarVisibility, value);
        }

        public string Keyword
        {
            get => _keyword;
            set => SetProperty(ref _keyword, value);
        }

        public ObservableCollection<LangTextDto> GridData
        {
            get => _gridData;
            set => SetProperty(ref _gridData, value);
        }

        public ClientConnectStatus ConnectStatus
        {
            get => _connectStatus;
            set => SetProperty(ref _connectStatus, value);
        }


        public LangTextDto GridSelectedItem { get; set; }

        public List<LangTextDto> GridSelectedItems { get; set; }

        public Visibility ReasonForVisibility => Visibility.Collapsed;

        public SnackbarMessageQueue MainWindowMessageQueue { get; }
        public event EventHandler OnRequestClose;
        public event EventHandler CloseDrawerHostEvent;

        public MainWindowViewModel(IEventAggregator ea, ILogger logger, 
            IStartupCheck startupCheck, IBackendService backendService)
        {
            LoadMainView();
            GridData = new ObservableCollection<LangTextDto>();
            
            //_langTextSearch = new LangTextSearchService(App.RepoClient, App.Mapper);

            _ea = ea;
            _logger = logger;
            _startupCheck = startupCheck;
            _backendService = backendService;


            _ea.GetEvent<LangtextPostToMainDataGrid>().Subscribe(LangtextDataReceived);
            _ea.GetEvent<MainDataGridSelectedItemsToMainWindowVM>().Subscribe(LangtextDataSelected);
            _ea.GetEvent<SendMessageQueueToMainWindowEventArgs>().Subscribe(ShowMessageQueueWithString);
            _ea.GetEvent<CloseMainWindowDrawerHostEvent>().Subscribe(CloseDrawerHost);
            _ea.GetEvent<ConnectProgressString>().Subscribe(UpdateProgressInfo);
            _ea.GetEvent<LoginRequiretEvent>().Subscribe(ShowLoginUC);
            _ea.GetEvent<ConnectStatusChangeEvent>().Subscribe(ChangeConnectStatus);
            //_ea.GetEvent<UploadLangtextZhUpdateEvent>().Subscribe(UploadLangtextZhUpdate);
            //_ea.GetEvent<UploadLangtextZhListUpdateEvent>().Subscribe(UploadLangtextZhUpdate);
            _ea.GetEvent<InitUserRequired>().Subscribe(OpenUserProfileSettingWindow);
            _ea.GetEvent<DatabaseUpdateEvent>().Subscribe(ShowImportDbRevUC);
            _ea.GetEvent<LogoutEvent>().Subscribe(UserLogout);
            _ea.GetEvent<LoginFromUcEvent>().Subscribe(LoginTaskCallFromUC);

            MainWindowMessageQueue = new SnackbarMessageQueue();


            //ConnectStatus = ClientConnectStatus.Login;
            //_ea.GetEvent<DataGridSelectedItemCount>().Subscribe(DataGridSelectedCount);

            //_mainWindow.RootDialogWindow.Loaded += RootDialogWindow_Loaded;
            //


            logger.LogDebug("======启动至主界面======");
            //logger.Error("======报错测试======");

            Task.Run(() => BootCheck());
        }

        private async Task BootCheck()
        {
            var startupCheckList = Task.Run(() => _startupCheck.StartupTaskList());
            //var continuation =  startupCheckList.ContinueWith(syncUser => _backendService.SyncUser());

            try
            {
                await startupCheckList;
                //await continuation;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
            }

            //_startupCheck = null;
        }

        private async void LoginTaskCallFromUC()
        {
            await _startupCheck.LoginTaskList();
        }

        private void UserLogout()
        {
            App.LangConfig.UserAuthToken = "";
            App.LangConfig.UserRefreshToken = "";
            AppConfigClient config = App.LangConfig;
            AppConfigClient.Save(config);

            ConnectStatus = ClientConnectStatus.Logout;
        }

        private void OpenUserProfileSettingWindow()
        {
            new UserProfileSetting().Show();
        }

        private void ChangeConnectStatus(ClientConnectStatus connectStatus)
        {
            ConnectStatus = connectStatus;
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
            //await GuidVaildStartupCheck();
        }


        private async void ShowLoginUC()
        {
            var view = new UC_Login();

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

        //private async void TimerForRefreshToken()
        //{
        //    while (ConnectStatus == ClientConnectStatus.Login)
        //    {
        //        await Task.Delay(TimeSpan.FromMinutes(10));

        //        await Task.Run(() =>
        //        {
        //             _accountService.LoginByToken();
        //        });
                
        //    }
        //}

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
            //var readDb = new LangTextRepoClientService();
            //var export = new ExportDbToFile();

            //var langlua = await readDb.GetAlltLangTexts(1);
            //export.ExportLua(langlua);

            //MessageBox.Show("导出完成！");
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
