using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.ViewModels;
using System.Collections.Generic;
using System.Windows;

namespace ESO_LangEditor.GUI.Views
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
