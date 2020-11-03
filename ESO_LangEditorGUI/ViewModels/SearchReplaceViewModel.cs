using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.View.UserControls;
using ESO_LangEditorLib.Models.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class SearchReplaceViewModel : BaseViewModel
    {

        private string _searchWord;
        private string _replaceWord;
        private bool _onlyMatchword;
        private bool _ingoreCase;
        private List<LangTextDto> _langList;
        private List<LangTextDto> _currentSearchList;
        private ListSearchReplace _searchReplace;
        private DataGridViewModel _dataGridViewModel;
        private List<LangTextDto> _resultList;
        private List<LangTextDto> ReplacedList;

        public string SearchWord
        {
            get { return _searchWord; }
            set { _searchWord = value; NotifyPropertyChanged(); }
        }

        public string ReplaceWord
        {
            get { return _replaceWord; }
            set { _replaceWord = value; NotifyPropertyChanged(); }
        }

        public bool OnlyMatchWord
        {
            get { return _onlyMatchword; }
            set { _onlyMatchword = value; NotifyPropertyChanged(); }
        }

        public bool IngoreCase
        {
            get { return _ingoreCase; }
            set { _ingoreCase = value; NotifyPropertyChanged(); }
        }

        public List<LangTextDto> CurrentSearchList
        {
            get { return _currentSearchList; }
            set { _currentSearchList = value; NotifyPropertyChanged(); }
        }

        public ICommand SearchRealTimeCommand => new ExcuteViewModelMethod(SearchByTextblockChanged);

        public ICommand SaveSearchResultCommand => new ExcuteViewModelMethod(ReplaceTextList);

        public SearchReplaceViewModel(List<LangTextDto> langList, UC_LangDataGrid langDataGrid)
        {
            _langList = langList;
            _dataGridViewModel = langDataGrid.LangDataGridDC;
            _dataGridViewModel.GridData = langList;
            _searchReplace = new ListSearchReplace(langList);
        }

        private void SearchByTextblockChanged(object o)
        {
            _resultList = _searchReplace.SearchResult(SearchWord, OnlyMatchWord, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            _dataGridViewModel.GridData = _resultList;


            //Debug.WriteLine("SearchByTextblockChanged Respond.");
        }

        private void ReplaceTextList(object o)
        {
            if (_resultList.Count > 0)
            {
                ReplacedList = _searchReplace.SearchReplace(SearchWord, ReplaceWord, OnlyMatchWord, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                //int traslated = 1;
                //DateTime _zhTimestamp = DateTime.Now;
                //Guid _userID = App.LangConfig.UserGuid;

                //foreach (var r in _resultList)
                //{
                //    r.IsTranslated = 1;
                //    r.ZhLastModifyTimestamp = DateTime.Now;
                //    r.UserId = App.LangConfig.UserGuid;

                //    ReplacedList.Add(r);

                //    Debug.WriteLine("Text GUID: {0}, TextID: {1}, TextEN: {2}, TextZH: {3}, Translated: {4}, LastModifyTimeZH: {5}, UserGUID: {6}",
                //        r.Id, r.TextId, r.TextEn, r.TextZh, r.IsTranslated, r.ZhLastModifyTimestamp, r.UserId);
                //}
            }
        }


    }
}
