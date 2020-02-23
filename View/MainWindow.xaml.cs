using ESO_Lang_Editor.Model;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //private MainWindowOption windowsOptions;
        List<LangSearchModel> SearchData;
        List<LangSearchModel> SelectedDatas;
        LangSearchModel SelectedData;

        ObservableCollection<string> searchTextInPosition;
        ObservableCollection<string> searchTextType;

        bool Searchabandon = false;
        

        public MainWindow()
        {
            //windowsOptions = new MainWindowOption();
            //DataContext = windowsOptions;
            InitializeComponent();
            SearchTextBox.IsEnabled = false;
            SearchButton.IsEnabled = false;


            SearchTextInPositionInit();
            SearchTextTypeInit();
            

            string version = " beta 0.8 - 6843a764";

            Title = "ESO文本查询编辑器" + version;

            textBlock_Info.Text = "";
            
            UpdatedDB_Check();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchDB();
        }

        public List<LangSearchModel> SearchLang(string SearchBarText)
        {
            var DBFile = new SQLiteController();
            SearchData = null;

            var da1 = DBFile.SearchData(SearchBarText, SearchField(), Searchabandon);

            return da1;
        }

        private void LangSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid datagrid = sender as DataGrid;
            Point aP = e.GetPosition(datagrid);
            IInputElement obj = datagrid.InputHitTest(aP);
            DependencyObject target = obj as DependencyObject;

            if (SelectedDatas != null)
                SelectedDatas.Clear();

            while (target != null)
            {
                if (target is DataGridRow)
                    if (datagrid.SelectedIndex != -1)
                    {
                        SelectedData = (LangSearchModel)datagrid.SelectedItem;
                        TextEditor textEditor = new TextEditor(SelectedData, SelectedDatas);
                        textEditor.Show();
                        //MessageBox.Show(data.Text_SC);

                    }

                target = VisualTreeHelper.GetParent(target);
            }
        }
        private string SearchCheck()
        {
            //获得已选择的搜索文本所在位置， 默认selectedSearchTextPosition = 0
            int selectedSearchTextPosition = SearchTextPositionComboBox.SelectedIndex;
            string searchText = SearchTextBox.Text;
            string SearchContent;

            switch (selectedSearchTextPosition)
            {
                case 0:
                    SearchContent = "%" + searchText + "%";   //全文搜索
                    break;
                case 1:
                    SearchContent = searchText + "%";         //仅在开头
                    break;
                case 2:
                    SearchContent = "%" + searchText;         //仅在结尾
                    break;
                default:
                    SearchContent = "%" + searchText + "%";   //出错直接全文搜索
                    break;
            }
            return SearchContent;
        }

        private int SearchField()
        {
            //搜索字段， 默认selectedSearchTextPosition = 1

            var selectedSearchType = SearchTypeComboBox.SelectedIndex;
            int searchField;

            switch (selectedSearchType)
            {
                case 0:
                    searchField = 0;   //搜ID
                    break;
                case 1:
                    searchField = 1;   //搜英文
                    break;
                case 2:
                    searchField = 2;   //搜汉化
                    break;
                case 3:
                    searchField = 3;   //搜是否已翻译
                    break;
                default:
                    searchField = 1;   //出错搜英文
                    break;
            }
            return searchField;
        }

        private void CreateDB_Click(object sender, RoutedEventArgs e)
        {
            var createDBWindow = new CreateDB_ImportCSV();

            createDBWindow.Show();

        }

        private void CsvFileCompare_Click(object sender, RoutedEventArgs e)
        {
            var compareCsvWindows = new CompareCsvWindow();
            compareCsvWindows.Show();
        }

        private void CsvCompareWithDB_Click(object sender, RoutedEventArgs e)
        {
            var compareWithDBWindows = new CompareWithDBWindow();
            compareWithDBWindows.Show();
        }

        private void ExportTranslate_Click(object sender, RoutedEventArgs e)
        {
            var exportTranslateWindow = new ExportTranslate();
            exportTranslateWindow.Show();
        }


        private void ExportToText_Click(object sender, RoutedEventArgs e)
        {
            var export = new ExportFromDB();
            MessageBoxResult result = MessageBox.Show("输出数据库的文本内容至Text,文件名分别为ID.txt 与Text. txt"
                + Environment.NewLine
                + "其中ID文件为合并ID, Text为内容。"
                + "点击确定开始输出，不导出请点取消。"
                + Environment.NewLine
                + "点击确定之后请耐心等待，输出完毕后会弹出提示!", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            switch (result)
            {
                case MessageBoxResult.OK:
                    export.ExportAsText();
                    MessageBox.Show("导出完成!", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void SearchTextInPositionInit()
        {
            searchTextInPosition = new ObservableCollection<string>();

            searchTextInPosition.Add("包含全文");
            searchTextInPosition.Add("仅包含开头");
            searchTextInPosition.Add("仅包含结尾");

            SearchTextPositionComboBox.ItemsSource = searchTextInPosition;
            SearchTextPositionComboBox.SelectedIndex = 0;
        }

        private void SearchTextTypeInit()
        {
            searchTextType = new ObservableCollection<string>();

            searchTextType.Add("搜编号");
            searchTextType.Add("搜英文");
            searchTextType.Add("搜译文");

            SearchTypeComboBox.ItemsSource = searchTextType;
            SearchTypeComboBox.SelectedIndex = 1;
        }

        private void ExportID_Click(object sender, RoutedEventArgs e)
        {
            int[] ID = new int[] { 38727365, 198758357, 132143172 };

            var export = new ExportFromDB();
            export.ExportIDArray(ID);
        }

        private void ImportTranslate_Click(object sender, RoutedEventArgs e)
        {
            var importTranslate = new ImportTranslateDB();
            importTranslate.Show();
        }

        private void SearchTextBlock_EnterPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SearchTextBox.IsFocused)
            {
                SearchDB();
            }
        }

        private void LangSearchDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBlock_SelectionInfo.Text = "已选择 " + LangSearch.SelectedItems.Count + " 条文本";

        }

        private void LangSearch_MouseRightUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            var selectedItems = grid.SelectedItems;

            if (SelectedDatas != null)
                SelectedDatas.Clear();

            SelectedDatas = new List<LangSearchModel>();



            if (selectedItems.Count > 1)
            {
                foreach (var selectedItem in selectedItems)
                {
                    if (selectedItem != null)
                        SelectedDatas.Add((LangSearchModel)selectedItem);
                }

                TextEditor textEditor = new TextEditor(SelectedData, SelectedDatas);
                textEditor.Show();
            }

        }

        private void SearchDB()
        {
            MessageBoxResult result = MessageBoxResult.Cancel;


            if (SearchTextBox.Text == "" || SearchTextBox.Text == " ")
            {
                result = MessageBox.Show("留空将执行全局搜索，即搜索数据库内全部内容，确定要执行吗？", "内存爆炸警告",
                    MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
            else
            {
                SearchData = SearchLang(SearchCheck());
            }

            switch (result)
            {
                case MessageBoxResult.OK:
                    SearchData = SearchLang(SearchCheck());
                    foreach (var data in SearchData)
                    {
                        LangSearch.Items.Add(data);
                    }
                    textBlock_Info.Text = "总计搜索到" + LangSearch.Items.Count + "条结果。";
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }

            if (SearchData != null)
            {
                if (LangSearch.Items.Count >= 1)
                {

                    LangSearch.Items.Clear();
                }

                foreach (var data in SearchData)
                {
                    LangSearch.Items.Add(data);
                }
                textBlock_Info.Text = "总计搜索到" + LangSearch.Items.Count + "条结果。";
            }
        }

        private void OpenHelpURLinBrowser(object sender, RoutedEventArgs e)
        {
            GoToSite("https://bevisbear.com/eso-lang-editor-help-doc");
        }

        public static void GoToSite(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        private void ExportToLang_Click(object sender, RoutedEventArgs e)
        {
            ToLang();
        }

        private void ExportToCHT_Click(object sender, RoutedEventArgs e)
        {
            ToCHT();
        }

        private void DatabaseModiy_Click(object sender, RoutedEventArgs e)
        {
            var databaseWindow = new DatabaseModifyWindow();

            databaseWindow.Show();
        }

        private void ToLang()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"EsoExtractData\EsoExtractData.exe";

            MessageBoxResult result = MessageBox.Show("将导出的Text文件直接转换为.lang文件，是否导出简体？"
                + Environment.NewLine
                + "点击“是”导出简体，点击“否”导出繁体（需要先转换至繁体中文）"
                + Environment.NewLine
                + "什么都不做请点取消。"
                + Environment.NewLine
                + "点击之后请耐心等待!", "提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    startInfo.Arguments = @" -x _tmp\Text.txt -i _tmp\ID.txt -t -o zh.lang";
                    break;
                case MessageBoxResult.No:
                    startInfo.Arguments = @" -x _tmp\Text_cht.txt -i _tmp\ID.txt -t -o zh.lang";
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }

            if (result != MessageBoxResult.Cancel)
            {
                Process proc = new Process();
                proc.StartInfo = startInfo;
                proc.Start();
                proc.WaitForExit();

                MessageBox.Show("完成！");

                //System.IO.Directory.Delete("_tmp", true);
            }
        }

        private void ToCHT()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"opencc\opencc.exe";
            startInfo.Arguments = @" -i _tmp\Text.txt -o _tmp\Text_cht.txt -c opencc\s2twp.json";

            Process proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();
            proc.WaitForExit();

            MessageBox.Show("完成！");
        }

        private void SearchAbandon_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            Searchabandon = true;
        }

        private void SearchAbandon_checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Searchabandon = false;
        }

        private void UpdatedDB_Check()
        {
            string csvDataPath = @"Data\CsvData.db";
            string csvDataUpdatePath = @"Data\CsvData.update";

            if (File.Exists(csvDataPath) && File.Exists(csvDataUpdatePath))
            {
                var DBFile = new SQLiteController();
                SearchData = DBFile.SearchData("1", 3, false);

                if (SearchData.Count >= 1)
                {
                    foreach (var data in SearchData)
                    {
                        if (data.isTranslated == 1 && data.RowStats == 20)
                        {
                            data.isTranslated = 3;
                        }
                        else
                        {
                            data.isTranslated = 2;
                        }
                    }

                    var exportTranslate = new ExportFromDB();
                    string exportPath = exportTranslate.ExportTranslateDB(SearchData);

                    if (File.Exists(exportPath))
                    {
                        MessageBox.Show("新版本已有更新的数据库，但你本地已查询到翻译过但未导出的文本，现已将翻译过的文本导出。"
                            + Environment.NewLine
                            + "请将 " + exportPath + " 发送给校对或导入人员，你自己也请使用导入翻译功能导入到更新的数据库！", 
                            "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        File.Delete(csvDataPath);
                        File.Move(csvDataUpdatePath, csvDataPath);
                        File.Delete(csvDataUpdatePath);

                        SearchTextBox.IsEnabled = true;
                        SearchButton.IsEnabled = true;

                    }
                    else
                    {
                        MessageBox.Show("新版本已有更新的数据库，但你本地已查询到翻译过但未导出的文本，但导出失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    File.Delete(csvDataPath);
                    File.Move(csvDataUpdatePath, csvDataPath);
                    File.Delete(csvDataUpdatePath);

                    SearchTextBox.IsEnabled = true;
                    SearchButton.IsEnabled = true;
                }
                
            }
            else if (File.Exists(csvDataPath))
            {
                SearchTextBox.IsEnabled = true;
                SearchButton.IsEnabled = true;
            }
            else if (File.Exists(csvDataUpdatePath))
            {
                File.Move(csvDataUpdatePath, csvDataPath);
                File.Delete(csvDataUpdatePath);

                SearchTextBox.IsEnabled = true;
                SearchButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("无法找到数据库文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void str_Click(object sender, RoutedEventArgs e)
        {
            var str = new UIstrFile();
            //str.createDB();
        }

        private void CreateStrDB_Click(object sender, RoutedEventArgs e)
        {
            var createStrDB = new CreateDB_ImportLua();
            createStrDB.Show();
        }

        private void LuaCompareWithDB_Click(object sender, RoutedEventArgs e)
        {
            var compareluaWindow = new CompareLuaWithDBWindow();
            compareluaWindow.Show();
        }

        /*
        private void SetTranslate_Click(object sender, RoutedEventArgs e)
        {
            var DBFile = new SQLiteController();
            List<LangSearchModel> SearchData33;

            var updateData = new List<LangSearchModel>();

            SearchData33 = DBFile.SearchData("1", 3);

            foreach(var data in SearchData33)
            {
                updateData.Add(new LangSearchModel
                {
                    ID_Index = data.ID_Index,
                    ID_Table = data.ID_Table,
                    ID_Type = data.ID_Type,
                    ID_Unknown = data.ID_Unknown,
                    isTranslated = 2
                });
            }

            DBFile.UpdateTextScFromImportDB(updateData);

            MessageBox.Show("完成！");

        }

        private void fildAdd_Click(object sender, RoutedEventArgs e)
        {
            var DBFile = new SQLiteController();

            DBFile.FieldAdd("RowStats", "int", 0);
        }

        
        */
    }
}
