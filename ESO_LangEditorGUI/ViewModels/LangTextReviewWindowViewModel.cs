using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.NetClient;
using ESO_LangEditorGUI.Command;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditorGUI.ViewModels
{
    public class LangTextReviewWindowViewModel : BindableBase
    {
        private string _searchResultInfo;
        private string _selectedInfo;
        private string _networkInfo;
        private ObservableCollection<LangTextForReviewDto> _gridData;
        private List<LangTextForReviewDto> _gridSelectedItems;
        private ObservableCollection<Dictionary<Guid, string>> _userList;
        private bool _isReviewSelectedItems;
        private LangtextNetService _langtextNetService = new LangtextNetService();

        public string SearchResultInfo
        {
            get { return _searchResultInfo; }
            set { SetProperty(ref _searchResultInfo, "查询到 " + value + " 条文本"); }
            //set { _searchInfo = "查询到 " + value + " 条文本"; ; NotifyPropertyChanged(); }
        }

        public string SelectedInfo
        {
            get { return _selectedInfo; }
            set { SetProperty(ref _selectedInfo, value); }
        }

        public string NetworkInfo
        {
            get { return _networkInfo; }
            set { SetProperty(ref _networkInfo, value); }
        }

        public ObservableCollection<LangTextForReviewDto> GridData
        {
            get { return _gridData; }
            set { SetProperty(ref _gridData, value); }
        }

        public List<LangTextForReviewDto> GridSelectedItems
        {
            get { return _gridSelectedItems; }
            set { SetProperty(ref _gridSelectedItems, value); }
        }

        public ObservableCollection<Dictionary<Guid, string>> UserList
        {
            get { return _userList; }
            set { SetProperty(ref _userList, value); }
        }

        public bool IsReviewSelectedItems
        {
            get { return _isReviewSelectedItems; }
            set { SetProperty(ref _isReviewSelectedItems, value); }
        }

        public ExcuteViewModelMethod QueryReviewPendingItems => new ExcuteViewModelMethod(QueryReviewItems);
        public ExcuteViewModelMethod SubmitApproveItems => new ExcuteViewModelMethod(SubmitApproveItemsToServer);
        public ExcuteViewModelMethod SubmitDenyItems => new ExcuteViewModelMethod(SubmitDenyItemsToServer);

        IEventAggregator _ea;
        

        public LangTextReviewWindowViewModel(IEventAggregator ea)
        {
            _ea = ea;
        }

        public async void QueryReviewItems(object o)
        {
            var token = App.LangConfig.UserAuthToken;
            GridData = null;
            NetworkInfo = "正在尝试读取……";

            try
            {
                var list = await _langtextNetService.GetLangtextInReviewAllAsync(token);
                GridData = new ObservableCollection<LangTextForReviewDto>(list);
                SearchResultInfo = GridData.Count.ToString();
                NetworkInfo = "读取完成";
            }
            catch(HttpRequestException ex)
            {
                NetworkInfo = ex.Message;
            }

        }

        public async void SubmitApproveItemsToServer(object o)
        {

        }

        public async void SubmitDenyItemsToServer(object o)
        {

        }


    }
}
