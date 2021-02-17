using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.Views.UserControls;
using ESO_LangEditorGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ESO_LangEditorGUI.Command
{
    public class ExportTranslateCommand : CommandBase
    {
        private readonly LangTextRepository _localSearch = new LangTextRepository();
        private readonly ExportTranslateViewModel _exportWindowViewModel;


        public ExportTranslateCommand(ExportTranslateViewModel exportWindowViewModel)
        {
            _exportWindowViewModel = exportWindowViewModel;

            SearchTranslatedText();

            Debug.WriteLine(IsExecuting);
        }

        public override void ExecuteCommand(object parameter)
        {
            ExportDbToFile exporter = new ExportDbToFile();

            bool isExportselectedItems = (bool)parameter;
            List<LangTextDto> exportItems;

            if (isExportselectedItems)
            {
                exportItems = _exportWindowViewModel.SelectedItems;
            }
            else
            {
                //exportItems = _langDatagrid.LangDataGridDC.GridData;
            }

            //var path = exporter.ExportLangTextsAsJson(exportItems, LangChangeType.ChangedZH);
            //_localSearch.UpdateTranslated(exportItems);
            //_exportWindowViewModel.MdNotifyContent = path;

            //MessageBox.Show("导出文件路径为：" + path);
        }

        //public void SetEnable(bool canEnable)
        //{
        //    IsExecuting = !canEnable;
        //}

        private async void SearchTranslatedText()
        {
            IsExecuting = true;

            List<LangTextDto> result;
            result = await Task.Run(() => _localSearch.GetLangTextsAsync("1", SearchTextType.TranslateStatus, SearchPostion.Full));

            if (result.Count >= 1)
                IsExecuting = false;

            //_langDatagrid.LangDataGridDC.GridData = result;
            _exportWindowViewModel.SearchResultInfo = result.Count.ToString();
        }
    }
}
