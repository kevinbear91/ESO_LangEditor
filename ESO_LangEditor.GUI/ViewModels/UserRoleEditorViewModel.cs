using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.EventAggres;
using ESO_LangEditor.GUI.Services;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ESO_LangEditor.GUI.ViewModels
{
    public class UserRoleEditorViewModel : BindableBase
    {
        private ObservableCollection<UserInClientDto> _userList;
        private ObservableCollection<ListBoxItem> _roleList;
        private string _roleName;
        private string _progressSting;
        private UserInClientDto _selectedUser;
        private List<string> _roleListFromServer;
        private UserDto _userInfo;
        private Guid _UserGuid;
        private string _userName;
        private string _userNickName;
        private bool _userLockoutEnabled;
        private DateTime? _userLockoutEnd;
        private string _userTranslatedNumber;
        private string _newUserInfo;
        private Guid _newUserId;
        private bool _isEnabledGuid;
        private bool _isOpenDialogs;
        private string _recoveryCode;
        private string _registrationCode;


        private List<string> _roles = new List<string>
        {
            "InitUser",
            "Editor",
            "Reviewer",
            "Admin",
            "Creater",
        };

        public ObservableCollection<UserInClientDto> UserList
        {
            get => _userList;
            set => SetProperty(ref _userList, value);
        }

        public ObservableCollection<ListBoxItem> RoleList
        {
            get => _roleList;
            set => SetProperty(ref _roleList, value);
        }

        public string RoleName
        {
            get => _roleName;
            set => SetProperty(ref _roleName, value);
        }

        public string ProgressSting
        {
            get => _progressSting;
            set => SetProperty(ref _progressSting, value);
        }

        public UserInClientDto SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        public Guid UserGuid
        {
            get => _UserGuid;
            set => SetProperty(ref _UserGuid, value);
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string UserNickName
        {
            get => _userNickName;
            set => SetProperty(ref _userNickName, value);
        }

        public bool UserLockoutEnabled
        {
            get => _userLockoutEnabled;
            set => SetProperty(ref _userLockoutEnabled, value);
        }

        public DateTime? UserLockoutEnd
        {
            get => _userLockoutEnd;
            set => SetProperty(ref _userLockoutEnd, value);
        }

        public string UserTranslatedNumebr
        {
            get => "已翻译：" + _userTranslatedNumber + " 条文本";
            set => SetProperty(ref _userTranslatedNumber, value);
        }

        public string NewUserInfo
        {
            get => _newUserInfo;
            set => SetProperty(ref _newUserInfo, value);
        }

        public Guid NewUserId
        {
            get => _newUserId;
            set => SetProperty(ref _newUserId, value);
        }

        public UserDto UserInfo
        {
            get => _userInfo;
            set => SetProperty(ref _userInfo, value);
        }

        public bool IsEnabledGuid
        {
            get => _isEnabledGuid;
            set => SetProperty(ref _isEnabledGuid, value);
        }

        public bool IsOpenDialogs
        {
            get => _isOpenDialogs;
            set => SetProperty(ref _isOpenDialogs, value);
        }

        public string RecoveryCode
        {
            get => _recoveryCode;
            set => SetProperty(ref _recoveryCode, value);
        }

        public string RegistrationCode
        {
            get => _registrationCode;
            set => SetProperty(ref _registrationCode, value);
        }

        //public ICommand SelectedUser => new ExcuteViewModelMethod(GetInfoBySelectedUser);
        public ICommand GetUserListCommand => new ExcuteViewModelMethod(GetUserList);
        public ICommand UpdateUserRolesCommand => new ExcuteViewModelMethod(UpdateUserRoles);
        public ICommand AddUserRoleCommand => new ExcuteViewModelMethod(AddNewRole);
        public ICommand UserPwRecoveryCodeCommand => new ExcuteViewModelMethod(GetUserPwRecoveryCode);
        public ICommand GetRegistrationCodeCommand => new ExcuteViewModelMethod(GetUserRegistrationCode);
        public ICommand SetUserInfoCommand => new ExcuteViewModelMethod(SetUserInfo);
        public ICommand SetUserPasswordToRandomCommand => new ExcuteViewModelMethod(SetUserPasswordToRandom);
        //public ICommand SetUserLockoutTimerCommand => new ExcuteViewModelMethod(SetUserLockoutTimer);

        private IEventAggregator _ea;
        private IUserAccess _userService;

        public UserRoleEditorViewModel(IEventAggregator ea, IUserAccess userAccess)
        {
            _ea = ea;
            _userService = userAccess;

            _ea.GetEvent<ConnectProgressString>().Subscribe(UpdateProgressSting);

            LoadListboxItem(null);

            GetUserListCommand.Execute(null);
        }

        private void UpdateProgressSting(string obj)
        {
            ProgressSting = obj;
        }

        private async void GetUserList(object obj)
        {
            //var userlist = await _userService.GetUserList();

            //UserList = new ObservableCollection<UserInClientDto>(userlist);

            ////TO DO

            //foreach (var user in _userList)
            //{
            //    Debug.WriteLine(user.Id);
            //}

        }

        public async void GetRolesBySelectedUser(object obj)
        {
            _roleListFromServer = null;
            _roleListFromServer = await _userService.GetUserRoles(SelectedUser.Id);

            LoadListboxItem(_roleListFromServer);

        }

        public async void GetInfoBySelectedUser(object obj)
        {
            UserInfo = null;
            IsOpenDialogs = true;

            UserInfo = await _userService.GetUserInfoFromServer(SelectedUser.Id);

            if (UserInfo != null)
            {
                var lockouttimer = new DateTime();
                lockouttimer.ToUniversalTime();

                UserGuid = UserInfo.ID;
                UserName = UserInfo.UserName;
                UserNickName = UserInfo.UserNickName;
                UserTranslatedNumebr = UserInfo.TranslatedCount.ToString();
                UserLockoutEnabled = UserInfo.LockoutEnabled;

                LoadListboxItem(UserInfo.UserRoles);
            }

            IsOpenDialogs = false;

        }

        private async void UpdateUserRoles(object obj)
        {
            List<string> selectedRoles = new List<string>();

            foreach (var role in RoleList)
            {
                if (role.IsSelected /*& !_roleListFromServer.Contains(role.Content.ToString())*/)
                {
                    selectedRoles.Add(role.Content.ToString());
                    Debug.WriteLine(role.Content);
                }
            }

            var respond = await _userService.SetUserRoles(SelectedUser.Id, selectedRoles);

            MessageBox.Show(respond.ApiMessageCodeString());

        }

        private async void AddNewRole(object o)
        {
            if (!string.IsNullOrWhiteSpace(RoleName))
            {
                await _userService.AddNewRole(RoleName);
            }

            //TO DO

        }

        private void LoadListboxItem(List<string> roles)
        {
            RoleList = null;
            RoleList = new ObservableCollection<ListBoxItem>();

            if (roles == null || roles.Count == 0)
            {
                foreach (var role in _roles)
                {
                    RoleList.Add(new ListBoxItem { Content = role, IsSelected = false });
                }
            }
            else
            {
                foreach (var role in _roles)
                {
                    bool isSelected = false;
                    if (roles.Contains(role))
                    {
                        isSelected = true;
                    }

                    RoleList.Add(new ListBoxItem { Content = role, IsSelected = isSelected });
                }
            }
        }

        private async void GetUserPwRecoveryCode(object obj)
        {
            if (RecoveryCode != null || RecoveryCode != "")
            {
                RecoveryCode = "";
            }

            if (SelectedUser != null)
            {
                IsOpenDialogs = true;

                RecoveryCode = await _userService.GetUserPasswordRecoveryCode(SelectedUser.Id);

                IsOpenDialogs = false;
            }

        }

        private async void GetUserRegistrationCode(object obj)
        {
            if (RegistrationCode != null || RegistrationCode != "")
            {
                RegistrationCode = "";
            }

            IsOpenDialogs = true;

            RegistrationCode = await _userService.GetRegistrationCode();

            IsOpenDialogs = false;
        }

        private async void SetUserInfo(object obj)
        {
            if (SelectedUser != null)
            {
                var respond = await _userService.SetUserInfo(new SetUserInfoDto
                {
                    UserID = UserGuid,
                    UserName = UserName,
                    UserNickName = UserNickName,
                    LockoutEnabled = UserLockoutEnabled,
                    LockoutEnd = UserLockoutEnd,
                });

                MessageBox.Show(respond.ApiMessageCodeString());
            }
        }

        private async void SetUserPasswordToRandom(object obj)
        {
            if (SelectedUser != null)
            {
                var respond = await _userService.SetUserPasswordToRandom(SelectedUser.Id);

                MessageBox.Show(respond.ApiMessageCodeString());

            }

        }

        //private void SetUserLockoutTimer(object obj)
        //{
        //    Debug.WriteLine(UserLockoutEnabled);

        //    if (UserLockoutEnabled)
        //    {
        //        UserLockoutEnd = DateTime.MaxValue;
        //    }
        //    else
        //    {
        //        UserLockoutEnd = DateTime.UtcNow;
        //    }
        //}

    }

}
