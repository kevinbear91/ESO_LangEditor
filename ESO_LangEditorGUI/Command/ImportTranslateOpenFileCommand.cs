using ESO_LangEditorGUI.View;
using ESO_LangEditorGUI.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace ESO_LangEditorGUI.Command
{
    public class ImportTranslateOpenFileCommand : CommandBase
    {
        private ImportTranslateWindowViewModel _importTranslateWindowViewModel;

        public ImportTranslateOpenFileCommand(ImportTranslateWindowViewModel importTranslateWindowViewModel)
        {
            _importTranslateWindowViewModel = importTranslateWindowViewModel;
        }

        public override void ExecuteCommand(object parameter)
        {
            ImportTranslateDB importTranslateWindow = parameter as ImportTranslateDB;

            OpenFileDialog dialog = new OpenFileDialog
            {
                Multiselect = true
            };

            if (dialog.ShowDialog(importTranslateWindow) == true)
            {
                if (dialog.FileName.EndsWith(".json") || dialog.FileName.EndsWith(".LangDB") || dialog.FileName.EndsWith(".LangUI") || dialog.FileName.EndsWith(".db") || dialog.FileName.EndsWith(".dbUI"))
                {
                    //int fileCount = _importTranslateWindowViewModel.FileList.Count;
                    if (_importTranslateWindowViewModel.FileList.Count >= 1)
                    {
                        _importTranslateWindowViewModel.FileList.Clear();
                    }

                    foreach (var file in dialog.FileNames)
                    {
                        _importTranslateWindowViewModel.FileList.Add(file, Path.GetFileName(file));
                        //new ESO_LangEditorLib.Services.Client.ParseLangFile().JsonReader(file);
                    }

                    importTranslateWindow.FileViewer.Items.Refresh();
                    //Debug.WriteLine(_importTranslateWindowViewModel.FileList.Count);
                    //FileID_listBox.ItemsSource = fileList;
                    //FileID_listBox.SelectedIndex = 0;

                    //textBlock_Info.Text = "共 " + filePath.Count + " 个文件";
                    //ImportToDB_button.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("仅支持读取 .LangDB、 .LangUI、.db、.dbUI 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    //FileID_listBox.ItemsSource = "";
                }
                //TotalFiles_textBlock.Text = "共 " + fileList.Count().ToString() + " 个文件，已选择 0 个。";
            }
            //if (_importTranslateWindowViewModel.FileList.Count > 1)
            //    _importTranslateWindowViewModel.ImportTraslate.IsExecuting = false;

            //throw new NotImplementedException();
        }
    }
}
