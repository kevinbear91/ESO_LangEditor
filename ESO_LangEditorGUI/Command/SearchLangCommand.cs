using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.View.UserControls;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ESO_LangEditorGUI.Command
{
    public class SearchLangCommand : CommandBaseAsync
    {
        private readonly SearchLangText search = new SearchLangText();
        private readonly LangTextRepository _localSearch = new LangTextRepository();
        private readonly MainWindowViewModel _mainView;

        public SearchLangCommand(MainWindowViewModel mainView)
        {
            _mainView = mainView;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            UC_LangDataGrid _langDatagrid = parameter as UC_LangDataGrid;
            List<LangTextDto> result;

            if (App.OnlineMode)
            {
                result = await search.GetLangTexts(_mainView.SelectedSearchPostion, _mainView.SelectedSearchTextType, _mainView.Keyword);
            }
            else
            {
                result = await Task.Run(() => _localSearch.GetLangTextsAsync(_mainView.Keyword, _mainView.SelectedSearchTextType, _mainView.SelectedSearchPostion));
            }
            _langDatagrid.LangDataGridDC.GridData = result;
            _mainView.SearchInfo = result.Count.ToString();
        }

        //public override async void ExecuteCommand(object parameter)
        //{
        //    UC_LangDataGrid _langDatagrid = parameter as UC_LangDataGrid;
        //    List<LangTextDto> result;

        //    if (App.OnlineMode)
        //    {
        //        result = await search.GetLangTexts(_mainView.SelectedSearchPostion, _mainView.SelectedSearchTextType, _mainView.Keyword);
        //    }
        //    else
        //    {
        //        result = await Task.Run(() => _localSearch.GetLangTextsAsync(_mainView.Keyword, _mainView.SelectedSearchTextType, _mainView.SelectedSearchPostion));
        //    }
        //    _langDatagrid.LangDataGridDC.GridData = result;
        //    _mainView.SearchInfo = result.Count.ToString();
        //}
    }
}
