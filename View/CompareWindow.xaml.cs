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
using FileHelpers;
using ESO_Lang_Editor.Model;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// CompareWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CompareWindow : Window
    {
        CsvParser fileParser = new CsvParser();
        private Dictionary<string, string> OldDict;
        private Dictionary<string, string> NewDict;

        public CompareWindow()
        {
            InitializeComponent();

            CompareAdded_Button.IsEnabled = false;
            CompareChanged_Button.IsEnabled = false;
            CompareDeleted_Button.IsEnabled = false;
            SaveToDB_Button.IsEnabled = false;
            Cancel_Button.IsEnabled = false;
        }

        private void BrowseOldFileButton_Click(object sender, RoutedEventArgs e)
        {
            SeletedCsvFileCheck(OldFileURLtextBox);
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
            if (OldFileURLtextBox.Text != "" && NewFileURLtextBox.Text != "")
            {
                //LoadCsv_Buton.IsEnabled = false;
                
                OldDict = LoadCsv(OldFileURLtextBox.Text, OldStatus_textBlock);
                //NewStatus_textBlock.Text = "正在读取文件……";
                NewDict = LoadCsv(NewFileURLtextBox.Text, NewStatus_textBlock);
            }

            CompareAdded_Button.IsEnabled = true;
            CompareChanged_Button.IsEnabled = true;
            CompareDeleted_Button.IsEnabled = true;

        }


        private Dictionary<string, string> LoadCsv(string path, TextBlock statusTextBox)
        {
            var engine = new FileHelperEngine<FileModel_Csv>(Encoding.UTF8);
            //var engine2 = new FileHelperAsyncEngine<FileModel_Csv>(Encoding.UTF8);
            statusTextBox.Text = "正在读取文件……";
            var reader = engine.ReadFile(path).ToList();
            statusTextBox.Text = "正在转换……";
            var Dict = fileParser.CsvListToDict(reader);

            statusTextBox.Text = "共 " + (Dict.Count+1) + " 条数据。" ;
            return Dict;
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
            NewStatus_textBlock.Text = "总计搜索到" + Changed_DataGrid.Items.Count + "条结果。";
        }
    }
}
