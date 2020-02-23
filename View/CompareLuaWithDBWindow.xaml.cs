using ESO_Lang_Editor.Model;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using static System.Convert;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// CompareWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CompareLuaWithDBWindow : Window
    {
        UIstrFile LuaParser = new UIstrFile();
        private Dictionary<string, string> NewDict;
        private Dictionary<string, string> langData;
        public ObservableCollection<string> compareOptions { get; set; }
        public ObservableCollection<string> tableNameOptions { get; set; }

        public CompareLuaWithDBWindow()
        {
            InitializeComponent();
            CompareOptionsInit();
            TableNameOptionsInit();




            CheckSaveToDBButtonCanEnable();

        }

        private void BrowseNewFileButton_Click(object sender, RoutedEventArgs e)
        {
            SeletedCsvFileCheck(NewFileURLtextBox);
        }

        private void SeletedCsvFileCheck(TextBox textBoxName)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Filter = "csv (*.csv)|.csv";
            if (dialog.ShowDialog(this) == true)
            {
                if (dialog.FileName.EndsWith(".lua"))
                {
                    textBoxName.Text = dialog.FileName;
                }
                else
                {
                    MessageBox.Show("仅支持读取 .Lua 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    textBoxName.Text = "";
                }
            }
        }


        private void LoadLua_Buton_Click(object sender, RoutedEventArgs e)
        {
            string fileName;
            //List<LangSearchModel> langData;

            if (NewFileURLtextBox.Text != "")
            {
                fileName = NewFileURLtextBox.Text;  //&& fileName != NewFileURLtextBox.Text
                if (NewDict == null)
                {
                    NewDict = StrToStringDict(LuaParser.ParserLua(NewFileURLtextBox.Text));

                    CompareData();
                    CheckSaveToDBButtonCanEnable();
                }
                else
                {
                    CompareData();
                    CheckSaveToDBButtonCanEnable();
                }
            }
            else
            {
                MessageBox.Show("未选择Lua文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CompareData()
        {
            int CompareOptionsIndex = CompareOptions_comboBox.SelectedIndex;
            //Dictionary<string, string> addedParser;


            if (Changed_DataGrid.Items.Count > 1)
                langData = null;
            Changed_DataGrid.Items.Clear();

            switch (CompareOptionsIndex)
            {
                case 0:
                    langData = LuaParser.LuaCompareAdded(NewDict);
                    break;
                case 1:
                    langData = LuaParser.LuaCompareEdited(NewDict);
                    break;
                case 2:
                    langData = LuaParser.LuaCompareRemove(NewDict);
                    break;
                default:
                    langData = LuaParser.LuaCompareAdded(NewDict);
                    break;
            }


            //langData = LuaParser.CsvDictToModel(addedParser);

            foreach (var data in langData)
            {
                Changed_DataGrid.Items.Add(data);
            }
            Status_textBlock.Text = "总计搜索到" + Changed_DataGrid.Items.Count + "条结果。";
        }

        public Dictionary<string, string> LoadDB()
        {
            var db = new SQLiteController();
            Dictionary<string, string> csvFileModel = new Dictionary<string, string>();

            var searchData = db.FullSearchData(false);

            foreach (var data in searchData)
            {
                csvFileModel.Add(data.ID_Type + "-" + data.ID_Unknown.ToString() + "-" + data.ID_Index.ToString(), data.Text_EN);
            }

            return csvFileModel;
        }



        private void CompareAdded_Button_Click(object sender, RoutedEventArgs e)
        {
            var langData = LuaParser.LuaCompareAdded(NewDict);
            //var langData = LuaParser.CsvDictToModel(addedParser);

            if (Changed_DataGrid.Items.Count > 1)
                langData = null;
            Changed_DataGrid.Items.Clear();

            foreach (var data in langData)
            {
                Changed_DataGrid.Items.Add(data);
            }
            //NewStatus_textBlock.Text = "总计搜索到" + Changed_DataGrid.Items.Count + "条结果。";
        }

        private void CompareOptionsInit()
        {
            compareOptions = new ObservableCollection<string>();

            compareOptions.Add("比较新增");
            compareOptions.Add("比较修改");
            compareOptions.Add("比较删除");

            CompareOptions_comboBox.ItemsSource = compareOptions;
            CompareOptions_comboBox.SelectedIndex = 0;
        }

        private void TableNameOptionsInit()
        {
            tableNameOptions = new ObservableCollection<string>();

            tableNameOptions.Add("PreGame");
            tableNameOptions.Add("Client");

            TableName_comboBox.ItemsSource = tableNameOptions;
            TableName_comboBox.SelectedIndex = 0;
        }

        private Dictionary<string, string> StrToStringDict(Dictionary<string, UIstrFile> strDict)
        {
            var _Dict = new Dictionary<string, string>();
            foreach(var line in strDict)
            {
                _Dict.Add(line.Key, line.Value.UI_EN);
            }

            return _Dict;
        }


        private void SaveToDB_Button_Click(object sender, RoutedEventArgs e)
        {
            var dbFileModel = new List<UIstrFile>();
            int CompareOptionsIndex = CompareOptions_comboBox.SelectedIndex;
            int rowStats = CompareOptions_comboBox.SelectedIndex;
            string updateStats = VersionInput_textBox.Text;
            string tableName = TableName_comboBox.SelectedItem.ToString();

            if (updateStats == "" || updateStats == "更新版本号")
                MessageBox.Show("请输入新版本文本的版本号！比如“Update25”等！", "提醒",
                        MessageBoxButton.OK, MessageBoxImage.Warning);


            if (CompareOptionsIndex == 1)
            {
                dbFileModel = LuaParser.FullSearchStrDB(false);

                foreach (var d in dbFileModel)
                {
                    d.UpdateStats = updateStats;
                }

            }
            else
            {
                foreach (var line in langData)
                {
                    dbFileModel.Add(new UIstrFile
                    {
                        UI_ID = line.Key,
                        UI_EN = line.Value,
                        UI_ZH = line.Value,
                        isTranslated = 0,
                        //RowStats = rowStats,         //插入数据库内直接定义
                        UpdateStats = updateStats,
                    });
                }
            }


            switch (CompareOptionsIndex)
            {
                case 0:
                    LuaParser.AddDataList(dbFileModel, tableName);
                    MessageBox.Show("加入完成！ 共" + dbFileModel.Count + " 条数据。", "结果",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case 1:
                    LuaParser.MarkChangedDataList(dbFileModel, tableName);
                    MessageBox.Show("标记修改完成！ 共" + dbFileModel.Count + " 条数据。", "结果",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case 2:
                    LuaParser.MarkDeleteDataList(dbFileModel, tableName);
                    MessageBox.Show("标记删除完成！ 共" + dbFileModel.Count + " 条数据。", "结果",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                default:
                    //connDB.AddDataArray(dbFileModel);
                    break;
            }

        }

        private void VersionInput_GetFocus(object sender, RoutedEventArgs e)
        {
            if (VersionInput_textBox.Text == "更新版本号(必填)")
                VersionInput_textBox.Text = "";
        }

        private void VersionInput_Lostfocus(object sender, RoutedEventArgs e)
        {
            if (VersionInput_textBox.Text == "")
                VersionInput_textBox.Text = "更新版本号(必填)";
            CheckSaveToDBButtonCanEnable();

        }

        private bool CheckSaveToDBButtonCanEnable()
        {
            if (NewFileURLtextBox.Text != "" 
                && VersionInput_textBox.Text != ""
                && VersionInput_textBox.Text != " "
                && VersionInput_textBox.Text != "更新版本号(必填)"
                && langData != null)
            {
                SaveToDB_Button.IsEnabled = true;
                return true;
            }
            else
            {
                SaveToDB_Button.IsEnabled = false;
                return false;
            }


        }
    }
}
