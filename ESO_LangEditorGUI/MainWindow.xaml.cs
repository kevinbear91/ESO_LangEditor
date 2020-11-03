using ESO_LangEditorGUI.Model;
using ESO_LangEditorGUI.Controller;
using ESO_LangEditorGUI.View;
using ESO_LangEditorLib;
using ESO_LangEditorLib.Models;
using ESO_LangEditorLib.Models.Client;
using System;

using System.Windows;

using System.Diagnostics;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorGUI.Interface;
using MaterialDesignThemes.Wpf;

namespace ESO_LangEditorGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //IDCatalog IDtypeName = new IDCatalog();

        public MainWindow()
        {

            InitializeComponent();
            DataContext = new MainWindowViewModel(LangDataGrid, this);

            GeneratingChips();

        }


        //private void LangSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    DataGrid datagrid = sender as DataGrid;
        //    Point aP = e.GetPosition(datagrid);
        //    IInputElement obj = datagrid.InputHitTest(aP);
        //    DependencyObject target = obj as DependencyObject;

        //    if (SelectedDatas != null)
        //        SelectedDatas.Clear();

        //    while (target != null)
        //    {
        //        if (target is DataGridRow)
        //            if (datagrid.SelectedIndex != -1)
        //            {
        //                if (isLua)
        //                {
        //                    TextEditor textEditor = new TextEditor((LuaUIData)datagrid.SelectedItem, ref IDtypeName, this);
        //                    textEditor.Show();
        //                }
        //                else
        //                {
        //                    //SelectedData = ;
        //                    TextEditor textEditor = new TextEditor((LangTextDto)datagrid.SelectedItem, ref IDtypeName, this);
        //                    textEditor.Show();
        //                    //MessageBox.Show((LangData)datagrid.SelectedItem);
        //                }


        //            }

        //        target = VisualTreeHelper.GetParent(target);
        //    }
        //}


        private void CsvFileCompare_Click(object sender, RoutedEventArgs e)
        {
            //var compareCsvWindows = new CompareCsvWindow();
            //compareCsvWindows.Show();
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


        private void ExportID_Click(object sender, RoutedEventArgs e)
        {
            //int[] ID = new int[] { 38727365, 198758357, 132143172 };

            ////var export = new ExportFromDB();
            //export.ExportIDArray(ID);
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

        private void ExportToLang_Click(object sender, RoutedEventArgs e)
        {
            ToLang();
        }

        private void ExportToCHT_Click(object sender, RoutedEventArgs e)
        {
            ToCHT();
        }

        private void DatabaseModiy_Click(object sender, RoutedEventArgs e)
        {
            //var databaseWindow = new DatabaseModifyWindow();

            //databaseWindow.Show();
        }

        private void ToLang()
        {
            //var export = new ExportFromDB();
            //var tolang = new ThirdPartSerices();

            //MessageBoxResult resultExport = MessageBox.Show("一键导出文本到.lang文件，点击确定开始，不导出请点取消。"
            //    + Environment.NewLine
            //    + "点击确定之后请耐心等待，输出完毕后会弹出提示!", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            //switch (resultExport)
            //{
            //    case MessageBoxResult.OK:
            //        export.ExportAsText();
            //        tolang.ConvertTxTtoLang(false);
            //        MessageBox.Show("导出完成!", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
            //        break;
            //    case MessageBoxResult.Cancel:
            //        break;
            //}
        }

        private void ToCHT()
        {
            //var export = new ExportFromDB();
            //var tolang = new ThirdPartSerices();

            //export.ExportAsText();

            //tolang.OpenCCtoCHT();
            //tolang.ConvertTxTtoLang(true);

            //MessageBox.Show("完成！");
        }


        //private void str_Click(object sender, RoutedEventArgs e)
        //{
        //    var str = new UIstrFile();
        //    //str.createDB();
        //}

        private void ExportToStr_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("导出UI STR内容"
                + Environment.NewLine
                + "点击“是”导出，点击“否”什么都不做。"
                + Environment.NewLine
                + "点击之后请耐心等待!", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);

            var strExport = new ExportFromDB();

            switch (result)
            {
                case MessageBoxResult.Yes:
                    //strExport.ExportStrDB();
                    break;
                case MessageBoxResult.No:
                    break;

            }
        }

        private void GeneratingChips()
        {
            //Chip c1 = new Chip();
            //c1.Content = "112edd";
            //c1.IsDeletable = true;


            //Chips.Children.Add(c1);
        }

        private void PackToRlease_Click(object sender, RoutedEventArgs e)
        {
            new PackToRelase().Show();

            //PackWindow.Show();
        }

        private void Sample2_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
            => Console.WriteLine($"SAMPLE 2: Closing dialog with parameter");
    }
}

