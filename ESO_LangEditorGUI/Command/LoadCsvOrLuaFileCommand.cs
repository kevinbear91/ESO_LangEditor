using ESO_LangEditorGUI.View;
using ESO_LangEditorGUI.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditorGUI.Command
{
    public class LoadCsvOrLuaFileCommand : CommandBase
    {
        private CompareWindowViewModel _compareWindowViewModel { get; }
        private CompareWithDBWindow _compareWithDBWindow { get; }

        public LoadCsvOrLuaFileCommand(CompareWindowViewModel compareWindowViewModel)
        {
            _compareWindowViewModel = compareWindowViewModel;
        }

        public override Task ExecuteAsync(object parameter)
        {
            throw new NotImplementedException();

            OpenFileDialog dialog = new OpenFileDialog { Multiselect = true };
            //dialog.Filter = "csv (*.csv)|.csv";
            if (dialog.ShowDialog(_compareWithDBWindow) == true)
            {
                if (dialog.FileName.EndsWith(".csv") || dialog.FileName.EndsWith(".lua"))
                {
                    foreach (var file in dialog.FileNames)
                    {
                        _compareWindowViewModel.FileList.Add(file);
                    }
                    //textBoxName.Text = dialog.FileName;
                }
                else
                {
                    MessageBox.Show("仅支持读取 .csv 文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    //textBoxName.Text = "";
                }
            }
        }
    }
}
