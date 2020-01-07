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
using ESO_Lang_Editor.Model;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// TextEditor.xaml 的交互逻辑
    /// </summary>
    public partial class TextEditor : Window
    {
        private LangSearchModel EditData;
        public TextEditor(LangSearchModel LangData)
        {
            
            InitializeComponent();
            EditData = LangData;
            textBox_EN.Text = EditData.Text_EN;
            textBox_ZH.Text = EditData.Text_SC;
            textblock_information.Text = "当前表：" + EditData.ID_Table 
                //+ "，数据库索引：" + EditData.IndexDB 
                + "，类型：" + EditData.ID_Type
                + "，未知列：" + EditData.ID_Unknown
                + "，文本索引：" + EditData.ID_Index + "。";
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            var EditedData = new LangSearchModel();
            var connDB = new SQLiteController();

            EditedData.ID_Table = EditData.ID_Table;
            EditedData.ID_Unknown = EditData.ID_Unknown;
            EditedData.ID_Index = EditData.ID_Index;
            EditedData.Text_SC = textBox_ZH.Text;

            var isSuccess = connDB.UpdateDataFromEditor(EditedData);
            MessageBox.Show(isSuccess);
            this.Close();
        }

        /*
        public TextEditor(LangSearchModel Data) : base()
        {
            
            Data = EditData;
        }
        */


    }
}
