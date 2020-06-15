using ESO_Lang_Editor.Model;
using ESO_LangEditorLib;
using ESO_LangEditorLib.Models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
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

        private LangDbController db = new LangDbController();

        //public List<NameList> nameList = new List<NameList>();

        //class NameList
        //{
        //    List<LangData> langDataName { get; set; }
        //    DataGrid dataGridName { get; set; }
        //    TabItem tabItemName { get; set; }
        //}

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
            ParserCsv csvparser = new ParserCsv();
            var newList = new List<LangData>();
            //removed = new List<LangData>();



            if (NewFileURLtextBox.Text != "")
            {
                filePath = NewFileURLtextBox.Text;  //&& fileName != NewFileURLtextBox.Text
                LoadCsv_Buton.IsEnabled = false;
                SaveToDB_Button.IsEnabled = false;

                CsvDict = await csvparser.CsvReaderToDictionaryAsync(filePath);

                

                dbLangDict = await Task.Run(() => db.GetAllLangsDictionaryAsync());

                //dbLangDict = await csvparser.CsvReaderToDictionaryAsync(@"D:\eso_zh\eso_zh\en.lang.csv");

                DiffDictionary(dbLangDict, CsvDict);

                Debug.WriteLine(dbLangDict.Count());

                //LoadCsv_Buton.IsEnabled = false;
                SaveToDB_Button.IsEnabled = true;

                


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
                        changed.Add(new LangData { 
                        UniqueID = other.Value.UniqueID,
                        ID = other.Value.ID,
                        Unknown = other.Value.Unknown,
                        Lang_Index = other.Value.Lang_Index,
                        Text_EN = other.Value.Text_EN,
                        Text_ZH = firstValue.Text_ZH,
                        UpdateStats = VersionInput_textBox.Text,
                        IsTranslated = firstValue.IsTranslated,
                        RowStats = 2,
                        });
                        removed.Remove(other.Key);
                    }
                }
                else
                {
                    added.Add(new LangData
                    {
                        UniqueID = other.Value.UniqueID,
                        ID = other.Value.ID,
                        Unknown = other.Value.Unknown,
                        Lang_Index = other.Value.Lang_Index,
                        Text_EN = other.Value.Text_EN,
                        Text_ZH = other.Value.Text_EN,
                        UpdateStats = VersionInput_textBox.Text,
                        IsTranslated = 0,
                        RowStats = 1,
                    });
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
            List<TabItem> tabs = new List<TabItem>
            {
                Added_tabitem,
                Changed_tabitem,
                Removed_tabitem
            };

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



        private async void SaveToDB_Button_Click(object sender, RoutedEventArgs e)
        {
            //var connDB = new SQLiteController();
            //var dbFileModel = new List<FileModel_IntoDB>();
            //int rowStats = CompareOptions_comboBox.SelectedIndex;
            string updateStats = VersionInput_textBox.Text;


            if (updateStats == "" || updateStats == "更新版本号" || updateStats == "更新版本号(必填)")
            {
                MessageBox.Show("请输入新版本文本的版本号！比如“Update25”等！", "提醒",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SaveToDB_Button.IsEnabled = false;
                SaveToDB_Button.Content = "正在应用更改……";

                if(added.Count >= 1)    //判断新加内容是否为空
                {
                    SaveToDB_Button.Content = "正在保存新加内容……";
                    await Task.Run(() => db.AddNewLangs(added));
                }
                    
                if(changed.Count >= 1)   //判断修改内容是否为空
                {
                    SaveToDB_Button.Content = "正在应用修改内容……";
                    await Task.Run(() => db.UpdateLangsEN(changed));
                }
                    
                if(removedList.Count >= 1)   //判断移除内容是否为空
                {
                    SaveToDB_Button.Content = "正在删除移除内容……";
                    await Task.Run(() => db.DeleteLangs(removedList));
                }
                    
            }

            SaveToDB_Button.IsEnabled = true;
            SaveToDB_Button.Content = "保存";






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

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
