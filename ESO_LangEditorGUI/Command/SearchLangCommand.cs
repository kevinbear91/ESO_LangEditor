using ESO_LangEditorGUI.Interface;
using ESO_LangEditorGUI.View;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorLib.Models.Client.Enum;
using ESO_LangEditorLib.Services.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ESO_LangEditorGUI.Command
{
    public class SearchLangCommand : CommandBase
    {
        private readonly SearchPostion _searchPostion;
        private readonly SearchTextType _searchType;
        private readonly string _keyword;
        private readonly bool _isLua;
        private readonly DataGridViewModel _langDataGrid;
        private readonly SearchLangText search = new SearchLangText();
        private readonly LangTextRepository _localSearch = new LangTextRepository();
        private readonly MainWindowViewModel _mainView;

        private readonly UC_LangDataGrid _langDataGriduc;

        //public SearchLangCommand(SearchPostion searchPostion, SearchTextType searchType, string keyWord, bool isLua, DataGridViewModel langDataGrid)
        //{
        //    _searchPostion = searchPostion;
        //    _searchType = searchType;
        //    _keyword = keyWord;
        //    _isLua = isLua;
        //    _langDataGrid = langDataGrid;
        //}

        //public SearchLangCommand(MainWindowViewModel mainView, DataGridViewModel langDataGrid)
        //{
        //    _mainView = mainView;
        //    _langDataGrid = langDataGrid;

        //    //Debug.WriteLine(_mainView.ToString());

        //}

        public SearchLangCommand(MainWindowViewModel mainView)
        {
            _mainView = mainView;
            //_langDataGriduc = langDataGrid;

            //Debug.WriteLine(_mainView.ToString());

        }

        public override async Task ExecuteAsync(object parameter)
        {
            UC_LangDataGrid _langDatagrid = parameter as UC_LangDataGrid;

            if (App.OnlineMode)
            {
                _langDatagrid.LangDataGrid.ItemsSource = await search.GetLangTexts(_mainView.SelectedSearchPostion, _mainView.SelectedSearchTextType, _mainView.Keyword);
            }
            else
            {
                _langDatagrid.LangDataGrid.ItemsSource = _localSearch.GetLangTexts(_mainView.Keyword, _mainView.SelectedSearchTextType);
            }
            
            //_langDataGrid.GridData = 
            
        }

    }
}
