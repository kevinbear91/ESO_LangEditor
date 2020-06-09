using ESO_LangEditorLib;
using ESO_LangEditorLib.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private List<LangData> csvData = new List<LangData>();
        private List<LangData> langData = new List<LangData>();
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

                    CsvParser csvParser = new CsvParser();

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
                    Lang_DbController db = new Lang_DbController();
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

        private void ForEachCsvData(IEnumerable<LangData> _csvData, string updateStats, bool isZH)
        {

            if (isZH)
            {
                foreach (var data in _csvData)
                {
                    langData.Add(new LangData
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
                    langData.Add(new LangData
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
                langData.Add(new LangData
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
                langData.Add(new LangData
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

            Lang_DbController db = new Lang_DbController();
            var oldLangdb = db.GetSearchOldDataAsync();

            DbAction_button.IsEnabled = true;
            DbAction_button.Content = "读取";

            foreach(var o in oldLangdb)
            {
                
                        if (o.RowStats == 10)
                        {
                            langData.Add(new LangData
                            {
                                UniqueID = o.ID + "-" + o.Unknown + "-" + o.Index,
                                ID = o.ID,
                                Unknown = o.Unknown,
                                Lang_Index = o.Index,
                                Text_EN = o.Text_EN,
                                Text_ZH = o.Text_SC,
                                UpdateStats = o.UpdateStats,
                                IsTranslated = o.isTranslated,
                                RowStats = 1,
                            });
                        }
                        else if (o.RowStats == 20)
                        {
                            langData.Add(new LangData
                            {
                                UniqueID = o.ID + "-" + o.Unknown + "-" + o.Index,
                                ID = o.ID,
                                Unknown = o.Unknown,
                                Lang_Index = o.Index,
                                Text_EN = o.Text_EN,
                                Text_ZH = o.Text_SC,
                                UpdateStats = o.UpdateStats,
                                IsTranslated = o.isTranslated,
                                RowStats = 2,
                            });
                        }
                        else
                        {
                            langData.Add(new LangData
                            {
                                UniqueID = o.ID + "-" + o.Unknown + "-" + o.Index,
                                ID = o.ID,
                                Unknown = o.Unknown,
                                Lang_Index = o.Index,
                                Text_EN = o.Text_EN,
                                Text_ZH = o.Text_SC,
                                UpdateStats = o.UpdateStats,
                                IsTranslated = o.isTranslated,
                                RowStats = o.RowStats,
                            });
                        }
                
                Debug.WriteLine("lang data: " + langData.Count);

            }

            db.InsertDataFromCsv(langData);

            MessageBox.Show("完成", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

            //Debug.WriteLine("old data: " + oldDbData.Count);
        }
    }
}
