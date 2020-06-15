using ESO_LangEditorLib;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Data;
using System.Threading.Tasks;
using System.Collections.Immutable;
using ESO_LangEditorLib.Models;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //private MainWindowOption windowsOptions;
        private List<LangData> SearchData;
        List<LangData> SelectedDatas;
        LangData SelectedData;

        //UIstrFile SelectedStrData;
        //List<UIstrFile> searchStrData;
        //List<UIstrFile> SelectedStrDatas;
        private bool isStr;

        ObservableCollection<string> searchTextInPosition;
        ObservableCollection<string> searchTextType;

        

        public MainWindow()
        {
            //windowsOptions = new MainWindowOption();
            //DataContext = windowsOptions;
            InitializeComponent();
            SearchTextBox.IsEnabled = false;
            SearchButton.IsEnabled = false;


            SearchTextInPositionInit();
            SearchTextTypeInit();
            

            string version = " v2.x";

            Title = "ESO文本查询编辑器" + version;

            textBlock_Info.Text = "暂无查询";
            textBlock_SelectionInfo.Text = "无选择条目";


            //LangSearch.AutoGeneratingColumn += LangDataGridAutoGenerateColumns;

            GeneratingColumns(false);

            //var db = new SqliteController();
            //db.CreateTable();

            //using (var db = new Lang_DbContext())
            //{
            //    db.Database.EnsureCreated();
            //}




            //UpdatedDB_Check();
            SearchTextBox.IsEnabled = true;
            SearchButton.IsEnabled = true;
        }


        private void GeneratingColumns(bool isStr)
        {
            if (isStr)
            {
                DataGridTextColumn c1 = new DataGridTextColumn();
                c1.Header = "UI ID";
                c1.Binding = new Binding("ID");
                c1.Width = 200;
                LangData.Columns.Add(c1);

                DataGridTextColumn c2 = new DataGridTextColumn();
                c2.Header = "英文";
                c2.Width = 200;
                c2.Binding = new Binding("Text_EN");
                LangData.Columns.Add(c2);

                DataGridTextColumn c3 = new DataGridTextColumn();
                c3.Header = "汉化";
                c3.Width = 200;
                c3.Binding = new Binding("Text_ZH");
                LangData.Columns.Add(c3);
            }
            else
            {
                DataGridTextColumn c1 = new DataGridTextColumn();
                c1.Header = "文本ID";
                c1.Binding = new Binding("UniqueID");
                //c1.Width = 110;
                LangData.Columns.Add(c1);

                DataGridTextColumn c2 = new DataGridTextColumn();
                c2.Header = "英文";
                //c2.Width = 200;
                c2.Binding = new Binding("Text_EN");
                LangData.Columns.Add(c2);

                DataGridTextColumn c3 = new DataGridTextColumn();
                c3.Header = "汉化";
                //c3.Width = 200;
                c3.Binding = new Binding("Text_ZH");
                LangData.Columns.Add(c3);
            }

        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            

            if (SearchStr_checkBox.IsChecked == true)
            {
                //SearchStrDB();
            }
            else
            {
                await SearchDB();
            }

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
                        if (isStr)
                        {
                            //SelectedStrData = (UIstrFile)datagrid.SelectedItem;
                            //TextEditor textEditor = new TextEditor(SelectedStrData, SelectedStrDatas);
                            //textEditor.Show();
                            //MessageBox.Show(data.Text_SC);
                        }
                        else
                        {
                            //SelectedData = ;
                            TextEditor textEditor = new TextEditor((LangData)datagrid.SelectedItem);
                            textEditor.Show();
                            //MessageBox.Show((LangData)datagrid.SelectedItem);
                        }


                    }

                target = VisualTreeHelper.GetParent(target);
            }
        }

        private void CreateDB_Click(object sender, RoutedEventArgs e)
        {
            var createDBWindow = new CreateDB_ImportCSV();

            createDBWindow.Show();

        }

        private void CsvFileCompare_Click(object sender, RoutedEventArgs e)
        {
            //var compareCsvWindows = new CompareCsvWindow();
            //compareCsvWindows.Show();
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
            //var export = new ExportFromDB();
            //MessageBoxResult result = MessageBox.Show("输出数据库的文本内容至Text,文件名分别为ID.txt 与Text. txt"
            //    + Environment.NewLine
            //    + "其中ID文件为合并ID, Text为内容。"
            //    + "点击确定开始输出，不导出请点取消。"
            //    + Environment.NewLine
            //    + "点击确定之后请耐心等待，输出完毕后会弹出提示!", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            //switch (result)
            //{
            //    case MessageBoxResult.OK:
            //        export.ExportAsText();
            //        MessageBox.Show("导出完成!", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
            //        break;
            //    case MessageBoxResult.Cancel:
            //        break;
            //}
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
            //int[] ID = new int[] { 38727365, 198758357, 132143172 };

            ////var export = new ExportFromDB();
            //export.ExportIDArray(ID);
        }

        private void ImportTranslate_Click(object sender, RoutedEventArgs e)
        {
            var importTranslate = new ImportTranslateDB();
            importTranslate.Show();
        }

        private async void SearchTextBlock_EnterPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SearchTextBox.IsFocused)
            {
                if (SearchStr_checkBox.IsChecked == true)
                {
                    //SearchStrDB();
                }
                else
                {
                    await SearchDB();
                }
            }
        }

        private void LangSearchDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBlock_SelectionInfo.Text = "已选择 " + LangData.SelectedItems.Count + " 条文本";

        }

        private void LangSearch_MouseRightUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            var selectedItems = grid.SelectedItems;

            if (isStr)
            {
                //if (SelectedStrDatas != null)
                //    SelectedStrDatas.Clear();

                //SelectedStrDatas = new List<UIstrFile>();

                //if (selectedItems.Count > 1)
                //{
                //    foreach (var selectedItem in selectedItems)
                //    {
                //        if (selectedItem != null)
                //            SelectedStrDatas.Add((UIstrFile)selectedItem);
                //    }

                //    TextEditor textEditor = new TextEditor(SelectedStrData, SelectedStrDatas);
                //    textEditor.Show();
                //}
            }
            else
            {
                if (SelectedDatas != null)
                    SelectedDatas.Clear();

                SelectedDatas = new List<LangData>();

                if (selectedItems.Count > 1)
                {
                    foreach (var selectedItem in selectedItems)
                    {
                        if (selectedItem != null)
                            SelectedDatas.Add((LangData)selectedItem);
                    }

                    TextEditor textEditor = new TextEditor(SelectedDatas);
                    textEditor.Show();
                }
            }

        }

        private async Task SearchDB()
        {
            int selectedSearchType = SearchTypeComboBox.SelectedIndex;
            int selectedSearchTextPosition = SearchTextPositionComboBox.SelectedIndex;
            string searchText = SearchTextBox.Text;

            SearchButton.IsEnabled = false;
            SearchButton.Content = "正在搜索……";
            SearchTextBox.IsEnabled = false;

            var db = new LangDbController();

            MessageBoxResult result = MessageBoxResult.Cancel;

            if (SearchTextBox.Text == "" || SearchTextBox.Text == " ")
            {
                result = MessageBox.Show("留空将执行全局搜索，即搜索数据库内全部内容，确定要执行吗？", "内存爆炸警告",
                    MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
            else
            {
                SearchData = await Task.Run(() =>
                {
                    var query = db.GetLangsListAsync(selectedSearchType, selectedSearchTextPosition, searchText);
                    return query;
                });

            }

            switch (result)
            {
                case MessageBoxResult.OK:
                    SearchData = await Task.Run(() =>
                    {
                        var query = db.GetLangsListAsync(selectedSearchType, selectedSearchTextPosition, searchText);
                        return query;
                    });
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }

            LangData.ItemsSource = SearchData;
            SearchButton.IsEnabled = true;
            SearchButton.Content = "搜索";
            SearchTextBox.IsEnabled = true;

            textBlock_Info.Text = GetInfoBlockText();
        }

        private string GetInfoBlockText()
        {
            return "总计搜索到" + LangData.Items.Count + "条结果。";
        }

        //private void SearchStrDB()
        //{
        //    MessageBoxResult result = MessageBoxResult.Cancel;


        //    if (SearchTextBox.Text == "" || SearchTextBox.Text == " ")
        //    {
        //        result = MessageBox.Show("留空将执行全局搜索，即搜索数据库内全部内容，确定要执行吗？", "内存爆炸警告",
        //            MessageBoxButton.OKCancel, MessageBoxImage.Warning);
        //    }
        //    else
        //    {
        //        searchStrData = SearchStr(SearchCheck());
        //    }

        //    switch (result)
        //    {
        //        case MessageBoxResult.OK:
        //            searchStrData = SearchStr(SearchCheck());
        //            foreach (var data in searchStrData)
        //            {
        //                LangSearch.Items.Add(data);
        //            }
        //            textBlock_Info.Text = "总计搜索到" + LangSearch.Items.Count + "条结果。";
        //            break;
        //        case MessageBoxResult.Cancel:
        //            break;
        //    }

        //    if (searchStrData != null)
        //    {
        //        if (LangSearch.Items.Count >= 1)
        //        {

        //            LangSearch.Items.Clear();
        //        }

        //        foreach (var data in searchStrData)
        //        {
        //            LangSearch.Items.Add(data);
        //        }
        //        textBlock_Info.Text = "总计搜索到" + LangSearch.Items.Count + "条结果。";
        //    }
        //}

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
            //var databaseWindow = new DatabaseModifyWindow();

            //databaseWindow.Show();
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


        private void UpdatedDB_Check()
        {
            //string csvDataPath = @"Data\CsvData.db";
            //string csvDataUpdatePath = @"Data\CsvData.update";

            //string strDataPath = @"Data\UI_Str.db";
            //string strDataUpdatePath = @"Data\UI_Str.update";


            //#region  检查CSV数据库更新
            //if (File.Exists(csvDataPath) && File.Exists(csvDataUpdatePath))
            //{
            //    var DBFile = new SQLiteController();
            //    SearchData = DBFile.SearchData("1", 3, false);

            //    if (SearchData.Count >= 1)
            //    {
            //        foreach (var data in SearchData)
            //        {
            //            if (data.isTranslated == 1 && data.RowStats == 20)
            //            {
            //                data.isTranslated = 3;
            //            }
            //            else
            //            {
            //                data.isTranslated = 2;
            //            }
            //        }

            //        var exportTranslate = new ExportFromDB();
            //        string exportPath = exportTranslate.ExportTranslateDB(SearchData);

            //        if (File.Exists(exportPath))
            //        {
            //            MessageBox.Show("新版本已有更新的数据库，但你本地已查询到翻译过但未导出的文本，现已将翻译过的文本导出。"
            //                + Environment.NewLine
            //                + "请将 " + exportPath + " 发送给校对或导入人员，你自己也请使用导入翻译功能导入到更新的数据库！", 
            //                "提示", MessageBoxButton.OK, MessageBoxImage.Information);

            //            GC.Collect();
            //            GC.WaitForPendingFinalizers();
            //            File.Delete(csvDataPath);
            //            File.Move(csvDataUpdatePath, csvDataPath);
            //            File.Delete(csvDataUpdatePath);

            //            SearchTextBox.IsEnabled = true;
            //            SearchButton.IsEnabled = true;

            //        }
            //        else
            //        {
            //            MessageBox.Show("新版本已有更新的数据库，但你本地已查询到翻译过但未导出的文本，但导出失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            //        }
            //    }
            //    else
            //    {
            //        GC.Collect();
            //        GC.WaitForPendingFinalizers();
            //        File.Delete(csvDataPath);
            //        File.Move(csvDataUpdatePath, csvDataPath);
            //        File.Delete(csvDataUpdatePath);

            //        SearchTextBox.IsEnabled = true;
            //        SearchButton.IsEnabled = true;
            //    }
                
            //}
            //else if (File.Exists(csvDataPath))
            //{
            //    SearchTextBox.IsEnabled = true;
            //    SearchButton.IsEnabled = true;
            //}
            //else if (File.Exists(csvDataUpdatePath))
            //{
            //    File.Move(csvDataUpdatePath, csvDataPath);
            //    File.Delete(csvDataUpdatePath);

            //    SearchTextBox.IsEnabled = true;
            //    SearchButton.IsEnabled = true;
            //}
            //else
            //{
            //    MessageBox.Show("无法找到数据库文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //#endregion


            //#region 检查 UI STR数据库更新
            //if (File.Exists(strDataPath) && File.Exists(strDataUpdatePath))
            //{
            //    var strDB = new UI_StrController();
            //    searchStrData = strDB.SearchData("1", 3, false);

            //    if (searchStrData.Count >= 1)
            //    {
            //        foreach (var data in searchStrData)
            //        {
            //            if (data.isTranslated == 1 && data.RowStats == 20)
            //            {
            //                data.isTranslated = 3;
            //            }
            //            else
            //            {
            //                data.isTranslated = 2;
            //            }
            //        }

            //        var exportTranslate = new ExportFromDB();
            //        string exportPath = exportTranslate.ExportTranslateDB(searchStrData);

            //        if (File.Exists(exportPath))
            //        {
            //            MessageBox.Show("新版本已有更新的UI数据库，但你本地已查询到翻译过但未导出的文本，现已将翻译过的文本导出。"
            //                + Environment.NewLine
            //                + "请将 " + exportPath + " 发送给校对或导入人员，你自己也请使用导入翻译功能导入到更新的数据库！",
            //                "提示", MessageBoxButton.OK, MessageBoxImage.Information);

            //            GC.Collect();
            //            GC.WaitForPendingFinalizers();
            //            File.Delete(strDataPath);
            //            File.Move(strDataUpdatePath, strDataPath);
            //            File.Delete(strDataUpdatePath);

            //            SearchTextBox.IsEnabled = true;
            //            SearchButton.IsEnabled = true;

            //        }
            //        else
            //        {
            //            MessageBox.Show("新版本已有更新的UI数据库，但你本地已查询到翻译过但未导出的文本，但导出失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            //        }
            //    }
            //    else
            //    {
            //        GC.Collect();
            //        GC.WaitForPendingFinalizers();
            //        File.Delete(strDataPath);
            //        File.Move(strDataUpdatePath, strDataPath);
            //        File.Delete(strDataUpdatePath);

            //        SearchTextBox.IsEnabled = true;
            //        SearchButton.IsEnabled = true;
            //    }

            //}
            //else if (File.Exists(strDataPath))
            //{
            //    SearchTextBox.IsEnabled = true;
            //    SearchButton.IsEnabled = true;
            //}
            //else if (File.Exists(strDataUpdatePath))
            //{
            //    File.Move(strDataUpdatePath, strDataPath);
            //    File.Delete(strDataUpdatePath);

            //    SearchTextBox.IsEnabled = true;
            //    SearchButton.IsEnabled = true;
            //}
            //else
            //{
            //    MessageBox.Show("无法找到UI数据库文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //#endregion

        }

        //private void str_Click(object sender, RoutedEventArgs e)
        //{
        //    var str = new UIstrFile();
        //    //str.createDB();
        //}

        private void CreateStrDB_Click(object sender, RoutedEventArgs e)
        {
        //    var createStrDB = new CreateDB_ImportLua();
        //    createStrDB.Show();
        }

        private void LuaCompareWithDB_Click(object sender, RoutedEventArgs e)
        {
        //    var compareluaWindow = new CompareLuaWithDBWindow();
        //    compareluaWindow.Show();
        }

        private void SearchStr_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            LangData.Columns.Clear();
            LangData.Items.Clear();

            isStr = true;

            GeneratingColumns(isStr);

        }

        private void SearchStr_checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            LangData.Columns.Clear();
            LangData.Items.Clear();

            isStr = false;

            GeneratingColumns(isStr);
        }


        private void ExportToStr_Click(object sender, RoutedEventArgs e)
        {
        //    MessageBoxResult result = MessageBox.Show("导出UI STR内容，请选择导出数据库"
        //        + Environment.NewLine
        //        + "点击“是”导出Pregame，点击“否”导出Client。"
        //        + Environment.NewLine
        //        + "什么都不做请点取消。"
        //        + Environment.NewLine
        //        + "点击之后请耐心等待!", "提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);

        //    var strExport = new ExportFromDB();

        //    switch (result)
        //    {
        //        case MessageBoxResult.Yes:
        //            strExport.ExportStrDB("Pregame");
        //            break;
        //        case MessageBoxResult.No:
        //            strExport.ExportStrDB("Client");
        //            break;
        //        case MessageBoxResult.Cancel:
        //            break;
        //    }
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
