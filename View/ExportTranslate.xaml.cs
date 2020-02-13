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

            SearchData = DBFile.SearchData("1", 3, false);

            foreach (var data in SearchData)
            {
                if (data.isTranslated == 1 && data.RowStats == 20)
                {
                    data.isTranslated = 3;
                }
                else
                {
                    data.isTranslated = 2;
                }
                
                Translated_DataGrid.Items.Add(data);
            }
            Status_textBlock.Text = "总计搜索到" + Translated_DataGrid.Items.Count + "条结果。";
        }

        private void ExportTranslate_Button_Click(object sender, RoutedEventArgs e)
        {
            var exportTranslate = new ExportFromDB();

            string exportPath = exportTranslate.ExportTranslateDB(SearchData);


            if (File.Exists(exportPath))
            {
                MessageBox.Show("导出成功，请将 " + exportPath + " 发送给校对或导入人员。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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

        
    }
}
