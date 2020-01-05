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
                + "，数据库索引：" + EditData.IndexDB 
                + "，类型：" + EditData.ID_Type 
                + "，文本索引：" + EditData.ID_Index + "。";
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
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
