using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.NetClient.Old;
using ESO_LangEditor.GUI.Services;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditor.GUI.ViewModels
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
        private LangTextForReviewDto _gridSelectedItem;
        private bool _isReviewSelectedItems = true;
        private ILangTextRepoClient _langTextRepoClient; // = new LangTextRepoClientService();
        private ILangTextAccess _langTextAccess;
        private string _selectedItemInfo;

        public string SearchResultInfo
        {
            get => _searchResultInfo;
            set { SetProperty(ref _searchResultInfo, "查询到 " + value + " 条文本"); }
        }

        public string SelectedInfo
        {
            get => _selectedInfo;
            set { SetProperty(ref _selectedInfo, "已选择 " + value + " 条文本"); }
        }

        public string NetworkInfo
        {
            get => _networkInfo;
            set => SetProperty(ref _networkInfo, value);
        }

        public ObservableCollection<LangTextForReviewDto> GridData
        {
            get => _gridData;
            set => SetProperty(ref _gridData, value);
        }

        public List<LangTextForReviewDto> GridSelectedItems
        {
            get => _gridSelectedItems;
            set => SetProperty(ref _gridSelectedItems, value);
        }

        public ObservableCollection<UserInClientDto> UserList
        {
            get => _userList;
            set => SetProperty(ref _userList, value);
        }

        public UserInClientDto SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        public bool IsReviewSelectedItems
        {
            get => _isReviewSelectedItems;
            set => SetProperty(ref _isReviewSelectedItems, value);
        }

        public string SelectedItemInfo
        {
            get => _selectedItemInfo;
            set => SetProperty(ref _selectedItemInfo, value);
        }

        public LangTextForReviewDto GridSelectedItem
        {
            get => _gridSelectedItem;
            set => SetProperty(ref _gridSelectedItem, value);
        }

        public Visibility ReasonForVisibility => Visibility.Visible;

        public ExcuteViewModelMethod QueryReviewPendingItems => new ExcuteViewModelMethod(QueryReviewUserList);
        public ExcuteViewModelMethod SubmitApproveItems => new ExcuteViewModelMethod(SubmitApproveItemsToServer);
        public ExcuteViewModelMethod SubmitDenyItems => new ExcuteViewModelMethod(SubmitDenyItemsToServer);

        private IEventAggregator _ea;

        public LangTextReviewWindowViewModel(IEventAggregator ea, ILangTextRepoClient langTextRepoClient, 
            ILangTextAccess langTextAccess)
        {
            _ea = ea;
            _langTextRepoClient = langTextRepoClient;
            _langTextAccess = langTextAccess;
        }

        public async void QueryReviewItemsBySelectedUser(object o)
        {
            GridData = null;
            NetworkInfo = "正在尝试读取……";

            try
            {
                var list = await _langTextAccess.GetLangTextsFromReviewer(SelectedUser.Id);
                

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
            catch (HttpRequestException ex)
            {
                NetworkInfo = ex.Message;
            }
        }

        public async void QueryReviewUserList(object o)
        {
            UserList = null;
            NetworkInfo = "正在尝试读取……";
            var userDtoList = new List<UserInClientDto>();
            var users = await _langTextRepoClient.GetUsers();
            Dictionary<Guid, UserInClient> userDict = users.ToDictionary(u => u.Id);

            try
            {
                var list = await _langTextAccess.GetUsersInReview();

                foreach (var user in list)
                {
                    userDtoList.Add(new UserInClientDto { Id = user, UserNickName = userDict[user].UserNickName });
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
                var respond = await _langTextAccess.ApproveLangTextsInReview(langIdList);

                if (respond.Code == (int)RespondCode.Success)
                {
                    foreach (var selected in langTextForReviewDtos)
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

        public async void SetSelectedItemInfo()
        {
            var localOldLang = await FindLangtext();
            string localOldTextZh = "读取本地修改前文本出错！";

            if (GridSelectedItem != null)
            {
                if (GridSelectedItem.ReasonFor == ReviewReason.ZhChanged
                    || GridSelectedItem.ReasonFor == ReviewReason.Deleted)
                {
                    localOldTextZh = "修改前中文(本地)：" + localOldLang.TextZh;
                }

                if (GridSelectedItem.ReasonFor == ReviewReason.EnChanged)
                {
                    localOldTextZh = "修改前英文(本地)：" + localOldLang.TextEn;
                }

                SelectedItemInfo = "文本唯一ID：" + GridSelectedItem.TextId
                + "\n" + "文本类型：(" + GridSelectedItem.IdType + ")" + App.iDCatalog.GetCategory(GridSelectedItem.IdType)
                + "\n" + "英文：" + GridSelectedItem.TextEn
                + "\n" + "中文：" + GridSelectedItem.TextZh
                + "\n" + localOldTextZh;

            }
            else
            {
                SelectedItemInfo = "无选择文本或当前文本不存在于本地数据库。";
            }


        }

        public async Task<LangTextClient> FindLangtext()
        {
            LangTextClient lang = new LangTextClient();

            if (GridSelectedItem != null && GridSelectedItem.ReasonFor != ReviewReason.NewAdded)
            {
                lang = await _langTextRepoClient.GetLangTextByGuidAsync(GridSelectedItem.Id);
            }
            return lang;
        }


    }
}
