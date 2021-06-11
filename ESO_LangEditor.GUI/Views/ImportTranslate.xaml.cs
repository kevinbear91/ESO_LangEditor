using ESO_LangEditor.GUI.Services;
using ESO_LangEditor.GUI.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ESO_LangEditor.GUI.Views
{
    /// <summary>
    /// ImportTranslateDB.xaml 的交互逻辑
    /// </summary>
    public partial class ImportTranslate : Window
    {

        private LangFileParser parseLangFile = new LangFileParser();

        public ImportTranslate()
        {
            InitializeComponent();

            //DataContext = new ImportTranslateWindowViewModel(LangDataGrid);

        }


        private void FileViewer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = DataContext as ImportTranslateViewModel;

            ListBox listbox = sender as ListBox;

            var selectedItem = listbox.SelectedItem;

            if (vm.GridData.Count >= 1)
            {
                vm.GridData.Clear();
            }

            vm.ReadSelectedFile(selectedItem.ToString());
            //vm.GridData.AddRange(parseLangFile.JsonToLangTextListReader(listbox.SelectedItem.ToString()));

            //if (path.EndsWith(".json"))
            //    LangDataGrid.LangDataGridDC.GridData = parseLangFile.JsonToLangTextListReader(path);
            //else
            //    LangDataGrid.LangDataGridDC.GridData = parseLangFile.LangTextReaderToListAsync(path);

        }
    }
}
