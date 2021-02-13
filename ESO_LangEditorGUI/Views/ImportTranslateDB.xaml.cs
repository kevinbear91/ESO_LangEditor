using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ESO_LangEditorGUI.Views
{
    /// <summary>
    /// ImportTranslateDB.xaml 的交互逻辑
    /// </summary>
    public partial class ImportTranslateDB : Window
    {

        private ParseLangFile parseLangFile = new ParseLangFile();

        public ImportTranslateDB()
        {
            InitializeComponent();

            //DataContext = new ImportTranslateWindowViewModel(LangDataGrid);

        }


        private void FileViewer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listbox = sender as ListBox;

            var selectedItem = (KeyValuePair<string, string>)listbox.SelectedItem; //(Dictionary<string, string>)
            string path = selectedItem.Key;

            //if (path.EndsWith(".json"))
            //    LangDataGrid.LangDataGridDC.GridData = parseLangFile.JsonToLangTextListReader(path);
            //else
            //    LangDataGrid.LangDataGridDC.GridData = parseLangFile.LangTextReaderToListAsync(path);
                
        }
    }
}
