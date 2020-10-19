using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorLib;
using ESO_LangEditorLib.Models.Client;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static System.Convert;

namespace ESO_LangEditorGUI.View
{
    /// <summary>
    /// CompareWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CompareWithDBWindow : Window
    {

        #region Lang DB 变量
        private Dictionary<string, LangTextDto> dbLangDict;
        private Dictionary<string, LangTextDto> CsvDict;

        private List<LangTextDto> added = new List<LangTextDto>();
        private List<LangTextDto> changed = new List<LangTextDto>();
        private List<LangTextDto> nonChanged = new List<LangTextDto>();
        private List<LangTextDto> removedList = new List<LangTextDto>();
        private Dictionary<string, LangTextDto> removed = new Dictionary<string, LangTextDto>();
        #endregion

        #region Lua Str UI 变量
        //private Dictionary<string, LuaUIData> dbLuaStr;
        //private Dictionary<string, LuaUIData> luaDict;

        //private List<LuaUIData> luaAdded = new List<LuaUIData>();
        //private List<LuaUIData> luaChanged = new List<LuaUIData>();
        //private List<LuaUIData> luaNonChanged = new List<LuaUIData>();
        //private List<LuaUIData> luaRemovedList = new List<LuaUIData>();
        //private Dictionary<string, LuaUIData> luaRemoved = new Dictionary<string, LuaUIData>();
        #endregion


        private LangDbController db = new LangDbController();
        private List<string> filepath = new List<string>();

        private bool isLua;


        public CompareWithDBWindow()
        {
            InitializeComponent();

            DataContext = new CompareWindowViewModel(LangDataGrid);

            //CheckSaveToDBButtonCanEnable();

            //NewFileURLtextBox.Text = "";
        }

        //private void BrowseNewFileButton_Click(object sender, RoutedEventArgs e)
        //{
        //    SeletedCsvFileCheck(NewFileURLtextBox);
        //}

        private void SeletedCsvFileCheck(TextBox textBoxName)
        {
            //OpenFileDialog dialog = new OpenFileDialog { Multiselect = true };
            ////dialog.Filter = "csv (*.csv)|.csv";
            //if (dialog.ShowDialog(this) == true)
            //{
            //    if (dialog.FileName.EndsWith(".csv") || dialog.FileName.EndsWith(".lua"))
            //    {
            //        foreach(var file in dialog.FileNames)
            //        {
            //            filepath.Add(file);
            //        }
            //        textBoxName.Text = dialog.FileName;
            //    }
            //    else
            //    {
            //        MessageBox.Show("仅支持读取 .csv 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            //        textBoxName.Text = "";
            //    }
            //}
        }


        private async void LoadCsv_Buton_Click(object sender, RoutedEventArgs e)
        {
            string filePath;
            string updateStats = VersionInput_textBox.Text;

            //var newList = new List<LangText>();

            if (updateStats == "" || updateStats == "更新版本号" || updateStats == "更新版本号(必填)")
            {
                MessageBox.Show("请输入新版本文本的版本号！比如“Update25”等！", "提醒",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                //if (NewFileURLtextBox.Text != "")
                //{
                //    VersionInput_textBox.IsEnabled = false;

                //    filePath = NewFileURLtextBox.Text;  //&& fileName != NewFileURLtextBox.Text
                //    LoadCsv_Buton.IsEnabled = false;
                //    SaveToDB_Button.IsEnabled = false;

                //    //if (filePath.EndsWith(".lua"))
                //    //{
                //    //    ParserLuaStr luaParser = new ParserLuaStr();

                //    //    luaDict = luaParser.LuaStrParser(filepath);
                //    //    dbLuaStr = await Task.Run(() => db.GetAllLuaLangsDictionaryAsync());

                //    //    DiffDictionary(dbLuaStr, luaDict);
                //    //    Debug.WriteLine(dbLuaStr.Count());
                //    //}
                //    //else
                //    //{
                //    //    ParserCsv csvparser = new ParserCsv();

                //    //    CsvDict = await csvparser.CsvReaderToDictionaryAsync(filePath);
                //    //    dbLangDict = await Task.Run(() => db.GetAllLangsDictionaryAsync());

                //    //    DiffDictionary(dbLangDict, CsvDict);
                //    //    Debug.WriteLine(dbLangDict.Count());
                //    //}

                //    //LoadCsv_Buton.IsEnabled = false;
                //    SaveToDB_Button.IsEnabled = true;

                //}
                //else
                //{
                //    MessageBox.Show("未选择CSV文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
            }

        }

        private void DiffDictionary(Dictionary<string, LangTextDto> first, Dictionary<string, LangTextDto> second)
        {
            //Debug.WriteLine("开始比较。");

            //removed = first;

            //foreach (var other in second)
            //{

            //    if (first.TryGetValue(other.Key, out LangTextDto firstValue))
            //    {
            //        if (firstValue.Text_EN.Equals(other.Value.Text_EN))
            //        {
            //            nonChanged.Add(firstValue);
            //            removed.Remove(other.Key);
            //        }
            //        else
            //        {
            //            changed.Add(new LangTextDto
            //            { 
            //            UniqueID = other.Value.UniqueID,
            //            ID = other.Value.ID,
            //            Unknown = other.Value.Unknown,
            //            Lang_Index = other.Value.Lang_Index,
            //            Text_EN = other.Value.Text_EN,
            //            Text_ZH = firstValue.Text_ZH,
            //            UpdateStats = VersionInput_textBox.Text,
            //            IsTranslated = firstValue.IsTranslated,
            //            RowStats = 2,
            //            });
            //            removed.Remove(other.Key);
            //        }
            //    }
            //    else
            //    {
            //        added.Add(new LangTextDto
            //        {
            //            UniqueID = other.Value.UniqueID,
            //            ID = other.Value.ID,
            //            Unknown = other.Value.Unknown,
            //            Lang_Index = other.Value.Lang_Index,
            //            Text_EN = other.Value.Text_EN,
            //            Text_ZH = other.Value.Text_EN,
            //            UpdateStats = VersionInput_textBox.Text,
            //            IsTranslated = 0,
            //            RowStats = 1,
            //        });
            //        removed.Remove(other.Key);
            //    }
            //}

            //removedList = removed.Values.ToList();

            //SetCompareUI(false);

        }


        //private void DiffDictionary(Dictionary<string, LuaUIData> first, Dictionary<string, LuaUIData> second)
        //{
        //    Debug.WriteLine("开始比较。");

        //    luaRemoved = first;

        //    foreach (var other in second)
        //    {

        //        if (first.TryGetValue(other.Key, out LuaUIData firstValue))
        //        {
        //            if (firstValue.Text_EN.Equals(other.Value.Text_EN))
        //            {
        //                luaNonChanged.Add(firstValue);
        //                luaRemoved.Remove(other.Key);
        //            }
        //            else
        //            {
        //                luaChanged.Add(new LuaUIData
        //                {
        //                    UniqueID = other.Value.UniqueID,
        //                    Text_EN = other.Value.Text_EN,
        //                    Text_ZH = firstValue.Text_ZH,
        //                    UpdateStats = VersionInput_textBox.Text,
        //                    IsTranslated = firstValue.IsTranslated,
        //                    RowStats = 2,
        //                    DataEnum = other.Value.DataEnum,
        //                });
        //                luaRemoved.Remove(other.Key);
        //            }
        //        }
        //        else
        //        {
        //            luaAdded.Add(new LuaUIData
        //            {
        //                UniqueID = other.Value.UniqueID,
        //                Text_EN = other.Value.Text_EN,
        //                Text_ZH = other.Value.Text_EN,
        //                UpdateStats = VersionInput_textBox.Text,
        //                IsTranslated = 0,
        //                RowStats = 1,
        //                DataEnum = other.Value.DataEnum,
        //            });
        //            luaRemoved.Remove(other.Key);
        //        }
        //    }

        //    luaRemovedList = luaRemoved.Values.ToList();

        //    SetCompareUI(true);
        //}

        //private void SetCompareUI(bool isLua)
        //{
        //    List<TabItem> tabs = new List<TabItem>
        //    {
        //        Added_tabitem,
        //        Changed_tabitem,
        //        Removed_tabitem
        //    };

        //    if(isLua)
        //    {
        //        GeneratingColumns(luaAdded, Added_DataGrid, Added_tabitem);
        //        GeneratingColumns(luaChanged, Changed_DataGrid, Changed_tabitem);
        //        GeneratingColumns(luaRemovedList, Removed_DataGrid, Removed_tabitem);
        //    }
        //    else
        //    {
        //        GeneratingColumns(added, Added_DataGrid, Added_tabitem);
        //        GeneratingColumns(changed, Changed_DataGrid, Changed_tabitem);
        //        GeneratingColumns(removedList, Removed_DataGrid, Removed_tabitem);
        //    }

        //    foreach(var tab in tabs)
        //    {
        //        if (tab.Visibility != Visibility.Collapsed)
        //        {
        //            tab.IsSelected = true;
        //        }
        //        else
        //        {
        //            tab.IsSelected = false;
        //        }
        //    }
        //}

        private void GeneratingColumns(List<LangTextDto> listName, DataGrid dataGridName, TabItem tabItemName)
        {
            //if (listName.Count > 0)
            //{
            //    DataGridTextColumn c1 = new DataGridTextColumn();
            //    c1.Header = "文本ID";
            //    c1.Binding = new Binding("UniqueID");
            //    //c1.Width = 110;
            //    dataGridName.Columns.Add(c1);

            //    DataGridTextColumn c2 = new DataGridTextColumn();
            //    c2.Header = "英文";
            //    //c2.Width = 200;
            //    c2.Binding = new Binding("Text_EN");
            //    dataGridName.Columns.Add(c2);

            //    DataGridTextColumn c3 = new DataGridTextColumn();
            //    c3.Header = "汉化";
            //    //c3.Width = 200;
            //    c3.Binding = new Binding("Text_ZH");
            //    dataGridName.Columns.Add(c3);

            //    dataGridName.ItemsSource = listName;

            //    tabItemName.Header = tabItemName.Header + "(" + listName.Count + ")";
            //}
            //else
            //{
            //    tabItemName.Visibility = Visibility.Collapsed;
            //}
        }


        //private void GeneratingColumns(List<LuaUIData> listName, DataGrid dataGridName, TabItem tabItemName)
        //#region Lua Str
        //{
        //    //if (listName.Count > 0)
        //    //{
        //    //    DataGridTextColumn c1 = new DataGridTextColumn();
        //    //    c1.Header = "文本ID";
        //    //    c1.Binding = new Binding("UniqueID");
        //    //    //c1.Width = 110;
        //    //    dataGridName.Columns.Add(c1);

        //    //    DataGridTextColumn c2 = new DataGridTextColumn();
        //    //    c2.Header = "英文";
        //    //    //c2.Width = 200;
        //    //    c2.Binding = new Binding("Text_EN");
        //    //    dataGridName.Columns.Add(c2);

        //    //    DataGridTextColumn c3 = new DataGridTextColumn();
        //    //    c3.Header = "汉化";
        //    //    //c3.Width = 200;
        //    //    c3.Binding = new Binding("Text_ZH");
        //    //    dataGridName.Columns.Add(c3);

        //    //    dataGridName.ItemsSource = listName;

        //    //    tabItemName.Header = tabItemName.Header + "(" + listName.Count + ")";
        //    //}
        //    //else
        //    //{
        //    //    tabItemName.Visibility = Visibility.Collapsed;
        //    //}
        //}
        //#endregion


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

                if (added.Count >= 1)    //判断新加内容是否为空
                {
                    SaveToDB_Button.Content = "正在保存新加内容……";
                    await Task.Run(() => db.AddNewLangs(added));
                }

                if (changed.Count >= 1)   //判断修改内容是否为空
                {
                    SaveToDB_Button.Content = "正在应用修改内容……";
                    await Task.Run(() => db.UpdateLangsEN(changed));
                }

                if (removedList.Count >= 1)   //判断移除内容是否为空
                {
                    SaveToDB_Button.Content = "正在删除移除内容……";
                    await Task.Run(() => db.DeleteLangs(removedList));
                }

                //if (luaAdded.Count >= 1)    //判断新加内容是否为空  --LUA
                //{
                //    SaveToDB_Button.Content = "正在保存新加内容……";
                //    await Task.Run(() => db.AddNewLangs(luaAdded));
                //}

                //if (luaChanged.Count >= 1)   //判断修改内容是否为空  --LUA
                //{
                //    SaveToDB_Button.Content = "正在应用修改内容……";
                //    await Task.Run(() => db.UpdateLangsEN(luaChanged));
                //}

                //if (luaRemovedList.Count >= 1)   //判断移除内容是否为空  --LUA
                //{
                //    SaveToDB_Button.Content = "正在删除移除内容……";
                //    await Task.Run(() => db.DeleteLangs(luaRemovedList));
                //}

            }

            SaveToDB_Button.IsEnabled = true;
            SaveToDB_Button.Content = "保存";
        }

        private void VersionInput_GetFocus(object sender, RoutedEventArgs e)
        {
            //if (VersionInput_textBox.Text == "更新版本号(必填)")
            //    VersionInput_textBox.Text = "";
        }

        private void VersionInput_Lostfocus(object sender, RoutedEventArgs e)
        {
            //if (VersionInput_textBox.Text == "")
            //    VersionInput_textBox.Text = "更新版本号(必填)";
            //CheckSaveToDBButtonCanEnable();

        }

        //private bool CheckSaveToDBButtonCanEnable()
        //{
        //    if (NewFileURLtextBox.Text != "" 
        //        && VersionInput_textBox.Text != ""
        //        && VersionInput_textBox.Text != " "
        //        && VersionInput_textBox.Text != "更新版本号(必填)"
        //        /*&& langData != null*/)
        //    {
        //        SaveToDB_Button.IsEnabled = true;
        //        return true;
        //    }
        //    else
        //    {
        //        SaveToDB_Button.IsEnabled = false;
        //        return false;
        //    }
        //}

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
