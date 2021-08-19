using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.Services;
using ESO_LangEditor.GUI.Views;
using Microsoft.Extensions.Logging;
using Prism.Events;
using Prism.Mvvm;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ESO_LangEditor.GUI.ViewModels
{
    public class UserProfileSettingViewModel : BindableBase
    {
        private string _userName = App.LangConfig.UserName;
        private string _userGuid = "GUID：" + App.LangConfig.UserGuid.ToString();
        private string _nickName;
        private UserInClientDto _userDto;
        private bool _waitResult;

        private PasswordBox _passwordBox;
        private PasswordBox _passwordBoxConfirm;
        private PasswordBox _passwordCurrently;


        public ExcuteViewModelMethod ChangeUserInfoCommand => new ExcuteViewModelMethod(UserInfoChange);
        public ExcuteViewModelMethod ChangeUserPasswordCommand => new ExcuteViewModelMethod(UserPasswordChange);


        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string UserGuid => _userGuid;

        public string NickName
        {
            get => _nickName;
            set => SetProperty(ref _nickName, value);
        }

        public string UserTranslatedInt
        {
            get { return "翻译了 0" + /*_userDto.TranslatedCount +*/ " 条"; }
            //set { SetProperty(ref _userAvatarPath, value); }
        }

        public string UserInReviewInt
        {
            get { return "待通过 0" + /*_userDto.InReviewCount +*/ " 条"; }
            //set { SetProperty(ref _userAvatarPath, value); }
        }

        public string UserRemovedInt
        {
            get { return "被移除了 0" + /*_userDto.RemovedCount +*/ " 条"; }
            //set { SetProperty(ref _userAvatarPath, value); }
        }

        public UserInClientDto UserDto
        {
            get => _userDto;
            set => SetProperty(ref _userDto, value);
        }

        public bool WaitResult
        {
            get => _waitResult;
            set => SetProperty(ref _waitResult, value);
        }

        private IEventAggregator _ea;
        private IUserAccess _userAccess;
        private ILogger _logger;

        public UserProfileSettingViewModel(IEventAggregator ea, IUserAccess userAccess, ILogger logger)
        {
            _ea = ea;
            _userAccess = userAccess;
            _logger = logger;

            UserDto = App.User;
            NickName = UserDto.UserNickName;
        }

        public void Load(PasswordBox passwordBox, PasswordBox passwordBoxConfrim, PasswordBox passwordCurrently)
        {
            _passwordBox = passwordBox;
            _passwordBoxConfirm = passwordBoxConfrim;
            _passwordCurrently = passwordCurrently;
        }

        private async void UserInfoChange(object obj)
        {
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(NickName))
            {
                WaitResult = true;

                var respond = await _userAccess.SetUserInfoChange(new UserInfoChangeDto
                {
                    UserID = UserDto.Id,
                    UserName = UserName,
                    UserNickName = NickName,
                });

                MessageBox.Show(respond.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                ChangeUserInfoCommand.CanExecute(true);
            }

            WaitResult = false;
        }

        private async void UserPasswordChange(object obj)
        {
            var regex = new Regex(@"(?=.*[0-9])(?=.*[a-zA-Z])(?=([\x21-\x7e]+)[^a-zA-Z0-9]).{8,30}", RegexOptions.IgnorePatternWhitespace);

            if (_passwordBox.Password != _passwordBoxConfirm.Password)
            {
                MessageBox.Show("两次输入的密码不一致！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (regex.IsMatch(_passwordBox.Password))
                {
                    WaitResult = true;
                    var respond = await _userAccess.SetUserPasswordChange(new UserPasswordChangeDto
                    {
                        UserId = UserDto.Id,
                        OldPassword = _passwordCurrently.Password,
                        NewPassword = _passwordBox.Password,
                        NewPasswordConfirm = _passwordBoxConfirm.Password,
                    });

                    _passwordCurrently.Clear();
                    _passwordBox.Clear();
                    _passwordBoxConfirm.Clear();

                    if (respond.Code == (int)RespondCode.Success)
                    {
                        var config = App.LangConfig;
                        config.UserAuthToken = "";
                        config.UserRefreshToken = "";
                        AppConfigClient.Save(config);

                        MessageBox.Show(respond.Message + "，请退出工具用新密码重新登录！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                        //SubmitProgress = "修改成功，请退出工具用密码重新登录！";
                    }
                    else
                    {
                        MessageBox.Show(respond.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    _passwordBox.Clear();
                    _passwordBoxConfirm.Clear();
                    MessageBox.Show("密码必须大于8位小于30位，包含数字、英文字母与特殊符号！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            WaitResult = false;
        }

    }
}
