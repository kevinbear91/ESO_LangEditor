using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.NetClient;
using ESO_LangEditor.GUI;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.EventAggres;
using ESO_LangEditor.GUI.Services.AccessServer;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
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
        private AccountService _accountService;
        private UserInClientDto _selectedUser;
        private List<string> _roleListFromServer;
        private string _userName;
        private string _newUserInfo;
        private Guid _newUserId;
        private Guid _UserGuid;
        private bool _isEnabledGuid;
        private UserDto _userInfo;
        private bool _isOpenDialogs;
        private string _pwResetCode;
        private string _regCode;

        private IUserAccess _userService;

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

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
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

        public Guid UserGuid
        {
            get => _UserGuid;
            set => SetProperty(ref _UserGuid, value);
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

        public string PwResetCode
        {
            get => _pwResetCode;
            set => SetProperty(ref _pwResetCode, value);
        }

        public string RegCode
        {
            get => _regCode;
            set => SetProperty(ref _regCode, value);
        }

        //public ICommand SelectedUser => new ExcuteViewModelMethod(GetInfoBySelectedUser);
        public ICommand GetUserListCommand => new ExcuteViewModelMethod(GetUserList);
        public ICommand UpdateUserRolesCommand => new ExcuteViewModelMethod(UpdateUserRoles);
        public ICommand AddUserRoleCommand => new ExcuteViewModelMethod(AddNewRole);
        public ICommand AddUserCommand => new ExcuteViewModelMethod(AddNewUser);
        public ICommand InitUserCommand => new ExcuteViewModelMethod(InitUser);
        public ICommand UserPwResetCodeCommand => new ExcuteViewModelMethod(GetUserPwResetCode);
        public ICommand GetRegistrationCodeCommand => new ExcuteViewModelMethod(GetUserRegistrationCode);



        IEventAggregator _ea;

        public UserRoleEditorViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _accountService = new AccountService(_ea);
            _userService = new UserAccess(App.ServerPath);

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
            var userlist = await _accountService.GetUserList();

            UserList = new ObservableCollection<UserInClientDto>(userlist);

            foreach (var user in _userList)
            {
                Debug.WriteLine(user.Id);
            }

        }

        public async void GetRolesBySelectedUser(object obj)
        {
            _roleListFromServer = null;
            _roleListFromServer = await _accountService.GetUserRoleList(SelectedUser.Id);

            LoadListboxItem(_roleListFromServer);

        }

        public async void GetInfoBySelectedUser(object obj)
        {
            UserInfo = null;
            IsOpenDialogs = true;

            UserInfo = await _userService.GetUserInfoFromServer(SelectedUser.Id);

            //_roleListFromServer = await _accountService.GetUserRoleList(SelectedUser.Id);

            LoadListboxItem(UserInfo.UserRoles);

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

            if (await _accountService.ModifyUserRoles(SelectedUser.Id, selectedRoles))
            {
                ProgressSting = "提交请求已完成";
            }
            else
            {
                ProgressSting = "提交请求出错";
            }

        }

        private async void AddNewRole(object o)
        {
            if (!string.IsNullOrWhiteSpace(RoleName))
            {
                await _accountService.AddRole(RoleName);
            }

        }

        private async void AddNewUser(object obj)
        {
            Debug.WriteLine(UserName);
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var newUserDto = new UserDto
                {
                    ID = NewUserId,
                    UserName = UserName,
                };

                var userDto = await _accountService.AddUser(newUserDto);

                NewUserInfo = userDto.ID + "\n" + userDto.UserName + "\n" + userDto.RefreshToken;
            }

        }

        private async void InitUser(object obj)
        {
            Debug.WriteLine(UserGuid);
            if (!string.IsNullOrWhiteSpace(UserGuid.ToString()))
            {
                var userDto = await _accountService.InitUser(UserGuid);

                NewUserInfo = userDto.ID + "\n" + userDto.UserName + "\n" + userDto.RefreshToken;
            }

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

        private async void GetUserPwResetCode(object obj)
        {
            if (PwResetCode != null || PwResetCode != "")
            {
                PwResetCode = "";
            }

            if (SelectedUser != null)
            {
                IsOpenDialogs = true;

                PwResetCode = await _userService.GetUserPwResetCode(SelectedUser.Id);

                IsOpenDialogs = false;
            }

        }

        private async void GetUserRegistrationCode(object obj)
        {
            if (RegCode != null || RegCode != "")
            {
                RegCode = "";
            }

            IsOpenDialogs = true;

            RegCode = await _userService.GetRegistrationCode();

            IsOpenDialogs = false;
        }


    }

}
