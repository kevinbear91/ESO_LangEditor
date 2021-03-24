using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.NetClient;
using ESO_LangEditorGUI.EventAggres;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.Views.UserControls;
using ESO_LangEditorGUI.ViewModels;
using Prism.Commands;
using Prism.Events;
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
        private readonly MainWindowSearchbarViewModel _mainView;
        private LangtextNetService apiLangtext = new LangtextNetService();

        private LangTextRepoClientService _langTextSearch;

        public DelegateCommand SendLangtextDto { get; private set; }
        IEventAggregator _ea;

        public SearchLangCommand(MainWindowSearchbarViewModel mainView, IEventAggregator ea)
        {
            //_mainView = mainView;

            _ea = ea;
            _langTextSearch = new LangTextRepoClientService();
        }

        public override async Task ExecuteAsync(object parameter)
        {
            //UC_LangDataGrid _langDatagrid = parameter as UC_LangDataGrid;

            MainWindowSearchbarViewModel _searchBarVM = parameter as MainWindowSearchbarViewModel;

            List<LangTextDto> result;

            if (App.OnlineMode)
            {
                result = apiLangtext.GetLangtextAsync(_mainView.Keyword, App.LangConfig.UserAuthToken).Result;

                //result.Add();

                result = await search.GetLangTexts(_mainView.SelectedSearchPostion, _mainView.SelectedSearchTextType, _mainView.Keyword);
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
