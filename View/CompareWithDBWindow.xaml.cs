using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Convert;
using FileHelpers;
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

        public CompareWithDBWindow()
        {
            InitializeComponent();
            CompareOptionsInit();

            SaveToDB_Button.IsEnabled = false;
            Cancel_Button.IsEnabled = false;

            

        }
        public ObservableCollection<string> compareOptions { get; set; }

        private void BrowseOldFileButton_Click(object sender, RoutedEventArgs e)
        {
            //SeletedCsvFileCheck(OldFileURLtextBox);
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
                    MessageBox.Show("仅支持读取 .csv 文件！");
                    textBoxName.Text = "";
                }
            }
        }

        
        private void LoadCsv_Buton_Click(object sender, RoutedEventArgs e)
        {
            int CompareOptionsIndex = CompareOptions_comboBox.SelectedIndex;
            Dictionary<string, string> addedParser;
            //List<LangSearchModel> langData;

            if (NewFileURLtextBox.Text != "")
            {
                OldDict = fileParser.LoadDB();
                //NewStatus_textBlock.Text = "正在读取文件……";
                NewDict = fileParser.LoadCsv(NewFileURLtextBox.Text);

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
                var langData = fileParser.CsvDictToModel(addedParser);

                if (Changed_DataGrid.Items.Count > 1)
                    langData = null;
                Changed_DataGrid.Items.Clear();

                foreach (var data in langData)
                {
                    Changed_DataGrid.Items.Add(data);
                }
                Status_textBlock.Text = "总计搜索到" + Changed_DataGrid.Items.Count + "条结果。";

                //LoadCsv_Buton.IsEnabled = false;

                //OldDict = LoadCsv(OldFileURLtextBox.Text, OldStatus_textBlock);
                //OldDict = fileParser.LoadDB();
                //NewStatus_textBlock.Text = "正在读取文件……";
                //NewDict = fileParser.LoadCsv(NewFileURLtextBox.Text);
            }
            else
            {
                MessageBox.Show("未选择CSV文件！");
            }

        }
       

        
        private void CompareAdded_Button_Click(object sender, RoutedEventArgs e)
        {
            var addedParser = fileParser.CsvCompareAdded(OldDict, NewDict);
            var langData = fileParser.CsvDictToModel(addedParser);

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
        
    }
}
