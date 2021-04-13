using ESO_LangEditor.Core.Models;
using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.EventAggres;
using ESO_LangEditorGUI.Services.AccessServer;
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

namespace ESO_LangEditorGUI.ViewModels
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
            get { return _userList; }
            set { SetProperty(ref _userList, value); }
        }

        public ObservableCollection<ListBoxItem> RoleList
        {
            get { return _roleList; }
            set { SetProperty(ref _roleList, value); }
        }

        public string RoleName
        {
            get { return _roleName; }
            set { SetProperty(ref _roleName, value); }
        }

        public string ProgressSting
        {
            get { return _progressSting; }
            set { SetProperty(ref _progressSting, value); }
        }

        public UserInClientDto SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        public string NewUserInfo
        {
            get { return _newUserInfo; }
            set { SetProperty(ref _newUserInfo, value); }
        }

        public Guid NewUserId
        {
            get { return _newUserId; }
            set { SetProperty(ref _newUserId, value); }
        }

        public Guid UserGuid
        {
            get { return _UserGuid; }
            set { SetProperty(ref _UserGuid, value); }
        }

        public bool IsEnabledGuid
        {
            get { return _isEnabledGuid; }
            set { SetProperty(ref _isEnabledGuid, value); }
        }

        //public ICommand SelectedUser => new ExcuteViewModelMethod(GetRolesBySelectedUser);
        public ICommand GetUserListCommand => new ExcuteViewModelMethod(GetUserList);
        public ICommand UpdateUserRolesCommand => new ExcuteViewModelMethod(UpdateUserRoles);
        public ICommand AddUserRoleCommand => new ExcuteViewModelMethod(AddNewRole);
        public ICommand AddUserCommand => new ExcuteViewModelMethod(AddNewUser);
        public ICommand InitUserCommand => new ExcuteViewModelMethod(InitUser);

        IEventAggregator _ea;

        public UserRoleEditorViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _accountService = new AccountService(_ea);

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

            foreach(var user in _userList)
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

        private async void UpdateUserRoles(object obj)
        {
            List<string> selectedRoles = new List<string>();

            foreach(var role in RoleList)
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
    }

}
