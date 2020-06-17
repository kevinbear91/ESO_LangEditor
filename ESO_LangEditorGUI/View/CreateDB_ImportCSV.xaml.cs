using ESO_LangEditorLib;
using ESO_LangEditorLib.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static System.Convert;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// CreateDB_ImportCSV.xaml 的交互逻辑
    /// </summary>
    public partial class CreateDB_ImportCSV : Window
    {
        private List<LangText> csvData = new List<LangText>();
        private List<LangText> langData = new List<LangText>();
        private List<LuaUIData> luaData = new List<LuaUIData>();
        private int importButtonSwitchInt = 0;

        public CreateDB_ImportCSV()
        {
            InitializeComponent();
        }

        private void LoadEN_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Filter = "csv (*.csv)|.csv";
            if (dialog.ShowDialog(this) == true)
            {
                if (dialog.FileName.EndsWith(".csv"))
                {
                    ImportPathEN_textBox.Text = dialog.FileName;
                }
                else
                {
                    MessageBox.Show("仅支持读取 .csv 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    ImportPathEN_textBox.Text = "";
                }
            }
        }

        private void LoadCN_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Filter = "csv (*.csv)|.csv";
            if (dialog.ShowDialog(this) == true)
            {
                if (dialog.FileName.EndsWith(".csv"))
                {
                    ImportPathCN_textBox.Text = dialog.FileName;
                }
                else
                {
                    MessageBox.Show("仅支持读取 .csv 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    ImportPathCN_textBox.Text = "";
                }
            }
        }

        private async void Import_button_Click(object sender, RoutedEventArgs e)
        {
            switch (importButtonSwitchInt)
            {
                case 0:
                    string csvPath = ImportPathEN_textBox.Text;

                    ParserCsv csvParser = new ParserCsv();

                    LoadEN_button.IsEnabled = false;
                    LoadCN_button.IsEnabled = false;
                    Import_button.IsEnabled = false;
                    Import_button.Content = "正在读取……";
                    

                    try
                    {
                        //csvData = await csvParser.CsvReader(csvPath);

                        LoadEN_button.IsEnabled = false;
                        LoadCN_button.IsEnabled = true;

                        importButtonSwitchInt = 1;

                        Import_button.IsEnabled = true;
                        Import_button.Content = "导入";
                    }
                    catch (IndexOutOfRangeException)
                    {
                        MessageBox.Show("无法读取.csv文件，或许不是转换后的语言.csv文件。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);

                        LoadEN_button.IsEnabled = true;
                        LoadCN_button.IsEnabled = true;

                        importButtonSwitchInt = 0;

                        Import_button.IsEnabled = true;
                        Import_button.Content = "读取";
                    }

                    break;

                case 1:
                    string updateStats = UpdateStats_textBox.Text;

                    Import_button.IsEnabled = false;
                    Import_button.Content = "正在导入";

                    ForEachCsvData(csvData, updateStats, false);

                    importButtonSwitchInt = 2;

                    Import_button.IsEnabled = true;
                    Import_button.Content = "可写入";
                    break;

                case 2:
                    // TO DO...
                    //SqliteController db = new SqliteController();
                    LangDbController db = new LangDbController();
                    Import_button.IsEnabled = false;
                    Import_button.Content = "正在创建数据库……";

                    db.InsertDataFromCsv(langData);

                    //db.InsertDataFromCsv(langData);

                    MessageBox.Show("创建完成！", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                    Import_button.IsEnabled = true;
                    Import_button.Content = "读取";
                    break;


            }
            
            

            

            




            //CsvParser fileParser = new CsvParser();
            //var db = new SQLiteController();
            //Dictionary<string, FileModel_IntoDB> intoDBContent = new Dictionary<string, FileModel_IntoDB>();
            //string updateStats = UpdateStats_textBox.Text;

            //if (updateStats == "" || updateStats == " " || updateStats == null)
            //{
            //    MessageBox.Show("初始版本号不能为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
            //else
            //{
            //    var csvContentEN = fileParser.LoadCsvToDict(ImportPathEN_textBox.Text);
            //    var csvContentCN = fileParser.LoadCsvToDict(ImportPathCN_textBox.Text);


            //    foreach (var en in csvContentEN)
            //    {
            //        var keyField = en.Key.Split(new char[] { '-' }, 3);
            //        intoDBContent.Add(en.Key, new FileModel_IntoDB
            //        {
            //            stringID = ToInt32(keyField[0]),
            //            stringUnknown = ToInt16(keyField[1]),
            //            stringIndex = ToInt32(keyField[2]),
            //            EN_text = en.Value
            //        });
            //    }

            //    foreach (var zh in csvContentCN)
            //    {
            //        if (intoDBContent.ContainsKey(zh.Key))
            //            intoDBContent[zh.Key].ZH_text = zh.Value;
            //    }


            //    List<FileModel_IntoDB> outputList = new List<FileModel_IntoDB>();
            //    foreach (var c in intoDBContent)
            //    {
            //        outputList.Add(new FileModel_IntoDB
            //        {
            //            stringID = c.Value.stringID,
            //            stringUnknown = c.Value.stringUnknown,
            //            stringIndex = c.Value.stringIndex,
            //            EN_text = c.Value.EN_text,
            //            ZH_text = c.Value.ZH_text,
            //            Istranslated = 0,
            //            UpdateStats = updateStats,
            //        });

            //    }

            //    db.CreateDBFileFromCSV(outputList);

            //    MessageBox.Show("创建完成！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            //}


        }

        //public List<LangData> langData { get; set; };

        //private async Task<List<LangData>> CsvDataToLangData (List<CsvData> csvData, bool isZH)
        //{
        //    //List<LangData> langData = new List<LangData>();
            
        //    string updateStats = UpdateStats_textBox.Text;

        //    ForEachCsvData(csvData, updateStats, false);

        //}

        private void ForEachCsvData(IEnumerable<LangText> _csvData, string updateStats, bool isZH)
        {

            if (isZH)
            {
                foreach (var data in _csvData)
                {
                    langData.Add(new LangText
                    {
                        UniqueID = data.UniqueID,
                        ID = data.ID,
                        Unknown = data.Unknown,
                        Lang_Index = data.Lang_Index,
                        Text_ZH = data.Text_EN,       //汉化文本
                        UpdateStats = updateStats,
                        IsTranslated = 0,
                        RowStats = 1,
                    });
                }
                
            }
            else
            {
                foreach (var data in _csvData)
                {
                    langData.Add(new LangText
                    {
                        UniqueID = data.UniqueID,
                        ID = data.ID,
                        Unknown = data.Unknown,
                        Lang_Index = data.Lang_Index,
                        Text_EN = data.Text_EN,          //英语文本
                        Text_ZH = "TODO",
                        UpdateStats = updateStats,
                        IsTranslated = 0,
                        RowStats = 1,
                    });;
                }
                System.Diagnostics.Debug.WriteLine("langdata lines total: " + langData.Count);
            }


            //foreach (var data in _csvData)
            //{
            //    //SaveToLangData(data, updateStats);
            //}
        }

        private void SaveToLangData(CsvData csvData, string updateStats, bool isZH)
        {
            if(isZH)
            {
                langData.Add(new LangText
                {
                    UniqueID = csvData.UniqueID,
                    ID = csvData.Fileid,
                    Unknown = csvData.Unknown,
                    Lang_Index = csvData.Index,
                    Text_ZH = csvData.Text,
                    UpdateStats = updateStats,
                    IsTranslated = 0,
                    RowStats = 1,
                });
            }
            else
            {
                langData.Add(new LangText
                {
                    UniqueID = csvData.UniqueID,
                    ID = csvData.Fileid,
                    Unknown = csvData.Unknown,
                    Lang_Index = csvData.Index,
                    Text_EN = csvData.Text,
                    UpdateStats = updateStats,
                    IsTranslated = 0,
                    RowStats = 1,
                });
            }    
            
        }

        private async void DbAction_button_Click(object sender, RoutedEventArgs e)
        {
            DbAction_button.IsEnabled = false;
            DbAction_button.Content = "正在读取";

            LangDbController db = new LangDbController();
            //var oldLangdb = db.GetSearchOldDataAsync();

            DbAction_button.IsEnabled = true;
            DbAction_button.Content = "读取";

            //foreach (var o in oldLangdb)
            //{

            //    if (o.RowStats == 10)
            //    {
            //        langData.Add(new LangText
            //        {
            //            UniqueID = o.ID + "-" + o.Unknown + "-" + o.Index,
            //            ID = o.ID,
            //            Unknown = o.Unknown,
            //            Lang_Index = o.Index,
            //            Text_EN = o.Text_EN,
            //            Text_ZH = o.Text_SC,
            //            UpdateStats = o.UpdateStats,
            //            IsTranslated = o.isTranslated,
            //            RowStats = 1,
            //        });
            //    }
            //    else if (o.RowStats == 20)
            //    {
            //        langData.Add(new LangText
            //        {
            //            UniqueID = o.ID + "-" + o.Unknown + "-" + o.Index,
            //            ID = o.ID,
            //            Unknown = o.Unknown,
            //            Lang_Index = o.Index,
            //            Text_EN = o.Text_EN,
            //            Text_ZH = o.Text_SC,
            //            UpdateStats = o.UpdateStats,
            //            IsTranslated = o.isTranslated,
            //            RowStats = 2,
            //        });
            //    }
            //    else
            //    {
            //        langData.Add(new LangText
            //        {
            //            UniqueID = o.ID + "-" + o.Unknown + "-" + o.Index,
            //            ID = o.ID,
            //            Unknown = o.Unknown,
            //            Lang_Index = o.Index,
            //            Text_EN = o.Text_EN,
            //            Text_ZH = o.Text_SC,
            //            UpdateStats = o.UpdateStats,
            //            IsTranslated = o.isTranslated,
            //            RowStats = o.RowStats,
            //        });
            //    }

            //    Debug.WriteLine("lang data: " + langData.Count);

            //}

            //db.InsertDataFromCsv(langData);


            //string table = "Client";

            var luaparser = new ParserLuaStr();
            

            var clientlua = luaparser.LuaStrParser(@"D:\eso_zh\ESO_LangEditor\u26\esoui\lang\en_client.lua");
            var pregamelua = luaparser.LuaStrParser(@"D:\eso_zh\ESO_LangEditor\u26\esoui\lang\en_pregame.lua");


            var newLua = Parserlua(clientlua, pregamelua);

            var oldLua = db.GetSearchLuaOldDataAsync();

            compreOldNew(oldLua, newLua);


            db.InsertDataFromold(luaData);

            Debug.WriteLine("lua data: " + luaData.Count);

            MessageBox.Show("完成", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

            //Debug.WriteLine("old data: " + oldDbData.Count);
        }

        private Dictionary<string, LuaUIData> Parserlua(Dictionary<string, LuaUIData> client, Dictionary<string, LuaUIData> pregame)
        {
            Dictionary<string, LuaUIData> parserdlua = new Dictionary<string, LuaUIData>();
            var cli = client;

            foreach(var p in pregame)
            {
                if (client.ContainsKey(p.Value.UniqueID))
                {
                    parserdlua.Add(p.Value.UniqueID, new LuaUIData
                    {
                        UniqueID = p.Value.UniqueID,
                        Text_EN = p.Value.Text_EN,
                        DataEnum = 3,
                    });
                    cli.Remove(p.Key);
                }
                else
                {
                    parserdlua.Add(p.Value.UniqueID, new LuaUIData
                    {
                        UniqueID = p.Value.UniqueID,
                        Text_EN = p.Value.Text_EN,
                        DataEnum = 1,
                    });
                    cli.Remove(p.Key);
                }

            }
            Debug.WriteLine("lua count: " + parserdlua.Count);

            foreach(var c in cli)
            {
                parserdlua.Add(c.Value.UniqueID, new LuaUIData
                {
                    UniqueID = c.Value.UniqueID,
                    Text_EN = c.Value.Text_EN,
                    DataEnum = 2,
                });
            }

            return parserdlua;

            //foreach(var lua in parserdlua)
            //{
            //    Debug.WriteLine("id: {0}, en: {1}, dataEnum: {2}", lua.UniqueID, lua.Text_EN, lua.DataEnum);
            //}



        }

        private void compreOldNew(List<LuaUIDataOld> oldlua, Dictionary<string, LuaUIData> newlua)
        {
            var templua = new Dictionary<string, LuaUIData>();

            foreach (var n in newlua)
            {
                foreach (var o in oldlua)
                {
                    if (n.Key.Contains(o.UI_ID) && !templua.ContainsKey(o.UI_ID))
                    {
                        templua.Add(n.Key, new LuaUIData
                        {
                            UniqueID = n.Value.UniqueID,
                            Text_EN = n.Value.Text_EN,
                            Text_ZH = o.UI_ZH,
                            DataEnum = n.Value.DataEnum,
                            UpdateStats = o.UpdateStats,
                            IsTranslated = o.isTranslated,
                            RowStats = o.RowStats,
                        });
                    }
                }
                
            }

            luaData = templua.Values.ToList();
          
        }
    }
}
