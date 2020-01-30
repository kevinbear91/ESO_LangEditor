using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ESO_Lang_Editor.Model;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// ImportTranslateDB.xaml 的交互逻辑
    /// </summary>
    public partial class ImportTranslateDB : Window
    {
        ObservableCollection<string> IDList;
        List<string> filePath;
        List<LangSearchModel> SearchData;

        public ImportTranslateDB()
        {
            InitializeComponent();
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

            var DBFile = new SQLiteController();
            string dbPath;

            if (TranslateData_dataGrid.Items.Count > 1)
                SearchData = null;
                TranslateData_dataGrid.Items.Clear();

            
            if(filePath.Count >= 0 && seletedIndex == -1)
            {
                dbPath = filePath.ElementAt(0);
            }
            else
            {
                dbPath = filePath.ElementAt(seletedIndex);
            }
            

            SearchData = DBFile.FullSearchTranslateDB(dbPath);

            foreach (var data in SearchData)
            {
                TranslateData_dataGrid.Items.Add(data);
            }
            //textBlock_Info.Text = "总计搜索到" + LangSearch.Items.Count + "条结果。";

            TotalFiles_textBlock.Text = "共 " + filePath.Count().ToString() + " 个文件，已选择 " + FileID_listBox.SelectedItems.Count + " 个。";

        }

        private void ImportToDB_button_Click(object sender, RoutedEventArgs e)
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
}
