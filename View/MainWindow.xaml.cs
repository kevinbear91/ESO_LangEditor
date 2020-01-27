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
using Microsoft.Win32;
using ESO_Lang_Editor.Model;

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

        public MainWindow()
        {
            //windowsOptions = new MainWindowOption();
            //DataContext = windowsOptions;
            InitializeComponent();
            SearchTextInPositionInit();
            SearchTextTypeInit();

            string version = " Alpha 0.1 - e4ed87fd";

            Title = "ESO文本查询编辑器" + version; 

            textBlock_Info.Text = "";
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (LangSearch.Items.Count > 1)
                SearchData = null;
                LangSearch.Items.Clear();

            SearchData = SearchLang(SearchCheck());

            foreach (var data in SearchData)
            {
                LangSearch.Items.Add(data);
            }
            textBlock_Info.Text = "总计搜索到" + LangSearch.Items.Count + "条结果。";
        }

        public List<LangSearchModel> SearchLang(string SearchBarText)
        {
            var DBFile = new SQLiteController();

            var da1 = DBFile.SearchData(SearchBarText);

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
            var selectedSearchType = SearchTypeComboBox.SelectedIndex;
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

        private void CreateDB_Click(object sender, RoutedEventArgs e)
        {

            var createDBWindow = new CreateDB_ImportCSV();

            createDBWindow.Show();

            /*
            OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Filter = "csv (*.csv)|.csv";
            if (dialog.ShowDialog(this) == true)
            {
                if (dialog.FileName.EndsWith(".csv"))
                {
                    

                }
                else
                {
                    MessageBox.Show("仅支持读取 .csv 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    //textBoxName.Text = "";
                }
            }
            */


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

        private void ExportToText_Click(object sender, RoutedEventArgs e)
        {
            var export = new ExportFromDB();
            MessageBoxResult result = MessageBox.Show("输出数据库的文本内容至Text,文件名分别为ID.txt 与Text. txt"
                + Environment.NewLine
                + "其中ID文件为合并ID, Text为内容。"
                + "点击确定开始输出，不导出请点取消。"
                + Environment.NewLine
                + "点击确定之后请耐心等待，输出完毕后会弹出提示!","提示",MessageBoxButton.OKCancel, MessageBoxImage.Information);
            switch(result)
            {
                case MessageBoxResult.OK:
                    export.ExportAsText();
                    MessageBox.Show("导出完成!" ,"完成",MessageBoxButton.OK,MessageBoxImage.Information);
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
                if(LangSearch.Items.Count > 1)
                SearchData = null;
                LangSearch.Items.Clear();

                SearchData = SearchLang(SearchCheck());

                foreach (var data in SearchData)
                {
                    LangSearch.Items.Add(data);
                }
                textBlock_Info.Text = "总计搜索到" + LangSearch.Items.Count + "条结果。";
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
    }
}
