using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.View.UserControls;
using ESO_LangEditorLib.Models.Client.Enum;
using ESO_LangEditorLib.Services.Client;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly string version = " v3.0.0";

        private string _windowTitle;
        private IEnumerable<SearchPostion> _searchPostion;
        private string _searchInfo;
        private string _selectedInfo;
        private string _progressInfo;
        private Visibility _progessbarVisibility;
        private string _keyword;
        private bool _isLuaChecked;
        private MainWindow _mainWindow;

        public UC_LangDataGrid LangDataGrid { get; set; }

        public DataGridViewModel LangDataGridView { get; }

        public ICommand MainviewCommand { get; }

        public ICommand SearchLangCommand { get; }

        public ICommand ExportToLangCommand { get; }

        //public ICommand RunDialogCommand => new LangOpenDialogCommand(ExecuteRunDialog);

        public ICommand OpenDoalogCommand => new LangOpenDialogCommand(ExportToLang);




        public string WindowTitle
        {
            get { return _windowTitle; }
            set { _windowTitle = value; NotifyPropertyChanged(); }
        }

        public SearchPostion SelectedSearchPostion { get; set; }

        public IEnumerable<SearchPostion> SearchPostion
        {
            get { return Enum.GetValues(typeof(SearchPostion)).Cast<SearchPostion>(); }
            //set { _searchPostion =  }
        }

        public SearchTextType SelectedSearchTextType { get; set; }

        public IEnumerable<SearchTextType> SearchTextType
        {
            get { return Enum.GetValues(typeof(SearchTextType)).Cast<SearchTextType>(); }
            //set { _searchTextType = value; NotifyPropertyChanged(); }
        }

        public string SearchInfo
        {
            get { return _searchInfo; }
            set { _searchInfo = "查询到 " + value + " 条文本"; ; NotifyPropertyChanged(); }
        }

        public string SelectedInfo
        {
            get { return _selectedInfo; }
            set { _selectedInfo = value; NotifyPropertyChanged(); }
        }

        public string ProgressInfo
        {
            get { return _progressInfo; }
            set { _progressInfo = value; NotifyPropertyChanged(); }
        }

        public Visibility ProgressbarVisibility
        {
            get { return _progessbarVisibility; }
            set { _progessbarVisibility = value; NotifyPropertyChanged(); }
        }

        public string Keyword
        {
            get { return _keyword; }
            set { _keyword = value; NotifyPropertyChanged(); }
        }

        public bool IsLuaChecked
        {
            get { return _isLuaChecked; }
            set { _isLuaChecked = value; NotifyPropertyChanged(); }
        }


        public MainWindowViewModel(UC_LangDataGrid langdatagrid, MainWindow mainWindow)
        {
            LoadMainView();
            LangDataGrid = langdatagrid;
            LangDataGrid.MainWindowViewModel = this;
            LangDataGrid.LangDatGridinWindow = LangDataGridInWindow.MainViewWindow;
            _mainWindow = mainWindow;
            //LangDataGridView = new DataGridViewModel();
            SearchLangCommand = new SearchLangCommand(this);

            ProgressbarVisibility = Visibility.Hidden;

            

            _mainWindow.RootDialogWindow.Loaded += RootDialogWindow_Loaded;
            //


            //ExportToLangCommand = new ExportToFileCommand(this);
        }

        private void RootDialogWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GuidVaildStartupCheck();
        }

        public void GuidVaildStartupCheck()
        {
            if (App.LangConfig.UserGuid == Guid.Empty)
            {
                ShowLoginUC();
            }
            else
            {
                App.LangNetworkService = new NetworkService(App.LangConfig, this);
            }
        }

        private async void ShowLoginUC()
        {
            var view = new UC_Login(this);

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        public async void ShowImportDbRevUC()
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

        private async void ExportToLang(object o)
        {
            var view = new ProgressDialog();

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        private void ClosingEventAndSaveConfigHandler(object sender, DialogClosingEventArgs eventArgs)
        {


            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        private void ExportLuaToStr()
        {
            var readDb = new LangTextRepository();
            var export = new ExportDbToFile();

            var langlua = readDb.GetAlltLangTexts(1);
            export.ExportLua(langlua);
        }

        private bool CompareDatabaseRev()
        {
            return App.LangConfig.DatabaseRev != App.LangConfigServer.LangDatabaseRevised;
        }


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
