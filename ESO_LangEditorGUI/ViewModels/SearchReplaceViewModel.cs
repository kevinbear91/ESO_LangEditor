using ESO_LangEditor.Core.Models;
using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
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
            //_dataGridViewModel = langDataGrid.LangDataGridDC;
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
            //bool onlyMatchword = (bool)CheckBox_OnlyMatchWord.IsChecked;
            //bool isingoreCase = (bool)CheckBox_IgnoreCase.IsChecked;
            //List<LangTextDto> result;
            string keyWord;
            LangTextRepository _langTextRepository = new LangTextRepository();

            try
            {
                if (!string.IsNullOrEmpty(SearchWord) && !string.IsNullOrEmpty(ReplaceWord))
                {
                    if (SearchWord.Contains('?'))
                        keyWord = SearchWord.Replace("?", @"\?");
                    else
                        keyWord = SearchWord;

                    if (_resultList.Count > 0)
                    {
                        ReplacedList = _searchReplace.SearchReplace(SearchWord, ReplaceWord, OnlyMatchWord, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                        _langTextRepository.UpdateLangsZH(ReplacedList);

                        MessageBox.Show("替换完成！");

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

                    //if (isingoreCase)
                    //    result = searchReplace.SearchReplace(LangList, keyWord, replaceKeyWord.Text, onlyMatchword);
                    //else
                    //    result = searchReplace.SearchReplace(LangList, keyWord, replaceKeyWord.Text, onlyMatchword, RegexOptions.IgnoreCase);

                    //textWindow.ApplyReplacedList(result);

                    //this.Close();
                }
                else
                {
                    MessageBox.Show("查找内容与替换内容均不许为空，空格请谨慎匹配！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("替换时发生了错误，错误信息： " + Environment.NewLine + ex.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            
        }


    }
}
