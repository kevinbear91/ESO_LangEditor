using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorLib;
using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Services.Client;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ESO_LangEditorGUI.View
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

            DataContext = new ImportTranslateWindowViewModel(LangDataGrid);

        }


        private void FileViewer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listbox = sender as ListBox;

            var selectedItem = (KeyValuePair<string, string>)listbox.SelectedItem; //(Dictionary<string, string>)
            string path = selectedItem.Key;

            if (path.EndsWith(".json"))
                LangDataGrid.LangDataGridDC.GridData = parseLangFile.JsonToLangTextListReader(path);
            else
                LangDataGrid.LangDataGridDC.GridData = parseLangFile.LangTextReaderToListAsync(path);
                
        }
    }
}
