using ESO_Lang_Editor.Model;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using static System.Convert;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// CreateDB_ImportLua.xaml 的交互逻辑
    /// </summary>
    public partial class CreateDB_ImportLua : Window
    {
        ObservableCollection<string> DBNameList = new ObservableCollection<string>();

        public CreateDB_ImportLua()
        {
            InitializeComponent();

            DBNameList.Add("Pregame");
            DBNameList.Add("Client");

            ImportDBName_comboBox.ItemsSource = DBNameList;
            ImportDBName_comboBox.SelectedIndex = 0;
        }

        private void LoadEN_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Filter = "csv (*.csv)|.csv";
            if (dialog.ShowDialog(this) == true)
            {
                if (dialog.FileName.EndsWith(".lua"))
                {
                    ImportPathEN_textBox.Text = dialog.FileName;
                }
                else
                {
                    MessageBox.Show("仅支持读取 .lua 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
                if (dialog.FileName.EndsWith(".str"))
                {
                    ImportPathCN_textBox.Text = dialog.FileName;
                }
                else
                {
                    MessageBox.Show("仅支持读取 .str 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    ImportPathCN_textBox.Text = "";
                }
            }
        }

        private void Import_button_Click(object sender, RoutedEventArgs e)
        {
            UIstrFile fileParser = new UIstrFile();
            string DBname = ImportDBName_comboBox.SelectedItem.ToString();

            var Lua_EN = fileParser.ParserLua(ImportPathEN_textBox.Text);
            var Str_CN = fileParser.ParserStr(ImportPathCN_textBox.Text);

            fileParser.createDB(Lua_EN, Str_CN, DBname);

            MessageBox.Show("创建完成！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
           
        }
    }
}
