using System;
using System.Collections.Generic;
using System.Windows;
using ESO_LangEditor.Core.Models;
using ESO_LangEditorGUI.ViewModels;

namespace ESO_LangEditorGUI.Views
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
            //DataContext = new SearchReplaceViewModel(list, LangDataGrid);
            //textWindow = window;
            //CheckBox_OnlyMatchWord.IsChecked = true;
        }

       
        
    }
}
