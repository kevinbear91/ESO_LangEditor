using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.View.UserControls;
using ESO_LangEditorLib.Models.Client.Enum;
using ESO_LangEditorLib.Services.Client;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        //private List<DataGridTextColumn> _dataGridColumnHeader;
        private string _keyword;
        private bool _isLuaChecked;

        public UC_LangDataGrid LangDataGrid { get; set; }

        public DataGridViewModel LangDataGridView { get; }

        //public UC_LangDataGrid LangDataGrid 
        //{ 
        //    get  { return _langDataGrid; }
        //    set { }

        //}

        public ICommand MainviewCommand { get; }

        public ICommand SearchLangCommand { get; }

        public ICommand ExportToLangCommand { get; }

        public ICommand RunDialogCommand => new LangOpenDialogCommand(ExecuteRunDialog);



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

        //public List<DataGridTextColumn> DataGridColumnHeader
        //{
        //    get { return _dataGridColumnHeader; }
        //    set { _dataGridColumnHeader = value; NotifyPropertyChanged(); }
        //}

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



        public MainWindowViewModel(UC_LangDataGrid langdatagrid)
        {
            LoadMainView();
            LangDataGrid = langdatagrid;
            LangDataGrid.MainWindowViewModel = this;
            LangDataGrid.LangDatGridinWindow = LangDataGridInWindow.MainViewWindow;
            //LangDataGridView = new DataGridViewModel();
            SearchLangCommand = new SearchLangCommand(this);

            ProgressbarVisibility = Visibility.Hidden;

            ExportToLangCommand = new ExportToFileCommand(this);
        }

        private void LoadMainView()
        {
            WindowTitle = "ESO文本查询编辑器" + version;
            SelectedInfo = "无选择条目";
            //SearchInfo = "暂无查询";

            //_keyword = "148ed451-bf19-43e9-a8d3-55f922cd349e";

        }

        private async void ExportToLang()
        {
            var _langTextRepository = new LangTextRepository();
            var _thirdPartSerices = new ThirdPartSerices();
            var exportDbToFile = new ExportDbToFile();
            
            MessageBoxResult resultExport = MessageBox.Show("一键导出文本到.lang文件，点击确定开始，不导出请点取消。"
                + Environment.NewLine
                + "点击确定之后请耐心等待，输出完毕后会弹出提示!", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            switch (resultExport)
            {
                case MessageBoxResult.OK:
                    ProgressInfo = "正在读取数据库……";
                    ProgressbarVisibility = Visibility.Visible;
                    var langtexts = await Task.Run(() => _langTextRepository.GetAlltLangTexts(0));

                    ProgressInfo = "正在写入文件……";
                    await Task.Run(() => exportDbToFile.ExportText(langtexts));

                    ProgressInfo = "正在转换格式……";
                    await Task.Run(() => _thirdPartSerices.ConvertTxTtoLang(false));

                    ProgressInfo = "";
                    ProgressbarVisibility = Visibility.Hidden;
                    MessageBox.Show("导出完成!", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private async void ExecuteRunDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new ProgressDialog
            {
                //DataContext = new SampleDialogViewModel()
            };


            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            //...now, lets update the "session" with some new content!
            //eventArgs.Session.UpdateContent(new SampleProgressDialog());
            //note, you can also grab the session when the dialog opens via the DialogOpenedEventHandler

            //lets run a fake operation for 3 seconds then close this baby.
            //Task.Delay(TimeSpan.FromSeconds(3))
            //    .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
            //        TaskScheduler.FromCurrentSynchronizationContext());
        }


        //public void GetLangSearchInfo(int count)
        //{
        //    SearchInfo = "查询到 " + count.ToString() + " 条文本";
        //}

    }
}
