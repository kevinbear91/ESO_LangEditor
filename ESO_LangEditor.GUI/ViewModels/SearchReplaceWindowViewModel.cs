using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.EventAggres;
using ESO_LangEditor.GUI.Services;
using NLog;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ESO_LangEditor.GUI.ViewModels
{
    public class SearchReplaceWindowViewModel : BindableBase
    {

        private string _searchWord;
        private string _replaceWord;
        private bool _onlyMatchword;
        private bool _ingoreCase;
        private bool _ingoreSearchFirst;
        private List<LangTextDto> _inputList;
        private List<LangTextDto> _currentSearchList;
        private List<LangTextDto> _resultList;
        private List<LangTextDto> ReplacedList;
        private ObservableCollection<LangTextDto> _gridData;

        public string SearchWord
        {
            get => _searchWord;
            set => SetProperty(ref _searchWord, value);
        }

        public string ReplaceWord
        {
            get => _replaceWord;
            set => SetProperty(ref _replaceWord, value);
        }

        public bool OnlyMatchWord
        {
            get => _onlyMatchword;
            set => SetProperty(ref _onlyMatchword, value);
        }

        public bool IngoreCase
        {
            get => _ingoreCase;
            set => SetProperty(ref _ingoreCase, value);
        }

        public bool IngoreSearchFirst
        {
            get => _ingoreSearchFirst;
            set => SetProperty(ref _ingoreSearchFirst, value);
        }

        public List<LangTextDto> CurrentSearchList
        {
            get => _currentSearchList;
            set => SetProperty(ref _currentSearchList, value);
        }

        public ObservableCollection<LangTextDto> GridData
        {
            get => _gridData;
            set => SetProperty(ref _gridData, value);
        }

        private ILangTextRepoClient _langTextRepository; // = new LangTextRepoClientService();
        private IEventAggregator _ea;
        private ILogger _logger;
        private IMapper _mapper;

        public ICommand GetMatchCommand => new ExcuteViewModelMethod(SearchIfMatch);
        public ICommand SaveSearchResultCommand => new ExcuteViewModelMethod(ReplaceTextListAsync);

        public SearchReplaceWindowViewModel(IEventAggregator ea, ILangTextRepoClient langTextRepoClient,
            ILogger logger, IMapper mapper)
        {
            _ea = ea;
            _langTextRepository = langTextRepoClient;
            _logger = logger;
            _mapper = mapper;

        }

        public void Load(List<LangTextDto> langTextDtos)
        {
            GridData = new ObservableCollection<LangTextDto>(langTextDtos);
            _inputList = langTextDtos;
        }

        private void SearchIfMatch(object o)
        {
            _resultList = SearchResult(SearchWord, OnlyMatchWord, RegexOptions.IgnoreCase);

            if (GridData != null)
                GridData = null;

            GridData = new ObservableCollection<LangTextDto>(_resultList);
        }

        private async void ReplaceTextListAsync(object o)
        {
            try
            {
                if (!string.IsNullOrEmpty(SearchWord) && !string.IsNullOrEmpty(ReplaceWord))
                {
                    if (!IngoreSearchFirst && _resultList != null && _resultList.Count > 0)
                    {
                        ReplacedList = SearchReplace(SearchWord, ReplaceWord, OnlyMatchWord, RegexOptions.IgnoreCase);
                        var replacedListToEnity = _mapper.Map<List<LangTextClient>>(ReplacedList);

                        if (await _langTextRepository.UpdateLangtexts(replacedListToEnity))
                        {
                            _logger.Debug("批量替换完成，共 " + replacedListToEnity.Count + " 条文本");

                            var langZhDto = _mapper.Map<List<LangTextForUpdateZhDto>>(ReplacedList);
                            _ea.GetEvent<UploadLangtextZhListUpdateEvent>().Publish(langZhDto);
                            MessageBox.Show("替换完成！");
                        }
                        else
                        {
                            MessageBox.Show("替换失败！");
                        }

                    }
                    else
                    {
                        MessageBox.Show("请先点击查找再保存，或者没有找到匹配项，请检查");
                    }

                }
                else
                {
                    MessageBox.Show("查找内容与替换内容均不许为空，空格请谨慎匹配！");
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal("替换时发生了错误，错误信息：" + ex.Message);
                MessageBox.Show("替换时发生了错误，错误信息： " + Environment.NewLine + ex.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private string SetMatchRule(string keyword, bool isOnlyMatchWord)
        {
            string pattern;

            Regex schar = new Regex(@"[\\.$^{\[(|)*+?]");
            keyword = schar.Replace(keyword, @"\$0");
            Debug.WriteLine(keyword);

            if (isOnlyMatchWord)
            {
                pattern = @"\b" + keyword + @"\b";
            }
            else
            {
                pattern = keyword;
            }
            return pattern;
        }

        #region 搜索匹配
        public List<LangTextDto> SearchResult(string keyword, bool isOnlyMatchWord, RegexOptions option)
        {
            var resultList = new List<LangTextDto>();

            string pattern = SetMatchRule(keyword, isOnlyMatchWord);
            //int count = 0;

            foreach (var text in _inputList)
            {
                if (Regex.IsMatch(text.TextZh, pattern, option))
                {
                    resultList.Add(text);
                }
            }

            return resultList;
        }
        #endregion


        public List<LangTextDto> SearchReplace(string keyword, string replaceWord, bool isOnlyMatchWord, RegexOptions option)
        #region 批量替换
        {
            string pattern = SetMatchRule(keyword, isOnlyMatchWord);
            var resultList = new List<LangTextDto>();

            Debug.WriteLine("isOnlyMatchWord = {0}", isOnlyMatchWord);

            foreach (var text in _resultList)
            {
                if (Regex.IsMatch(text.TextZh, pattern, option))
                {
                    string replacedWord = Regex.Replace(text.TextZh, pattern, replaceWord, option);

                    text.IsTranslated = 1;
                    text.ZhLastModifyTimestamp = DateTime.Now;
                    text.UserId = App.LangConfig.UserGuid;
                    text.TextZh = replacedWord;

                    resultList.Add(text);

                    //Debug.WriteLine("Text GUID: {0}, TextID: {1}, TextEN: {2}, TextZH: {3}, Translated: {4}, LastModifyTimeZH: {5}, UserGUID: {6}",
                    //    text.Id, text.TextId, text.TextEn, text.TextZh, text.IsTranslated, text.ZhLastModifyTimestamp, text.UserId);
                }
            }

            return resultList;
        }
        #endregion


    }
}
