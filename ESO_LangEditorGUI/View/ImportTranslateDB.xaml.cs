using ESO_Lang_Editor.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// ImportTranslateDB.xaml 的交互逻辑
    /// </summary>
    public partial class ImportTranslateDB : Window
    {
        private ObservableCollection<string> IDList;
        private List<string> filePath;
        private List<LangSearchModel> SearchData;
        private List<UIstrFile> SearchStrData;

        private bool isStr;

        public ImportTranslateDB()
        {
            InitializeComponent();
        }

        private void GeneratingColumns(bool isStr)
        {
            if (isStr)
            {
                DataGridTextColumn c1 = new DataGridTextColumn();
                c1.Header = "UI ID";
                c1.Binding = new Binding("UI_ID");
                c1.Width = 200;
                TranslateData_dataGrid.Columns.Add(c1);

                DataGridTextColumn c2 = new DataGridTextColumn();
                c2.Header = "英文";
                c2.Width = 200;
                c2.Binding = new Binding("UI_EN");
                TranslateData_dataGrid.Columns.Add(c2);

                DataGridTextColumn c3 = new DataGridTextColumn();
                c3.Header = "汉化";
                c3.Width = 200;
                c3.Binding = new Binding("UI_ZH");
                TranslateData_dataGrid.Columns.Add(c3);
            }
            else
            {
                DataGridTextColumn c1 = new DataGridTextColumn();
                c1.Header = "ID";
                c1.Binding = new Binding("ID_Type");
                c1.Width = 80;
                TranslateData_dataGrid.Columns.Add(c1);

                DataGridTextColumn c2 = new DataGridTextColumn();
                c2.Header = "Unknown";
                c2.Width = 40;
                c2.Binding = new Binding("ID_Unknown");
                TranslateData_dataGrid.Columns.Add(c2);

                DataGridTextColumn c3 = new DataGridTextColumn();
                c3.Header = "索引";
                c3.Width = 50;
                c3.Binding = new Binding("ID_Index");
                TranslateData_dataGrid.Columns.Add(c3);

                DataGridTextColumn c4 = new DataGridTextColumn();
                c4.Header = "英文";
                c4.Width = 100;
                c4.Binding = new Binding("Text_EN");
                TranslateData_dataGrid.Columns.Add(c4);

                DataGridTextColumn c5 = new DataGridTextColumn();
                c5.Header = "汉化";
                c5.Width = 300;
                c5.Binding = new Binding("Text_SC");
                TranslateData_dataGrid.Columns.Add(c5);
            }

        }

        private void Import_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;

            filePath = new List<string>();
            List<string> fileList = new List<string>();


            if (dialog.ShowDialog(this) == true)
            {
                if (dialog.FileName.EndsWith(".db"))
                {
                    foreach (var file in dialog.FileNames)
                    {
                        fileList.Add(System.IO.Path.GetFileName(file));
                        filePath.Add(file);
                    }
                    FileID_listBox.ItemsSource = fileList;
                }
                else
                {
                    MessageBox.Show("仅支持读取 .db 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    FileID_listBox.ItemsSource = "";
                }
                TotalFiles_textBlock.Text = "共 " + fileList.Count().ToString() + " 个文件，已选择 0 个。";
            }
        }


        private void IDList_SelectChanged(object sender, SelectionChangedEventArgs e)
        {
            int seletedIndex = FileID_listBox.SelectedIndex;

            string dbPath;

            if (TranslateData_dataGrid.Items.Count > 1)
                SearchData = null;
            TranslateData_dataGrid.Items.Clear();


            if (filePath.Count >= 0 && seletedIndex == -1)
            {
                dbPath = filePath.ElementAt(0);
                DataGridSet(dbPath);
            }
            else
            {
                dbPath = filePath.ElementAt(seletedIndex);
                DataGridSet(dbPath);
            }

            if (isStr)
            {
                var strDB = new UI_StrController();
                SearchStrData = strDB.FullSearchStrDB(dbPath);

                foreach (var data in SearchStrData)
                {
                    TranslateData_dataGrid.Items.Add(data);
                }
                //textBlock_Info.Text = "总计搜索到" + LangSearch.Items.Count + "条结果。";

                TotalFiles_textBlock.Text = "共 " + filePath.Count().ToString() + " 个文件，已选择 " + FileID_listBox.SelectedItems.Count + " 个。";
            }
            else
            {
                var DBFile = new SQLiteController();
                SearchData = DBFile.FullSearchTranslateDB(dbPath);

                foreach (var data in SearchData)
                {
                    TranslateData_dataGrid.Items.Add(data);
                }
                //textBlock_Info.Text = "总计搜索到" + LangSearch.Items.Count + "条结果。";

                TotalFiles_textBlock.Text = "共 " + filePath.Count().ToString() + " 个文件，已选择 " + FileID_listBox.SelectedItems.Count + " 个。";
            }
            


            

        }

        private void ImportToDB_button_Click(object sender, RoutedEventArgs e)
        {
            if (isStr)
            {
                var strDB = new UI_StrController();
                bool isSuccess;
                List<UIstrFile> importData;

                foreach (var s in FileID_listBox.SelectedItems)
                {
                    //按GUI列表选择的对象数目来读取索引，用索引探测已选定的文件路径来搜索翻译后的数据库文件，然后将所有内容存储到变量中。
                    importData = strDB.FullSearchStrDB(filePath.ElementAt(FileID_listBox.Items.IndexOf(s)));
                    isSuccess = strDB.UpdateTextScFromImportDB(importData);;

                    if (isSuccess)
                        MessageBox.Show("导入成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("导入失败！请检查文件！", "失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                var DBFile = new SQLiteController();
                bool isSuccess;
                List<LangSearchModel> importData;

                foreach (var s in FileID_listBox.SelectedItems)
                {
                    //按GUI列表选择的对象数目来读取索引，用索引探测已选定的文件路径来搜索翻译后的数据库文件，然后将所有内容存储到变量中。
                    importData = DBFile.FullSearchTranslateDB(filePath.ElementAt(FileID_listBox.Items.IndexOf(s)));
                    isSuccess = DBFile.UpdateTextScFromImportDB(importData);

                    if (isSuccess)
                        MessageBox.Show("导入成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("导入失败！请检查文件！", "失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            

        }

        public void DataGridSet(string DBPath)
        {
            TranslateData_dataGrid.Columns.Clear();
            TranslateData_dataGrid.Items.Clear();

            if (CheckTableIfExist("Pregame", DBPath) || CheckTableIfExist("Client", DBPath))
            {
                isStr = true;
                GeneratingColumns(true);
            }
            else
            {
                isStr = false;
                GeneratingColumns(false);
            }


        }

        #region  检查表是否存在

        public bool CheckTableIfExist(string tableName, string DBPath)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + DBPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;

                try
                {
                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='" + tableName + "';";
                    var table = cmd.ExecuteScalar();

                    if (table != null && table.ToString() == tableName)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    throw new Exception("查询数据表" + tableName + "失败：" + ex.Message);
                }


            }
        }

        #endregion
    }
}
