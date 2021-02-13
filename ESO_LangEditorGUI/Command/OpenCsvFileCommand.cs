using ESO_LangEditorGUI.Views;
using ESO_LangEditorGUI.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ESO_LangEditorGUI.Command
{
    public class OpenCsvFileCommand : CommandBase
    {
        private CompareWindowViewModel _compareWindowViewModel;

        public OpenCsvFileCommand(CompareWindowViewModel compareWindowViewModel)
        {
            _compareWindowViewModel = compareWindowViewModel;
        }

        public override void ExecuteCommand(object parameter)
        {

            CompareWithDBWindow compareWithDBWindow = parameter as CompareWithDBWindow;

            OpenFileDialog dialog = new OpenFileDialog { Multiselect = true };
            //dialog.Filter = "csv (*.csv)|.csv";
            if (dialog.ShowDialog(compareWithDBWindow) == true)
            {
                if (dialog.FileName.EndsWith(".csv") || dialog.FileName.EndsWith(".lua"))
                {
                    foreach (var file in dialog.FileNames)
                    {
                        _compareWindowViewModel.FileList.Add(file);
                        //filepath.Add(file);
                    }
                    if (_compareWindowViewModel.FileList.Count >= 1)
                    {
                        _compareWindowViewModel.FileCount = _compareWindowViewModel.FileList.Count.ToString();
                        _compareWindowViewModel.LoadCsvAndDbCommand.IsExecuting = false;
                        _compareWindowViewModel.PathTooltip();
                    }
                }
                else
                {
                    MessageBox.Show("仅支持读取 .csv 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }
    }
}
