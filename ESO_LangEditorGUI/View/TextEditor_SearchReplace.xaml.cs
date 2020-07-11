using ESO_Lang_Editor.View;
using ESO_LangEditorGUI.Controller;
using ESO_LangEditorLib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
        TextEditor textWindow;

        public TextEditor_SearchReplace(List<LangText> list, TextEditor window)
        {
            InitializeComponent();
            LangList = list;
            textWindow = window;
            CheckBox_OnlyMatchWord.IsChecked = true;
        }

        private void Button_findMatch_Click(object sender, RoutedEventArgs e)
        {
            bool onlyMatchword = (bool)CheckBox_OnlyMatchWord.IsChecked;
            bool isingoreCase = (bool)CheckBox_IgnoreCase.IsChecked;
            int findNumber;

            if (isingoreCase)
                findNumber = searchReplace.FindMatch(LangList, searchKeyWord.Text, onlyMatchword);
            else
                findNumber = searchReplace.FindMatch(LangList, searchKeyWord.Text, onlyMatchword, RegexOptions.IgnoreCase);

            MessageBox.Show("列表内" + LangList.Count + ". 匹配到 " + findNumber);

        }

        private void Button_Replace_Click(object sender, RoutedEventArgs e)
        {
            bool onlyMatchword = (bool)CheckBox_OnlyMatchWord.IsChecked;
            bool isingoreCase = (bool)CheckBox_IgnoreCase.IsChecked;
            List<LangText> result;

            if (isingoreCase)
                result = searchReplace.SearchReplace(LangList, searchKeyWord.Text, replaceKeyWord.Text, onlyMatchword);
            else
                result = searchReplace.SearchReplace(LangList, searchKeyWord.Text, replaceKeyWord.Text, onlyMatchword, RegexOptions.IgnoreCase);

            textWindow.ApplyReplacedList(result);
            this.Close();

            //MessageBox.Show("列表内" + LangList.Count + ". 匹配到 " + result.Count);
        }
    }
}
