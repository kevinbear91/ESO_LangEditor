using ESO_Lang_Editor.Model;
using ESO_LangEditorLib;
using ESO_LangEditorLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// ExportTranslate.xaml 的交互逻辑
    /// </summary>
    public partial class ExportTranslate : Window
    {
        private List<LangText> SearchData;
        //private List<UIstrFile> SearchStrData;

        private bool isStr;

        public ExportTranslate()
        {
            InitializeComponent();

            textBlock_SelectionInfo.Text = "";
            ExportTranslate_Button.IsEnabled = false;

            GeneratingColumns(false);
            InitDataGrid(false);


        }


        private async Task InitDataGrid(bool isUI_Str)
        {
            isStr = isUI_Str;

            textBlock_Info.Text = "正在查找……";

            if (isStr)
            {
                //var strDB = new UI_StrController();

                //if (Translated_DataGrid.Items.Count >= 1)
                //    SearchData = null;
                //Translated_DataGrid.Items.Clear();

                //SearchStrData = strDB.SearchData("1", 3, false);

                //foreach (var data in SearchStrData)
                //{
                //    if (data.isTranslated == 1 && data.RowStats == 20)
                //    {
                //        data.isTranslated = 3;
                //    }
                //    else
                //    {
                //        data.isTranslated = 2;
                //    }

                //    Translated_DataGrid.Items.Add(data);
                //}
                //Status_textBlock.Text = "总计搜索到" + Translated_DataGrid.Items.Count + "条结果。";
            }
            else
            {
                var db = new LangDbController();

                if (Translated_DataGrid.Items.Count >= 1)
                    SearchData = null;
                Translated_DataGrid.Items.Clear();

                SearchData = await Task.Run(() => db.GetLangsListAsync(5, 0, "1"));

                Translated_DataGrid.ItemsSource = SearchData;

                textBlock_Info.Text = "总计搜索到 " + SearchData.Count + " 条结果。";

                ExportTranslate_Button.IsEnabled = true;
            }
            
        }

        private void ExportTranslate_Button_Click(object sender, RoutedEventArgs e)
        {
            var exportTranslate = new ExportFromDB();
            string exportPath;

            if (isStr)
            {
                exportPath = exportTranslate.ExportTranslateDB(SearchData);
            }
            else
            {
                exportPath = exportTranslate.ExportTranslateDB(SearchData);
            }

            //MessageBox.Show(GetTimeToFileName());
            //MessageBox.Show("导出成功，请将 " + exportPath + " 发送给校对或导入人员。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

            if (File.Exists(exportPath))
            {
                MessageBox.Show("导出成功，请将 " + exportPath + " 发送给校对或导入人员。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("导出失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void GeneratingColumns(bool isStr)
        {
            if (isStr)
            {
                DataGridTextColumn c1 = new DataGridTextColumn();
                c1.Header = "UI ID";
                c1.Binding = new Binding("UI_ID");
                c1.Width = 200;
                Translated_DataGrid.Columns.Add(c1);

                DataGridTextColumn c2 = new DataGridTextColumn();
                c2.Header = "英文";
                c2.Width = 200;
                c2.Binding = new Binding("UI_EN");
                Translated_DataGrid.Columns.Add(c2);

                DataGridTextColumn c3 = new DataGridTextColumn();
                c3.Header = "汉化";
                c3.Width = 200;
                c3.Binding = new Binding("UI_ZH");
                Translated_DataGrid.Columns.Add(c3);
            }
            else
            {
                DataGridTextColumn c1 = new DataGridTextColumn();
                c1.Header = "ID";
                c1.Binding = new Binding("UniqueID");
                //c1.Width = 50;
                Translated_DataGrid.Columns.Add(c1);

                DataGridTextColumn c2 = new DataGridTextColumn();
                c2.Header = "英文";
                //c2.Width = 100;
                c2.Binding = new Binding("Text_EN");
                Translated_DataGrid.Columns.Add(c2);

                DataGridTextColumn c3 = new DataGridTextColumn();
                c3.Header = "汉化";
                c3.Width = 200;
                c3.Binding = new Binding("Text_ZH");
                Translated_DataGrid.Columns.Add(c3);
            }

        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void IsStr_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            Translated_DataGrid.Columns.Clear();
            Translated_DataGrid.Items.Clear();

            isStr = true;

            GeneratingColumns(isStr);
            InitDataGrid(isStr);
        }

        private void IsStr_checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Translated_DataGrid.Columns.Clear();
            Translated_DataGrid.Items.Clear();

            isStr = false;

            GeneratingColumns(isStr);
            InitDataGrid(isStr);
        }
        
    }
}
