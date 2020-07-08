using ESO_LangEditorGUI.Controller;
using ESO_LangEditorLib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ESO_LangEditorGUI.View
{
    /// <summary>
    /// TextEditor_SearchReplace.xaml 的交互逻辑
    /// </summary>
    public partial class TextEditor_SearchReplace : Window
    {
        List<LangText> LangList;
        ListSearchReplace searchReplace = new ListSearchReplace();

        public TextEditor_SearchReplace(List<LangText> list)
        {
            InitializeComponent();
            LangList = list;
        }

        private void Button_findMatch_Click(object sender, RoutedEventArgs e)
        {

            int findNumber = searchReplace.FindMatch(LangList, searchKeyWord.Text);

            MessageBox.Show("列表内"+ LangList.Count + ". 匹配到 " + findNumber);

        }

        private void Button_Replace_Click(object sender, RoutedEventArgs e)
        {
            var result = searchReplace.SearchReplace(LangList, searchKeyWord.Text, replaceKeyWord.Text);

            MessageBox.Show("列表内" + LangList.Count + ". 匹配到 " + result.Count);
        }
    }
}
