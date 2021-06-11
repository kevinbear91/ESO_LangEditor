using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.NetClient;
using ESO_LangEditor.GUI.EventAggres;
using ESO_LangEditor.GUI.Services;
using ESO_LangEditor.GUI.Views.UserControls;
using ESO_LangEditor.GUI.ViewModels;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ESO_LangEditor.GUI.Command
{
    public class SearchLangCommand : CommandBaseAsync
    {
        private readonly SearchLangText search = new SearchLangText();
        private LangtextNetService apiLangtext = new LangtextNetService(App.ServerPath);

        private LangTextRepoClientService _langTextSearch;

        public DelegateCommand SendLangtextDto { get; private set; }
        IEventAggregator _ea;

        public SearchLangCommand(IEventAggregator ea)
        {
            _ea = ea;
            _langTextSearch = new LangTextRepoClientService();
        }

        public override async Task ExecuteAsync(object parameter)
        {
            //UC_LangDataGrid _langDatagrid = parameter as UC_LangDataGrid;

            MainWindowSearchbarViewModel _searchBarVM = parameter as MainWindowSearchbarViewModel;

            List<LangTextDto> result;

            if(string.IsNullOrWhiteSpace(_searchBarVM.Keyword))
            {
                MessageBox.Show("不支持全局搜索，请输入关键字！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (_searchBarVM.ServerSideSearch)
                {
                    result = apiLangtext.GetLangtextAsync(_searchBarVM.Keyword, App.LangConfig.UserAuthToken).Result;

                    //result.Add();

                    //result = await search.GetLangTexts(_searchBarVM.SelectedSearchPostion, _searchBarVM.SelectedSearchTextType, _searchBarVM.Keyword);
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

                }
                SendLangText(result);
                
            }

            
            //_langDatagrid.LangDataGridDC.GridData = result;
            //_mainView.SearchInfo = result.Count.ToString();
        }

        private void SendLangText(List<LangTextDto> langTexts)
        {
            _ea.GetEvent<LangtextPostToMainDataGrid>().Publish(langTexts);
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
