using Core.EnumTypes;
using Core.Models;
using Core.Entities;
using GUI;
using GUI.Command;
using GUI.Views;
using GUI.Views.UserControls;
using GUI.EventAggres;
using GUI.Services;
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

namespace GUI.ViewModels
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
        private IEventAggregator _ea;
        private ILogger _logger;
        private IStartupCheck _startupCheck;
        private Visibility _jpVisibility = Visibility.Collapsed;

        public ICommand MainviewCommand { get; }
        public ICommand SearchLangCommand { get; }

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
            get => App.ConnectStatus;
            set => SetProperty(ref App.ConnectStatus, value);
        }

        public Visibility JpVisibility
        {
            get => _jpVisibility;
            set => SetProperty(ref _jpVisibility, value);
        }

        public LangTextDto GridSelectedItem { get; set; }

        public List<LangTextDto> GridSelectedItems { get; set; }

        public Visibility ReasonForVisibility => Visibility.Collapsed;


        public SnackbarMessageQueue MainWindowMessageQueue { get; }
        public event EventHandler OnRequestClose;
        public event EventHandler CloseDrawerHostEvent;

        public MainWindowViewModel(IEventAggregator ea, ILogger logger,
            IStartupCheck startupCheck)
        {
            LoadMainView();
            GridData = new ObservableCollection<LangTextDto>();

            _ea = ea;
            _logger = logger;
            _startupCheck = startupCheck;

            _ea.GetEvent<LangtextPostToMainDataGrid>().Subscribe(LangtextDataReceived);
            _ea.GetEvent<MainDataGridSelectedItemsToMainWindowVM>().Subscribe(LangtextDataSelected);
            _ea.GetEvent<SendMessageQueueToMainWindowEventArgs>().Subscribe(ShowMessageQueueWithString);
            _ea.GetEvent<CloseMainWindowDrawerHostEvent>().Subscribe(CloseDrawerHost);
            _ea.GetEvent<ConnectProgressString>().Subscribe(UpdateProgressInfo);
            _ea.GetEvent<LoginRequiretEvent>().Subscribe(ShowLoginUC);
            _ea.GetEvent<ConnectStatusChangeEvent>().Subscribe(ChangeConnectStatus);
            _ea.GetEvent<DatabaseUpdateEvent>().Subscribe(ShowImportDbRevUC);
            _ea.GetEvent<LogoutEvent>().Subscribe(UserLogout);
            _ea.GetEvent<LoginFromUcEvent>().Subscribe(LoginTaskCallFromUC);
            _ea.GetEvent<JpColumnVisibilityEvent>().Subscribe(SetJpCplumnVisibility);

            MainWindowMessageQueue = new SnackbarMessageQueue();


            //ConnectStatus = ClientConnectStatus.Login;
            //_ea.GetEvent<DataGridSelectedItemCount>().Subscribe(DataGridSelectedCount);

            //_mainWindow.RootDialogWindow.Loaded += RootDialogWindow_Loaded;
            //


            logger.LogDebug("======启动至主界面======");
            //logger.Error("======报错测试======");


        }

        private void SetJpCplumnVisibility(Visibility obj)
        {
            JpVisibility = obj;
        }

        private async Task BootCheck()
        {
            var startupCheckList = Task.Run(() => _startupCheck.StartupTaskList());
            //var continuation =  startupCheckList.ContinueWith(syncUser => _backendService.SyncUser());

            try
            {
                await startupCheckList;
                await _startupCheck.Login();
                //await continuation;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
            }

            _logger.LogDebug("======启动检查完成======");


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
            await BootCheck();
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

    }
}
