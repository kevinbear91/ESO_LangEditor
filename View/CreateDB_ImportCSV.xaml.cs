using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Convert;
using Microsoft.Win32;
using ESO_Lang_Editor.Model;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// CreateDB_ImportCSV.xaml 的交互逻辑
    /// </summary>
    public partial class CreateDB_ImportCSV : Window
    {
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

        private void Import_button_Click(object sender, RoutedEventArgs e)
        {
            CsvParser fileParser = new CsvParser();
            var db = new SQLiteController();
            Dictionary<string, FileModel_IntoDB> intoDBContent = new Dictionary<string, FileModel_IntoDB>();


            var csvContentEN = fileParser.LoadCsvToDict(ImportPathEN_textBox.Text);
            var csvContentCN = fileParser.LoadCsvToDict(ImportPathCN_textBox.Text);


            foreach (var en in csvContentEN)
            {
                var keyField = en.Key.Split(new char[] { '-' }, 3);
                intoDBContent.Add(en.Key, new FileModel_IntoDB
                {
                    stringID = ToInt32(keyField[0]),
                    stringUnknown = ToInt16(keyField[1]),
                    stringIndex = ToInt32(keyField[2]),
                    EN_text = en.Value

                });
            }

            foreach (var zh in csvContentCN)
            {
                if (intoDBContent.ContainsKey(zh.Key))
                    intoDBContent[zh.Key].ZH_text = zh.Value;
            }


            List<FileModel_IntoDB> outputList = new List<FileModel_IntoDB>();
            foreach(var c in intoDBContent)
            {
                outputList.Add(new FileModel_IntoDB
                {
                    stringID = c.Value.stringID,
                    stringUnknown = c.Value.stringUnknown,
                    stringIndex = c.Value.stringIndex,
                    EN_text = c.Value.EN_text,
                    ZH_text = c.Value.ZH_text
                });
               
            }

            db.CreateDBFileFromCSV(outputList);

            MessageBox.Show("创建完成！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
