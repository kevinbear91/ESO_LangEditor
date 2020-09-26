using ESO_LangEditorGUI.Interface;
using ESO_LangEditorGUI.Models.Enum;
using ESO_LangEditorGUI.View;
using ESO_LangEditorGUI.ViewModels;
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
        private readonly MainWindowViewModel _mainView;

        //public SearchLangCommand(SearchPostion searchPostion, SearchTextType searchType, string keyWord, bool isLua, DataGridViewModel langDataGrid)
        //{
        //    _searchPostion = searchPostion;
        //    _searchType = searchType;
        //    _keyword = keyWord;
        //    _isLua = isLua;
        //    _langDataGrid = langDataGrid;
        //}

        public SearchLangCommand(MainWindowViewModel mainView, DataGridViewModel langDataGrid)
        {
            _mainView = mainView;
            _langDataGrid = langDataGrid;
            //IsExecuting = false;

            //IsExecuting = false;

            //Debug.WriteLine(_mainView.ToString());

        }

        public override async Task ExecuteAsync(object parameter)
        {
            _langDataGrid.GridData = await search.GetLangText(_mainView.SelectedSearchPostion, _mainView.SelectedSearchTextType, _mainView.Keyword);
        }

    }
}
