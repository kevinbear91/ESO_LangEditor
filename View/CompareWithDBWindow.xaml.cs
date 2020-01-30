using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using static System.Convert;
using ESO_Lang_Editor.Model;
using System.Collections.ObjectModel;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// CompareWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CompareWithDBWindow : Window
    {
        CsvParser fileParser = new CsvParser();
        private Dictionary<string, string> OldDict;
        private Dictionary<string, string> NewDict;
        private List<LangSearchModel> langData;
        public ObservableCollection<string> compareOptions { get; set; }

        public CompareWithDBWindow()
        {
            InitializeComponent();
            CompareOptionsInit();

            SaveToDB_Button.IsEnabled = false;
            Cancel_Button.IsEnabled = false;

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
                    MessageBox.Show("仅支持读取 .csv 文件！","提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    textBoxName.Text = "";
                }
            }
        }

        
        private void LoadCsv_Buton_Click(object sender, RoutedEventArgs e)
        {
            string fileName;
            //List<LangSearchModel> langData;

            if (NewFileURLtextBox.Text != "")
            {
                fileName = NewFileURLtextBox.Text;  //&& fileName != NewFileURLtextBox.Text
                if (OldDict == null && NewDict == null)
                {
                    OldDict = LoadDB();
                    NewDict = fileParser.LoadCsvToDict(NewFileURLtextBox.Text);

                    CompareData();
                }
                else
                {
                    CompareData();

                    SaveToDB_Button.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("未选择CSV文件！","错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void CompareData()
        {
            int CompareOptionsIndex = CompareOptions_comboBox.SelectedIndex;
            Dictionary<string, string> addedParser;


            if (Changed_DataGrid.Items.Count > 1)
                addedParser = null;
                Changed_DataGrid.Items.Clear();

            switch (CompareOptionsIndex)
            {
                case 0:
                    addedParser = fileParser.CsvCompareAdded(OldDict, NewDict);
                    break;
                case 1:
                    addedParser = fileParser.CsvCompareEdited(OldDict, NewDict);
                    break;
                case 2:
                    addedParser = fileParser.CsvCompareRemove(OldDict, NewDict);
                    break;
                default:
                    addedParser = fileParser.CsvCompareAdded(OldDict, NewDict);
                    break;
            }


            langData = fileParser.CsvDictToModel(addedParser);

            foreach (var data in langData)
            {
                Changed_DataGrid.Items.Add(data);
            }
            Status_textBlock.Text = "总计搜索到" + Changed_DataGrid.Items.Count + "条结果。";
        }

        public Dictionary<string, string> LoadDB()
        {
            var db = new SQLiteController();
            Dictionary<string, string> csvFileModel = new Dictionary<string, string>();

            var searchData = db.FullSearchData();

            foreach (var data in searchData)
            {
                csvFileModel.Add(data.ID_Type + "-" + data.ID_Unknown.ToString() + "-" + data.ID_Index.ToString(), data.Text_EN);
            }

            return csvFileModel;
        }



        private void CompareAdded_Button_Click(object sender, RoutedEventArgs e)
        {
            var addedParser = fileParser.CsvCompareAdded(OldDict, NewDict);
            //var langData = fileParser.CsvDictToModel(addedParser);

            if (Changed_DataGrid.Items.Count > 1)
                langData = null;
            Changed_DataGrid.Items.Clear();

            foreach (var data in langData)
            {
                Changed_DataGrid.Items.Add(data);
            }
            //NewStatus_textBlock.Text = "总计搜索到" + Changed_DataGrid.Items.Count + "条结果。";
        }

        private void CompareOptionsInit()
        {
            compareOptions = new ObservableCollection<string>();

            compareOptions.Add("比较新增");
            compareOptions.Add("比较修改");
            compareOptions.Add("比较删除");

            CompareOptions_comboBox.ItemsSource = compareOptions;
            CompareOptions_comboBox.SelectedIndex = 0;
        }

        private void SaveToDB_Button_Click(object sender, RoutedEventArgs e)
        {
            var connDB = new SQLiteController();
            var dbFileModel = new List<FileModel_IntoDB>();
            int CompareOptionsIndex = CompareOptions_comboBox.SelectedIndex;

            foreach (var line in langData)
            {
                dbFileModel.Add(new FileModel_IntoDB
                {
                    stringID = ToInt32(line.ID_Type),
                    stringIndex = ToInt32(line.ID_Index),
                    //stringOffset = ToInt32(line.ID_Table),
                    stringUnknown = ToInt32(line.ID_Unknown),
                    EN_text = line.Text_EN,
                    ZH_text = line.Text_EN,
                    Istranslated = 0
                });
            }


            switch (CompareOptionsIndex)
            {
                case 0:
                    connDB.AddDataArray(dbFileModel);
                    MessageBox.Show("加入完成！ 共" + dbFileModel.Count + " 条数据。", "结果", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case 1:
                    connDB.UpdateDataArrayFromCompare(dbFileModel);
                    MessageBox.Show("修改完成！ 共" + dbFileModel.Count + " 条数据。", "结果", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case 2:
                    //删除 todo
                    break;
                default:
                    //connDB.AddDataArray(dbFileModel);
                    break;
            }

        }
    }
}
