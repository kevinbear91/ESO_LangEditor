using ESO_Lang_Editor.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// ExportTranslate.xaml 的交互逻辑
    /// </summary>
    public partial class ExportTranslate : Window
    {
        List<LangSearchModel> SearchData;

        public ExportTranslate()
        {
            InitializeComponent();
            InitDataGrid();
        }


        private void InitDataGrid()
        {
            var DBFile = new SQLiteController();

            if (Translated_DataGrid.Items.Count > 1)
                SearchData = null;
            Translated_DataGrid.Items.Clear();

            SearchData = DBFile.SearchData("1", 3);

            foreach (var data in SearchData)
            {
                Translated_DataGrid.Items.Add(data);
            }
            Status_textBlock.Text = "总计搜索到" + Translated_DataGrid.Items.Count + "条结果。";
        }

        private void ExportTranslate_Button_Click(object sender, RoutedEventArgs e)
        {
            var connDB = new SQLiteController();
            string number = GetRandomNumber();

            if (!Directory.Exists("Export"))
                Directory.CreateDirectory("Export");

            string dbPath = @"Export\Transelate_" + number + ".db";

            connDB.CreateTranslateDBwithData(SearchData, dbPath);


            if (File.Exists(dbPath))
            {
                MessageBox.Show("导出成功，请将 " + dbPath + " 发送给校对或导入人员。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("导出失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string GetRandomNumber()
        {
            Random rnd = new Random();
            string number = rnd.Next(1234, 9876).ToString();
            return number;
        }
    }
}
