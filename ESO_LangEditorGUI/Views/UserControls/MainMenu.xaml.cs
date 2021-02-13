using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ESO_LangEditorGUI.Views.UserControls
{
    /// <summary>
    /// MainMenuList.xaml 的交互逻辑
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
            //DataContext = new MainMenuListViewModel();
        }

        private void CsvCompareWithDB_Click(object sender, RoutedEventArgs e)
        {
            new CompareWithDBWindow().Show();
            //compareWithDBWindows.Show();
        }

        private void ExportTranslate_Click(object sender, RoutedEventArgs e)
        {
            new ExportTranslate().Show();
            //exportTranslateWindow.Show();
        }


        private void ImportTranslate_Click(object sender, RoutedEventArgs e)
        {
            new ImportTranslateDB().Show();
            //importTranslate.Show();
        }

        private void OpenHelpURLinBrowser(object sender, RoutedEventArgs e)
        {
            GoToSite("https://bevisbear.com/eso-lang-editor-help-doc");
        }

        public static void GoToSite(string url)
        {
            //System.Diagnostics.Process.Start(url);
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void DatabaseModiy_Click(object sender, RoutedEventArgs e)
        {
            //var databaseWindow = new DatabaseModifyWindow();

            //databaseWindow.Show();
        }

        private void ExportToStr_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("导出UI STR内容"
                + Environment.NewLine
                + "点击“是”导出，点击“否”什么都不做。"
                + Environment.NewLine
                + "点击之后请耐心等待!", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);

            var strExport = new ExportDbToFile();

            switch (result)
            {
                case MessageBoxResult.Yes:
                    //strExport.ExportStrDB();
                    break;
                case MessageBoxResult.No:
                    break;

            }
        }


        private void PackToRlease_Click(object sender, RoutedEventArgs e)
        {
            new PackToRelase().Show();
        }
    }
}
