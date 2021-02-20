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
    public partial class SearchReplaceWindow : Window
    {
        public SearchReplaceWindow(List<LangTextDto> langTextDtos)
        {
            InitializeComponent();

            var vm = DataContext as SearchReplaceWindowViewModel;
            vm.Load(langTextDtos);
        }
    }
}
