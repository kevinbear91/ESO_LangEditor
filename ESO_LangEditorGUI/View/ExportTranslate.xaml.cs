using ESO_LangEditorGUI.Model;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorLib;
using ESO_LangEditorLib.Models;
using ESO_LangEditorLib.Models.Client.Enum;
using ESO_LangEditorLib.Services.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ESO_LangEditorGUI.View
{
    /// <summary>
    /// ExportTranslate.xaml 的交互逻辑
    /// </summary>
    public partial class ExportTranslate : Window
    {

        public ExportTranslate()
        {
            InitializeComponent();

            DataContext = new ExportTranslateWindowViewModel(LangDataGrid);
        }


        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
    }
}
