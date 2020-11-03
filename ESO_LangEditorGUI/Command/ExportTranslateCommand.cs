using ESO_LangEditorGUI.View.UserControls;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Models.Client.Enum;
using ESO_LangEditorLib.Services.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ESO_LangEditorGUI.Command
{
    public class ExportTranslateCommand : CommandBase
    {
        private readonly LangTextRepository _localSearch = new LangTextRepository();
        private readonly ExportTranslateWindowViewModel _exportWindowViewModel;
        private readonly UC_LangDataGrid _langDatagrid;


        public ExportTranslateCommand(UC_LangDataGrid langDatagrid, ExportTranslateWindowViewModel exportWindowViewModel)
        {
            _langDatagrid = langDatagrid;
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
                exportItems = _langDatagrid.LangDataGridDC.GridData;
            }

            var path = exporter.ExportLangTextsAsJson(exportItems, LangChangeType.ChangedZH);
            _localSearch.UpdateTranslated(exportItems);
            //_exportWindowViewModel.MdNotifyContent = path;

            MessageBox.Show("导出文件路径为：" + path);
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

            _langDatagrid.LangDataGridDC.GridData = result;
            _exportWindowViewModel.SearchResultInfo = result.Count.ToString();
        }
    }
}
