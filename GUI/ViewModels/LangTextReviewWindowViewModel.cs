using Core.Entities;
using Core.EnumTypes;
using Core.Models;
using GUI;
using GUI.EventAggres;
using GUI.Command;
using GUI.Services;
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

namespace GUI.ViewModels
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
        private ILangTextRepoClient _langTextRepoClient;
        private ILangTextAccess _langTextAccess;
        private string _currentLang;
        private bool _isSubmitItems = false;

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

        public string CurrentLang
        {
            get => _currentLang;
            set => SetProperty(ref _currentLang, value);
        }

        public LangTextForReviewDto GridSelectedItem
        {
            get => _gridSelectedItem;
            set => SetProperty(ref _gridSelectedItem, value);
        }

        public Visibility ReasonForVisibility => Visibility.Visible;
        public Visibility JpVisibility => Visibility.Collapsed;

        public ExcuteViewModelMethod QueryReviewPendingItems => new ExcuteViewModelMethod(QueryReviewUserList);
        public ExcuteViewModelMethod SubmitApproveItems => new ExcuteViewModelMethod(SubmitApproveItemsToServer);
        public ExcuteViewModelMethod SubmitDenyItems => new ExcuteViewModelMethod(SubmitDenyItemsToServer);

        public LangTextReviewWindowViewModel(IEventAggregator ea, ILangTextRepoClient langTextRepoClient,
            ILangTextAccess langTextAccess)
        {
            //_ea = ea;
            _langTextRepoClient = langTextRepoClient;
            _langTextAccess = langTextAccess;

            //_ea.GetEvent<LangtextReviewWindowDeleteSelectedItems>().Subscribe(SubmitDeleteSelectedItems);
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

        //public async void SubmitDeleteSelectedItemsToServer()
        //{
        //    if (_isSubmitItems)
        //    {
        //        MessageBox.Show("已有上传指令正在执行，请等待完成后再提交！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    else
        //    {
        //        _isSubmitItems = true;
        //        NetworkInfo = "正在上传并等待服务器执行……";

        //        List<Guid> langIdList;
        //        List<LangTextForReviewDto> langTextForReviewDtos = GridSelectedItems;

        //        if (langTextForReviewDtos.Count >= 1)
        //        {
        //            langIdList = langTextForReviewDtos.Select(lang => lang.Id).ToList();
        //            Debug.WriteLine("langIdList count: " + langIdList.Count);

        //            try
        //            {
        //                var respond = await _langTextAccess.RemoveLangTextsInReview(langIdList);

        //                if (respond.Code == (int)RespondCode.Success)
        //                {
        //                    foreach (var selected in langTextForReviewDtos)
        //                    {
        //                        GridData.Remove(selected);
        //                    }
        //                    NetworkInfo = "执行完成";
        //                }
        //            }
        //            catch (HttpRequestException ex)
        //            {
        //                NetworkInfo = ex.Message;
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("请选择要删除的文本！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        _isSubmitItems = false;
        //    }

        //}

        public async void SubmitDenyItemsToServer(object o)
        {
            if (_isSubmitItems)
            {
                MessageBox.Show("已有上传指令正在执行，请等待完成后再提交！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _isSubmitItems = true;
                NetworkInfo = "正在上传并等待服务器执行……";

                List<Guid> langIdList;
                List<LangTextForReviewDto> langTextForReviewDtos = GridSelectedItems;

                if (langTextForReviewDtos.Count >= 1)
                {
                    langIdList = langTextForReviewDtos.Select(lang => lang.Id).ToList();
                    Debug.WriteLine("langIdList count: " + langIdList.Count);

                    try
                    {
                        var respond = await _langTextAccess.RemoveLangTextsInReview(langIdList);

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
                else
                {
                    MessageBox.Show("请选择要删除的文本！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                _isSubmitItems = false;
            }
        }

        public async void SetSelectedItemInfo()
        {
            var localOldLang = await GetLangTextForReviewFromServer();
            CurrentLang = "读取本地修改前文本出错！";

            if (GridSelectedItem != null)
            {
                if (GridSelectedItem.ReasonFor == ReviewReason.ZhChanged
                    || GridSelectedItem.ReasonFor == ReviewReason.Deleted)
                {
                    CurrentLang = localOldLang.TextZh;
                }

                if (GridSelectedItem.ReasonFor == ReviewReason.EnChanged)
                {
                    CurrentLang = localOldLang.TextEn;
                }

                //SelectedItemInfo = "文本唯一ID：" + GridSelectedItem.TextId
                //+ "\n" + "文本类型：(" + GridSelectedItem.IdType + ")" + App.iDCatalog.GetCategory(GridSelectedItem.IdType)
                //+ "\n" + "英文：" + GridSelectedItem.TextEn
                //+ "\n" + "中文：" + GridSelectedItem.TextZh
                //+ "\n" + localOldTextZh;

            }
            else
            {
                CurrentLang = "无选择文本或当前文本不存在于本地数据库。";
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

        private async Task<LangTextForReviewDto> GetLangTextForReviewFromServer()
        {
            LangTextForReviewDto langTextForReviewDto = new LangTextForReviewDto();

            if (GridSelectedItem != null && GridSelectedItem.ReasonFor != ReviewReason.NewAdded)
            {
                langTextForReviewDto = await _langTextAccess.GetLangTextFromReview(GridSelectedItem.Id);
            }
            return langTextForReviewDto;
        }


    }
}
