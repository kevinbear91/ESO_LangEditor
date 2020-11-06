using ESO_LangEditorGUI.View;
using ESO_LangEditorGUI.Controller;
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
using System.Globalization;
using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.ViewModels;

namespace ESO_LangEditorGUI.View
{
    /// <summary>
    /// TextEditor_SearchReplace.xaml 的交互逻辑
    /// </summary>
    public partial class TextEditor_SearchReplace : Window
    {
        //List<LangTextDto> LangList;
       // ListSearchReplace searchReplace = new ListSearchReplace();
        //TextEditor textWindow;

        public TextEditor_SearchReplace(List<LangTextDto> list)
        {
            InitializeComponent();
            //LangList = list;
            DataContext = new SearchReplaceViewModel(list, LangDataGrid);
            //textWindow = window;
            //CheckBox_OnlyMatchWord.IsChecked = true;
        }

        private void Button_Replace_Click(object sender, RoutedEventArgs e)
        {
            bool onlyMatchword = (bool)CheckBox_OnlyMatchWord.IsChecked;
            bool isingoreCase = (bool)CheckBox_IgnoreCase.IsChecked;
            List<LangTextDto> result;
            string keyWord;
            
            try
            {
                if (!string.IsNullOrEmpty(searchKeyWord.Text) && !string.IsNullOrEmpty(replaceKeyWord.Text))
                {
                    if (searchKeyWord.Text.Contains('?'))
                        keyWord = searchKeyWord.Text.Replace("?", @"\?");
                    else
                        keyWord = searchKeyWord.Text;

                    //if (isingoreCase)
                    //    result = searchReplace.SearchReplace(LangList, keyWord, replaceKeyWord.Text, onlyMatchword);
                    //else
                    //    result = searchReplace.SearchReplace(LangList, keyWord, replaceKeyWord.Text, onlyMatchword, RegexOptions.IgnoreCase);

                    //textWindow.ApplyReplacedList(result);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("查找内容与替换内容均不许为空，空格请谨慎匹配！");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("替换时发生了错误，错误信息： " + Environment.NewLine + ex.ToString(), "错误",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            

            //MessageBox.Show("列表内" + LangList.Count + ". 匹配到 " + result.Count);
        }
        
    }
}
