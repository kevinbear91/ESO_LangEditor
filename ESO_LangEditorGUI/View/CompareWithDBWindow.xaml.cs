using ESO_Lang_Editor.Model;
using ESO_LangEditorLib;
using ESO_LangEditorLib.Models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static System.Convert;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// CompareWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CompareWithDBWindow : Window
    {

        private Dictionary<string, LangData> dbLangDict;
        private Dictionary<string, LangData> CsvDict;
        //private List<LangSearchModel> langData;
        private List<LangData> csvData;

        //private Dictionary<string, LangData> added = new Dictionary<string, LangData>();
        //private Dictionary<string, LangData> changed = new Dictionary<string, LangData>();
        //private Dictionary<string, LangData> removed = new Dictionary<string, LangData>();
        //private Dictionary<string, LangData> nonChanged = new Dictionary<string, LangData>();

        private List<LangData> added = new List<LangData>();
        private List<LangData> changed = new List<LangData>();
        private List<LangData> nonChanged = new List<LangData>();
        private List<LangData> removedList = new List<LangData>();
        private Dictionary<string, LangData> removed = new Dictionary<string, LangData>();

        public ObservableCollection<string> compareOptions { get; set; }

        public CompareWithDBWindow()
        {
            InitializeComponent();

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
                if (dialog.FileName.EndsWith(".csv"))
                {
                    textBoxName.Text = dialog.FileName;
                }
                else
                {
                    MessageBox.Show("仅支持读取 .csv 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    textBoxName.Text = "";
                }
            }
        }


        private async void LoadCsv_Buton_Click(object sender, RoutedEventArgs e)
        {
            string filePath;
            ////List<LangSearchModel> langData;
            ///
            CsvParser csvparser = new CsvParser();
            var newList = new List<LangData>();
            //removed = new List<LangData>();



            if (NewFileURLtextBox.Text != "")
            {
                filePath = NewFileURLtextBox.Text;  //&& fileName != NewFileURLtextBox.Text

                CsvDict = await csvparser.CsvReaderToDictionaryAsync(filePath);

                var db = new Lang_DbController();

                //dbLangDict = await db.GetAllLangsDictionaryAsync();

                dbLangDict = await csvparser.CsvReaderToDictionaryAsync(@"D:\eso_zh\eso_zh\en.lang.csv");


                DiffDictionary(dbLangDict, CsvDict);


                //removed = dbLang.Except(csvData).ToList();

                Debug.WriteLine(dbLangDict.Count());



                //foreach(var other in csvData)
                //{
                //    if (dbLang.Except())
                //}


                //db.GetLangsAsync(1, 0, "");



                //if (OldDict == null && NewDict == null)
                //{
                //    OldDict = LoadDB();
                //    NewDict = fileParser.LoadCsvToDict(NewFileURLtextBox.Text);

                //    CompareData();
                //    CheckSaveToDBButtonCanEnable();
                //}
                //else
                //{
                //    CompareData();

                //    CheckSaveToDBButtonCanEnable();
                //}
            }
            else
            {
                MessageBox.Show("未选择CSV文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DiffDictionary(Dictionary<string, LangData> first, Dictionary<string, LangData> second)
        {
            Debug.WriteLine("开始比较。");

            removed = first;

            foreach (var other in second)
            {

                if (first.TryGetValue(other.Key, out LangData firstValue))
                {
                    if (firstValue.Text_EN.Equals(other.Value.Text_EN))
                    {
                        nonChanged.Add(firstValue);
                        removed.Remove(other.Key);
                    }
                    else
                    {
                        changed.Add(other.Value);
                        removed.Remove(other.Key);
                    }
                }
                else
                {
                    added.Add(other.Value);
                    removed.Remove(other.Key);
                }
            }

            removedList = removed.Values.ToList();

            //Debug.WriteLine("移除：" + removed.Count());
            //Debug.WriteLine("无更改：" + nonChanged.Count());
            //Debug.WriteLine("更改：" + changed.Count());
            //Debug.WriteLine("添加：" + added.Count());

            SetCompareUI();

        }

        private void SetCompareUI()
        {
            List<TabItem> tabs = new List<TabItem>();
            tabs.Add(Added_tabitem); 
            tabs.Add(Changed_tabitem);
            tabs.Add(Removed_tabitem);

            GeneratingColumns(added, Added_DataGrid, Added_tabitem);
            GeneratingColumns(changed, Changed_DataGrid, Changed_tabitem);
            GeneratingColumns(removedList, Removed_DataGrid, Removed_tabitem);

            
            foreach(var tab in tabs)
            {
                if (tab.Visibility != Visibility.Collapsed)
                {
                    tab.IsSelected = true;
                }
                else
                {
                    tab.IsSelected = false;
                }
            }
        }

        private void GeneratingColumns(List<LangData> listName, DataGrid dataGridName, TabItem tabItemName)
        {

            if (listName.Count > 0)
            {
                DataGridTextColumn c1 = new DataGridTextColumn();
                c1.Header = "文本ID";
                c1.Binding = new Binding("UniqueID");
                //c1.Width = 110;
                dataGridName.Columns.Add(c1);

                DataGridTextColumn c2 = new DataGridTextColumn();
                c2.Header = "英文";
                //c2.Width = 200;
                c2.Binding = new Binding("Text_EN");
                dataGridName.Columns.Add(c2);

                DataGridTextColumn c3 = new DataGridTextColumn();
                c3.Header = "汉化";
                //c3.Width = 200;
                c3.Binding = new Binding("Text_ZH");
                dataGridName.Columns.Add(c3);

                dataGridName.ItemsSource = listName;

                tabItemName.Header = tabItemName.Header + "(" + listName.Count + ")";
            }
            else
            {
                tabItemName.Visibility = Visibility.Collapsed;
            }
        }


        



        private void CompareAdded_Button_Click(object sender, RoutedEventArgs e)
        {
            //var addedParser = fileParser.CsvCompareAdded(OldDict, NewDict);
            //var langData = fileParser.CsvDictToModel(addedParser);

            //if (Changed_DataGrid.Items.Count > 1)
            //    langData = null;
            //Changed_DataGrid.Items.Clear();

            //foreach (var data in langData)
            //{
            //    Changed_DataGrid.Items.Add(data);
            //}
            //NewStatus_textBlock.Text = "总计搜索到" + Changed_DataGrid.Items.Count + "条结果。";
        }


        private void SaveToDB_Button_Click(object sender, RoutedEventArgs e)
        {
            //var connDB = new SQLiteController();
            //var dbFileModel = new List<FileModel_IntoDB>();
            //int CompareOptionsIndex = CompareOptions_comboBox.SelectedIndex;
            //int rowStats = CompareOptions_comboBox.SelectedIndex;
            //string updateStats = VersionInput_textBox.Text;
            

            //if (updateStats == "" || updateStats == "更新版本号")
            //    MessageBox.Show("请输入新版本文本的版本号！比如“Update25”等！", "提醒",
            //            MessageBoxButton.OK, MessageBoxImage.Warning);


            //if (CompareOptionsIndex == 1)
            //{
            //    var data = connDB.SearchZHbyIndexWithUnknown(langData);
                
            //    foreach(var d in data)
            //    {
            //        dbFileModel.Add(new FileModel_IntoDB
            //        {
            //            stringID = d.stringID,
            //            stringIndex = d.stringIndex,
            //            stringUnknown = d.stringUnknown,
            //            EN_text = d.EN_text,
            //            ZH_text = d.ZH_text,
            //            Istranslated = d.Istranslated,
            //            //RowStats = rowStats,         //插入数据库内直接定义
            //            UpdateStats = updateStats,
            //        });
            //    }
            //}
            //else
            //{
            //    foreach (var line in langData)
            //    {
            //        dbFileModel.Add(new FileModel_IntoDB
            //        {
            //            stringID = ToInt32(line.ID_Type),
            //            stringIndex = ToInt32(line.ID_Index),
            //            stringUnknown = ToInt32(line.ID_Unknown),
            //            EN_text = line.Text_EN,
            //            ZH_text = line.Text_EN,
            //            Istranslated = 0,
            //            //RowStats = rowStats,         //插入数据库内直接定义
            //            UpdateStats = updateStats,
            //        });
            //    }
            //}


            //switch (CompareOptionsIndex)
            //{
            //    case 0:
            //        connDB.AddDataList(dbFileModel);
            //        MessageBox.Show("加入完成！ 共" + dbFileModel.Count + " 条数据。", "结果",
            //            MessageBoxButton.OK, MessageBoxImage.Information);
            //        break;
            //    case 1:
            //        connDB.MarkChangedDataList(dbFileModel);
            //        MessageBox.Show("标记修改完成！ 共" + dbFileModel.Count + " 条数据。", "结果",
            //            MessageBoxButton.OK, MessageBoxImage.Information);
            //        break;
            //    case 2:
            //        connDB.MarkDeleteDataList(dbFileModel);
            //        MessageBox.Show("标记删除完成！ 共" + dbFileModel.Count + " 条数据。", "结果",
            //            MessageBoxButton.OK, MessageBoxImage.Information);
            //        break;
            //    default:
            //        //connDB.AddDataArray(dbFileModel);
            //        break;
            //}

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
                /*&& langData != null*/)
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
