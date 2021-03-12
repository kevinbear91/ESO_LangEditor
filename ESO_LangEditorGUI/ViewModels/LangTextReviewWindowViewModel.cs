using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.NetClient;
using ESO_LangEditorGUI.Command;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        private ObservableCollection<UserInClientDto> _userList;
        private UserInClientDto _selectedUser;
        private List<LangTextForReviewDto> _gridSelectedItems;
        private bool _isReviewSelectedItems = true;
        private LangtextNetService _langtextNetService = new LangtextNetService();

        public string SearchResultInfo
        {
            get { return _searchResultInfo; }
            set { SetProperty(ref _searchResultInfo, "查询到 " + value + " 条文本"); }
        }

        public string SelectedInfo
        {
            get { return _selectedInfo; }
            set { SetProperty(ref _selectedInfo, "已选择 " + value + " 条文本"); }
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

        public ObservableCollection<UserInClientDto> UserList
        {
            get { return _userList; }
            set { SetProperty(ref _userList, value); }
        }

        public UserInClientDto SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        public bool IsReviewSelectedItems
        {
            get { return _isReviewSelectedItems; }
            set { SetProperty(ref _isReviewSelectedItems, value); }
        }
        public ExcuteViewModelMethod QueryReviewPendingItems => new ExcuteViewModelMethod(QueryReviewUserList);
        public ExcuteViewModelMethod SubmitApproveItems => new ExcuteViewModelMethod(SubmitApproveItemsToServer);
        public ExcuteViewModelMethod SubmitDenyItems => new ExcuteViewModelMethod(SubmitDenyItemsToServer);

        IEventAggregator _ea;
        
        public LangTextReviewWindowViewModel(IEventAggregator ea)
        {
            _ea = ea;
        }

        public async void QueryReviewItemsBySelectedUser(object o)
        {
            var token = App.LangConfig.UserAuthToken;
            GridData = null;
            NetworkInfo = "正在尝试读取……";

            try
            {
                var list = await _langtextNetService.GetLangtextInReviewAsync(SelectedUser.Id.ToString(), token);

                if (list == null && list.Count == 0)
                {
                    NetworkInfo = "读取完成，无待审核条目";
                }
                else
                {
                    GridData = new ObservableCollection<LangTextForReviewDto>(list);
                    SearchResultInfo = GridData.Count.ToString();
                    NetworkInfo = "读取完成";
                }
            }
            catch(HttpRequestException ex)
            {
                NetworkInfo = ex.Message;
            }
        }

        public async void QueryReviewUserList(object o)
        {
            var token = App.LangConfig.UserAuthToken;
            UserList = null;
            NetworkInfo = "正在尝试读取……";
            var userDtoList = new List<UserInClientDto>();

            try
            {
                var list = await _langtextNetService.GetUsersInReviewAllAsync(token);
                
                foreach(var user in list)
                {
                    userDtoList.Add(new UserInClientDto{ Id = user, UserNickName = "Bevis"});
                }
                UserList = new ObservableCollection<UserInClientDto>(userDtoList);
                NetworkInfo = "读取完成";
            }
            catch (HttpRequestException ex)
            {
                NetworkInfo = ex.Message;
            }

        }

        public async void SubmitApproveItemsToServer(object o)
        {
            var token = App.LangConfig.UserAuthToken;
            NetworkInfo = "正在上传并等待服务器执行……";

            List<Guid> langIdList;
            List<LangTextForReviewDto> langTextForReviewDtos;

            if (IsReviewSelectedItems)
            {
                langTextForReviewDtos = GridSelectedItems;
            }
            else
            {
                langTextForReviewDtos = GridData.ToList();
            }

            langIdList = langTextForReviewDtos.Select(lang => lang.Id).ToList();
            Debug.WriteLine("langIdList count: " + langIdList.Count);

            try
            {
                var respondCode = await _langtextNetService.PutReviewListIdAsync(langIdList, token);

                if (respondCode == HttpStatusCode.OK)
                {
                    foreach(var selected in langTextForReviewDtos)
                    {
                        GridData.Remove(selected);
                    }
                    NetworkInfo = "执行完成";
                }
            }
            catch (HttpRequestException ex)
            {
                NetworkInfo = ex.Message;
            }
        }

        public async void SubmitDenyItemsToServer(object o)
        {

        }


    }
}
