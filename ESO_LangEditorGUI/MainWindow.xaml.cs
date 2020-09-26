﻿using ESO_LangEditorGUI.Model;
using ESO_LangEditorGUI.Controller;
using ESO_LangEditorGUI.View;
using ESO_LangEditorLib;
using ESO_LangEditorLib.Models;
using ESO_LangEditorLib.Models.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorGUI.Interface;
using ESO_LangEditorGUI.Models.Enum;

namespace ESO_LangEditorGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private MainWindowOption windowsOptions;
        private List<LangTextDto> SearchData;
        private List<LangTextDto> SelectedDatas;
        //LangText SelectedData;

        //UIstrFile SelectedStrData;
        private List<LuaUIData> searchLuaData;
        private List<LuaUIData> SelectedLuaDatas;
        private bool isLua;

        ObservableCollection<string> searchTextInPosition;
        ObservableCollection<string> searchTextType;

        LangDbController db = new LangDbController();

        IDCatalog IDtypeName = new IDCatalog();
        WindowController windowControll = new WindowController();

        public MainWindow()
        {
            //windowsOptions = new MainWindowOption();
            //DataContext = windowsOptions;
            
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();


            SearchTextBox.IsEnabled = false;
            SearchButton.IsEnabled = false;


            //SearchTextInPositionInit();
            //SearchTextTypeInit();


            //string version = " v2.4.0";

            //Title = "ESO文本查询编辑器" + version;

            //textBlock_Info.Text = "暂无查询";
            //textBlock_SelectionInfo.Text = "无选择条目";


            //LangSearch.AutoGeneratingColumn += LangDataGridAutoGenerateColumns;

            //GeneratingColumns();
            GeneratingChips();

            //var db = new SqliteController();
            //db.CreateTable();

            //using (var db = new LangDbContext())
            //{
            //    db.Database.EnsureCreated();
            //}

            new CheckDBFile().CheckDBUpdateFile(this);

            //UpdatedDB_Check();

        }


        

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //await SearchDB();
            //LangData.ItemsSource = await windowControll.GetLangTexts();

            //await LangDataGrid.SetDataAsync();
            //MessageBox.Show(SearchTextPositionComboBox.SelectedItem.ToString());

            //SearchLangText search = new SearchLangText();

            //List<LangTextDto> langtexts = await Task.Run(() => net.GetLangTexts());

            //await search.GetLangText(SearchPostion.Full, SearchTextType.ByUser, "148ed451-bf19-43e9-a8d3-55f922cd349e");

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
                        if (isLua)
                        {
                            TextEditor textEditor = new TextEditor((LuaUIData)datagrid.SelectedItem, ref IDtypeName, this);
                            textEditor.Show();
                        }
                        else
                        {
                            //SelectedData = ;
                            TextEditor textEditor = new TextEditor((LangTextDto)datagrid.SelectedItem, ref IDtypeName, this);
                            textEditor.Show();
                            //MessageBox.Show((LangData)datagrid.SelectedItem);
                        }


                    }

                target = VisualTreeHelper.GetParent(target);
            }
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


        //private void SearchTextInPositionInit()
        //{
        //    //searchTextInPosition = new ObservableCollection<string>
        //    //{
        //    //    "包含全文",
        //    //    "仅包含开头",
        //    //    "仅包含结尾"
        //    //};

        //    SearchTextPositionComboBox.ItemsSource = windowControll.GetSearchPostion();
        //    SearchTextPositionComboBox.SelectedIndex = 0;
        //}

        //private void SearchTextTypeInit()
        //{
        //    //searchTextType = new ObservableCollection<string>
        //    //{
        //    //    "搜类型",   //0
        //    //    "搜英文",   //1
        //    //    "搜译文",   //2
        //    //    "搜版本号",
        //    //    "搜唯一ID",
        //    //    "搜已翻译条目"
        //    //};

        //    SearchTypeComboBox.ItemsSource = windowControll.GetSearchTextType();
        //    SearchTypeComboBox.SelectedIndex = 1;
        //}

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
                await SearchDB();
            }
        }

        private void LangSearchDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // textBlock_SelectionInfo.Text = "已选择 " + LangData.SelectedItems.Count + " 条文本";

        }

        private void LangSearch_MouseRightUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            var selectedItems = grid.SelectedItems;

            if (isLua)
            {
                if (SelectedLuaDatas != null)
                    SelectedLuaDatas.Clear();

                SelectedLuaDatas = new List<LuaUIData>();

                if (selectedItems.Count > 1)
                {
                    foreach (var selectedItem in selectedItems)
                    {
                        if (selectedItem != null)
                            SelectedLuaDatas.Add((LuaUIData)selectedItem);
                    }

                    TextEditor textEditor = new TextEditor(SelectedLuaDatas, ref IDtypeName, this);
                    textEditor.Show();
                }
            }
            else
            {
                if (SelectedDatas != null)
                    SelectedDatas.Clear();

                SelectedDatas = new List<LangTextDto>();

                if (selectedItems.Count > 1)
                {
                    foreach (var selectedItem in selectedItems)
                    {
                        if (selectedItem != null)
                            SelectedDatas.Add((LangTextDto)selectedItem);
                    }

                    TextEditor textEditor = new TextEditor(SelectedDatas, ref IDtypeName, this);
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
            //SearchButton.Content = "正在搜索……";
            SearchTextBox.IsEnabled = false;

            //if (SearchTypeComboBox.SelectedIndex == 0)
            //{
            //    if (System.Text.RegularExpressions.Regex.IsMatch(SearchTextBox.Text, "[^0-9]"))
            //    {
            //        MessageBox.Show("Please enter only numbers.");
            //    }
            //}



            MessageBoxResult result = MessageBoxResult.Cancel;

            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                result = MessageBox.Show("留空将执行全局搜索，即搜索数据库内全部内容，确定要执行吗？", "内存爆炸警告",
                    MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
            else
            {
                await GetLangData(selectedSearchType, selectedSearchTextPosition, searchText);
            }

            switch (result)
            {
                case MessageBoxResult.OK:
                    await GetLangData(selectedSearchType, selectedSearchTextPosition, searchText);
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }

            //LangData.ItemsSource = SearchData;
            SearchButton.IsEnabled = true;
            //SearchButton.Content = "搜索";
            SearchTextBox.IsEnabled = true;

            //textBlock_Info.Text = GetInfoBlockText();
        }

        private async Task GetLangData(int selectedSearchType, int selectedSearchTextPosition, string searchText)
        {
            int searchType = selectedSearchType;

            if (windowControll.InputCheck(searchType, searchText))
            {
                if (searchType == 5 && isLua == false)
                    searchType = 6;

                if (isLua)
                {
                    searchLuaData = await Task.Run(() =>
                    {
                        var query = db.GetLuaLangsListAsync(searchType, selectedSearchTextPosition, searchText);
                        return query;
                    });
                    //LangData.ItemsSource = searchLuaData;
                }
                else
                {
                    //SearchData = await Task.Run(() =>
                    //{
                    //    var query = db.GetLangsListAsync(searchType, selectedSearchTextPosition, searchText);
                    //    return query;
                    //});
                    //LangData.ItemsSource = SearchData;
                }
            }
            else
            {
                MessageBox.Show("当前搜索条件仅支持数字！", "提示",
                      MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        //private string GetInfoBlockText()
        //{
        //    //return "总计搜索到" + LangData.Items.Count + "条结果。";
        //}

        public async Task SetSaveStats(bool saved)
        {
            if (saved)
            {
                var messageQueue = Snackbar_SaveInfo.MessageQueue;
                var message = "保存成功！";
                messageQueue.Enqueue(message);

            }
            else
            {
                var messageQueue = Snackbar_SaveInfo.MessageQueue;
                var message = "保存失败！";
                messageQueue.Enqueue(message);

            }


        }


        private void OpenHelpURLinBrowser(object sender, RoutedEventArgs e)
        {
            GoToSite("https://bevisbear.com/eso-lang-editor-help-doc");
        }

        public static void GoToSite(string url)
        {
            //System.Diagnostics.Process.Start(url);
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
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
            var export = new ExportFromDB();
            var tolang = new ThirdPartController();

            MessageBoxResult resultExport = MessageBox.Show("一键导出文本到.lang文件，点击确定开始，不导出请点取消。"
                + Environment.NewLine
                + "点击确定之后请耐心等待，输出完毕后会弹出提示!", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            switch (resultExport)
            {
                case MessageBoxResult.OK:
                    export.ExportAsText();
                    tolang.ConvertTxTtoLang(false);
                    MessageBox.Show("导出完成!", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void ToCHT()
        {
            var export = new ExportFromDB();
            var tolang = new ThirdPartController();

            export.ExportAsText();

            tolang.OpenCCtoCHT();
            tolang.ConvertTxTtoLang(true);

            MessageBox.Show("完成！");
        }


        //private void str_Click(object sender, RoutedEventArgs e)
        //{
        //    var str = new UIstrFile();
        //    //str.createDB();
        //}


        private void SearchStr_checkBox_Checked(object sender, RoutedEventArgs e)
        {

            //if (LangData.Items.Count >= 1)
            //{
            //    LangData.ItemsSource = null;

            //    isLua = true;
            //    //GeneratingColumns();
            //}
            //else
            //{
            //    isLua = true;
            //    //GeneratingColumns();
            //}

        }

        private void SearchStr_checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //if (LangData.Items.Count >= 1)
            //{
            //    LangData.ItemsSource = null;

            //    isLua = false;
            //    //GeneratingColumns();
            //}
            //else
            //{
            //    isLua = false;
            //    //GeneratingColumns();
            //}
        }


        private void ExportToStr_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("导出UI STR内容"
                + Environment.NewLine
                + "点击“是”导出，点击“否”什么都不做。"
                + Environment.NewLine
                + "点击之后请耐心等待!", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);

            var strExport = new ExportFromDB();

            switch (result)
            {
                case MessageBoxResult.Yes:
                    strExport.ExportStrDB();
                    break;
                case MessageBoxResult.No:
                    break;

            }
        }

        private void GeneratingChips()
        {
            //Chip c1 = new Chip();
            //c1.Content = "112edd";
            //c1.IsDeletable = true;


            //Chips.Children.Add(c1);
        }

        private void PackToRlease_Click(object sender, RoutedEventArgs e)
        {
            var PackWindow = new PackToRelase();

            PackWindow.Show();
        }
    }
}
