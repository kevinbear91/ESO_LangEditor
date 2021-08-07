using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.EventAggres;
using ESO_LangEditor.GUI.Services;
using ESO_LangEditor.GUI.ViewModels;
using Prism.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditor.GUI.Command
{
    public class SearchLangCommand : CommandBaseAsync
    {
        //private LangtextNetService apiLangtext = new LangtextNetService(App.ServerPath);
        private ILangTextRepoClient _langTextSearch;
        private IEventAggregator _ea;

        public SearchLangCommand(IEventAggregator ea, ILangTextRepoClient langTextRepoClient)
        {
            _ea = ea;
            _langTextSearch = langTextRepoClient;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            MainWindowSearchbarViewModel _searchBarVM = parameter as MainWindowSearchbarViewModel;
            List<LangTextDto> result;

            if (string.IsNullOrWhiteSpace(_searchBarVM.Keyword))
            {
                MessageBox.Show("不支持全局搜索，请输入关键字！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (_searchBarVM.DoubleKeyWordSearch)
                {
                    result = await _langTextSearch.GetLangTextByConditionAsync(_searchBarVM.Keyword, _searchBarVM.KeywordSecond,
                        _searchBarVM.SelectedSearchTextType, _searchBarVM.SelectedSearchTextTypeSecond,
                        _searchBarVM.SelectedSearchPostion);
                }
                else
                {
                    result = await _langTextSearch.GetLangTextByConditionAsync(_searchBarVM.Keyword,
                        _searchBarVM.SelectedSearchTextType,
                        _searchBarVM.SelectedSearchPostion);
                }

                //if (_searchBarVM.ServerSideSearch)
                //{
                //    result = apiLangtext.GetLangtextAsync(_searchBarVM.Keyword, App.LangConfig.UserAuthToken).Result;

                //    //result.Add();

                //    //result = await search.GetLangTexts(_searchBarVM.SelectedSearchPostion, _searchBarVM.SelectedSearchTextType, _searchBarVM.Keyword);
                //}
                //else
                //{
                //    if (_searchBarVM.DoubleKeyWordSearch)
                //    {
                //        result = await _langTextSearch.GetLangTextByConditionAsync(_searchBarVM.Keyword, _searchBarVM.KeywordSecond,
                //            _searchBarVM.SelectedSearchTextType, _searchBarVM.SelectedSearchTextTypeSecond,
                //            _searchBarVM.SelectedSearchPostion);
                //    }
                //    else
                //    {
                //        result = await _langTextSearch.GetLangTextByConditionAsync(_searchBarVM.Keyword,
                //            _searchBarVM.SelectedSearchTextType,
                //            _searchBarVM.SelectedSearchPostion);
                //    }

                //}
                _ea.GetEvent<LangtextPostToMainDataGrid>().Publish(result);

            }

        }
    }
}
