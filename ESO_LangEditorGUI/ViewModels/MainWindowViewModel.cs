using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using ESO_LangEditor.GUI.NetClient;
using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.EventAggres;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.Views;
using ESO_LangEditorGUI.Views.UserControls;
using MaterialDesignThemes.Wpf;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
        private LangTextDto _gridSelectedItem;
        private List<LangTextDto> _gridSelectedItems;


        public UC_LangDataGrid LangDataGrid { get; set; }

        public DataGridViewModel LangDataGridView { get; }

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

            MainWindowMessageQueue = new SnackbarMessageQueue();

            //_ea.GetEvent<DataGridSelectedItemCount>().Subscribe(DataGridSelectedCount);

            //_mainWindow.RootDialogWindow.Loaded += RootDialogWindow_Loaded;
            //


            //ExportToLangCommand = new ExportToFileCommand(this);
        }

        public void DataGridSelectedCount(List<LangTextDto> langtextList)
        {
            SelectedInfo = "已选择 " + langtextList.Count + " 条文本";
        }

        public void LangtextDataSelected(List<LangTextDto> obj)
        {
            //SelectedInfo = "已选择 " + obj.Count + " 项";
        }

        //private void RootDialogWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    GuidVaildStartupCheck();
        //}

        //public void GuidVaildStartupCheck()
        //{
        //    if (App.LangConfig.UserGuid == Guid.Empty)
        //    {
        //        ShowLoginUC();
        //    }
        //    else
        //    {
        //        //var startCheck = new StartupConfigCheck(this);
        //        //startCheck.TryToGetServerRespondAndConfig();
        //        //App.LangNetworkService = new NetworkService(App.LangConfig, this);
        //    }
        //}

        private async void ShowLoginUC()
        {
            var view = new UC_Login(this);

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        public async void ShowImportDbRevUC(bool isDbRev)
        {
            var view = new ImportDbRevProgressDialog(isDbRev);

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

        private async void ExportToLang(object o)
        {
            var view = new ProgressDialog();

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

        }

        public async void OpenLangEditor(LangTextDto langtext)
        {
            var view = new LangtextEditor(langtext);
            view.Show();

            //show the dialog
           // var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

        }


        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        private void ClosingEventAndSaveConfigHandler(object sender, DialogClosingEventArgs eventArgs)
        {


            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        private void ExportLuaToStr(object o)
        {
            var readDb = new LangTextRepository();
            var export = new ExportDbToFile();

            var langlua = readDb.GetAlltLangTexts(1);
            export.ExportLua(langlua);

            MessageBox.Show("导出完成！");
        }

        private bool CompareDatabaseRev()
        {
            return App.LangConfig.DatabaseRev != App.LangConfigServer.LangDatabaseRevised;
        }

        private void LangtextDataReceived(List<LangTextDto> langtexList)
        {
            if (GridData.Count > 0)
                GridData.Clear();

            GridData.AddRange(langtexList);
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
