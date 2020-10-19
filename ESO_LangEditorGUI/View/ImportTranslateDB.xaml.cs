using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorLib;
using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Services.Client;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ESO_LangEditorGUI.View
{
    /// <summary>
    /// ImportTranslateDB.xaml 的交互逻辑
    /// </summary>
    public partial class ImportTranslateDB : Window
    {
        //private List<string> filePath = new List<string>();
        //List<string> fileList = new List<string>();
        //List<LangTextDto> SearchData = new List<LangTextDto>();
        //List<LuaUIData> SearchLuaData = new List<LuaUIData>();
        //private List<UIstrFile> SearchStrData;

        //private bool isLua;

        private ParserCsv importDB = new ParserCsv();
        private LangDbController db = new LangDbController();

        private ParseLangFile parseLangFile = new ParseLangFile();

        public ImportTranslateDB()
        {
            InitializeComponent();

            DataContext = new ImportTranslateWindowViewModel(LangDataGrid);

            //GeneratingColumns(false);
            //textBlock_Info.Text = "";
            //textBlock_SelectionInfo.Text = "";
            //textBlock_Total.Text = "";
            //ImportToDB_button.IsEnabled = false;
            //ImportAll_Checkbox.IsEnabled = false;
        }

        

        //private void Import_button_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog dialog = new OpenFileDialog
        //    {
        //        Multiselect = true
        //    };

        //    if (dialog.ShowDialog(this) == true)
        //    {
        //        if (dialog.FileName.EndsWith(".LangDB") || dialog.FileName.EndsWith(".LangUI") || dialog.FileName.EndsWith(".db") || dialog.FileName.EndsWith(".dbUI"))
        //        {
        //            //if (fileList.Count >= 1 || filePath.Count >= 1)
        //            //{
        //            //    fileList.Clear();
        //            //    filePath.Clear();
        //            //    FileID_listBox.ItemsSource = null;
        //            //}

        //            foreach (var file in dialog.FileNames)
        //            {
        //                //fileList.Add(System.IO.Path.GetFileName(file));
        //                //filePath.Add(file);
        //            }

        //            //FileID_listBox.ItemsSource = fileList;
        //            //FileID_listBox.SelectedIndex = 0;

        //            //textBlock_Info.Text = "共 " + filePath.Count + " 个文件";
        //            //ImportToDB_button.IsEnabled = true;
        //        }
        //        else
        //        {
        //            MessageBox.Show("仅支持读取 .LangDB、 .LangUI、.db、.dbUI 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        //            FileID_listBox.ItemsSource = "";
        //        }
        //        //TotalFiles_textBlock.Text = "共 " + fileList.Count().ToString() + " 个文件，已选择 0 个。";
        //    }
        //    //if (fileList.Count > 1 && filePath.Count > 1)
        //    //    ImportAll_Checkbox.IsEnabled = true;
        //}


        //private async void IDList_SelectChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    int seletedIndex = FileID_listBox.SelectedIndex;
        //    string dbPath;

        //    //if (fileList.Count != 0)
        //    //{
        //    //    dbPath = filePath.ElementAt(seletedIndex);
        //    //    await ReadFileByName(dbPath, false);
                
        //    //}
                

        //    //if (filePath.Count >= 0 && seletedIndex == -1)
        //    //{
        //    //    dbPath = filePath.ElementAt(0);
        //    //    //DataGridSet(dbPath);
        //    //}
        //    //else
        //    //{
        //    //    dbPath = filePath.ElementAt(seletedIndex);
        //    //    //DataGridSet(dbPath);
        //    //}


        //}

        private async Task ReadFileByName(string dbPath, bool isSaveToDb)
        {
            ImportToDB_button.IsEnabled = false;

            if (dbPath.EndsWith(".LangUI"))   //Lua Str UI文本
            {
                //SearchLuaData = await importDB.ExportLuaReaderToListAsync(dbPath);
                //TranslateData_dataGrid.ItemsSource = SearchLuaData;

                //textBlock_Info.Text = "共 " + filePath.Count().ToString() + " 个文件，已选择 " + FileID_listBox.SelectedItems.Count + " 个。";
                //textBlock_SelectionInfo.Text = "当前文件共 " + SearchLuaData.Count + " 条文本。";

                //isLua = true;
                
                //if(isSaveToDb)
                //    await db.UpdateLangsZH(SearchLuaData);

                //ImportToDB_button.IsEnabled = true;
            }
            else if (dbPath.EndsWith(".db"))
            {
                var oldTranslate = new ImportOldTranslateDB();

                //SearchData = oldTranslate.FullSearchData(dbPath);
                //TranslateData_dataGrid.ItemsSource = SearchData;
                //textBlock_Info.Text = "共 " + filePath.Count().ToString() + " 个文件，已选择 " + FileID_listBox.SelectedItems.Count + " 个。";
                //textBlock_SelectionInfo.Text = "当前文件共 " + SearchData.Count + " 条文本。";

                //if (isSaveToDb)
                //    await db.UpdateLangsZH(SearchData);

                //ImportToDB_button.IsEnabled = true;

            }
            else if (dbPath.EndsWith(".dbUI"))
            {
                var oldTranslate = new ImportOldTranslateDB();

                //SearchLuaData = oldTranslate.FullSearchStrDB(dbPath, "Pregame");
                //TranslateData_dataGrid.ItemsSource = SearchLuaData;
                //textBlock_Info.Text = "共 " + filePath.Count().ToString() + " 个文件，已选择 " + FileID_listBox.SelectedItems.Count + " 个。";
                //textBlock_SelectionInfo.Text = "当前文件共 " + SearchLuaData.Count + " 条文本。";

               // isLua = true;

                if (isSaveToDb)
                    //await db.UpdateLangsZH(SearchLuaData);

                ImportToDB_button.IsEnabled = true;
            }
            else
            {
                //SearchData = await importDB.ExportReaderToListAsync(dbPath);
                //TranslateData_dataGrid.ItemsSource = SearchData;

                //textBlock_Info.Text = "共 " + filePath.Count().ToString() + " 个文件，已选择 " + FileID_listBox.SelectedItems.Count + " 个。";
                //textBlock_SelectionInfo.Text = "当前文件共 " + SearchData.Count + " 条文本。";

                //if (isSaveToDb)
                 //   await db.UpdateLangsZH(SearchData);
                //await db.UpdateLangsZH(SearchData);
               // ImportToDB_button.IsEnabled = true;
            }
        }

        private async void ImportToDB_button_Click(object sender, RoutedEventArgs e)
        {

            //if (filePath.Count > 1 && ImportAll_Checkbox.IsChecked == true)
            //{
            //    foreach (var f in filePath)
            //    {
            //        //Debug.WriteLine(f);

            //        await ReadFileByName(f,true);
            //        //SearchData = await importDB.ExportReaderToListAsync(f);
            //        //TranslateData_dataGrid.ItemsSource = SearchData;

            //        textBlock_Total.Text = "正在导入第 " + (filePath.IndexOf(f) + 1) + " 项，共" + filePath.Count + " 项";

            //        OpenFile_button.IsEnabled = false;
            //        ImportToDB_button.IsEnabled = false;
            //        ImportAll_Checkbox.IsEnabled = false;
            //        ImportToDB_button.Content = "导入中……";

            //        //await db.UpdateLangsZH(SearchData);


            //    }
            //    ImportToDB_button.Content = "导入";
            //    OpenFile_button.IsEnabled = true;
            //    textBlock_Total.Text = "导入完毕！";
            //}
            //else
            //{
            //    OpenFile_button.IsEnabled = false;
            //    ImportToDB_button.IsEnabled = false;
            //    ImportAll_Checkbox.IsEnabled = false;
            //    ImportToDB_button.Content = "导入中……";

            //    await ReadFileByName(filePath.ElementAt(FileID_listBox.SelectedIndex), true);

            //    OpenFile_button.IsEnabled = true;
            //    ImportToDB_button.Content = "导入";

            //}

        }

        private void FileViewer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listbox = sender as ListBox;

            var selectedItem = (KeyValuePair<string, string>)listbox.SelectedItem; //(Dictionary<string, string>)
            string path = selectedItem.Key;

            if (path.EndsWith(".json"))
                LangDataGrid.LangDataGridDC.GridData = parseLangFile.JsonReader(path);
            else
                LangDataGrid.LangDataGridDC.GridData = parseLangFile.LangTextReaderToListAsync(path);
                
        }
    }
}
