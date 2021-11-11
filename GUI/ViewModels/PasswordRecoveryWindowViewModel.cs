using Core.EnumTypes;
using Core.Models;
using GUI.Command;
using GUI.Services;
using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace GUI.ViewModels
{
    public class PasswordRecoveryWindowViewModel : BindableBase
    {
        private string _userName;
        private PasswordBox _passwordBox;
        private PasswordBox _passwordBoxConfirm;
        private string _recoveryCode;
        private bool _waitResult;

        private IUserAccess _userAccess;
        private ILogger _logger;


        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string RecoveryCode
        {
            get => _recoveryCode;
            set => SetProperty(ref _recoveryCode, value);
        }

        public bool WaitResult
        {
            get => _waitResult;
            set => SetProperty(ref _waitResult, value);
        }

        public ExcuteViewModelMethod UserPasswordResetCommand => new ExcuteViewModelMethod(UserPasswordReset);

        public PasswordRecoveryWindowViewModel(IUserAccess userAccess, ILogger logger)
        {
            _userAccess = userAccess;
            _logger = logger;
        }

        public void Load(PasswordBox passwordBox, PasswordBox passwordBoxConfrim)
        {
            _passwordBox = passwordBox;
            _passwordBoxConfirm = passwordBoxConfrim;
        }

        private async void UserPasswordReset(object obj)
        {
            var regex = new Regex(@"(?=.*[0-9])(?=.*[a-zA-Z])(?=([\x21-\x7e]+)[^a-zA-Z0-9]).{8,30}", RegexOptions.IgnorePatternWhitespace);

            if (_passwordBox.Password != _passwordBoxConfirm.Password)
            {
                MessageBox.Show("两次输入的密码不一致！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (regex.IsMatch(_passwordBoxConfirm.Password))
                {
                    WaitResult = true;
                    var respond = await _userAccess.SetUserPasswordByRecoveryCode(new UserPasswordRecoveryDto
                    {
                        UserName = UserName,
                        NewPassword = _passwordBox.Password,
                        NewPasswordConfirm = _passwordBoxConfirm.Password,
                        RecoveryCode = RecoveryCode,
                    });

                    if (respond.Code == (int)RespondCode.Success)
                    {
                        MessageBox.Show("修改成功，请使用新密码登录！", respond.Code.ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(respond.Message, respond.Code.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    _passwordBox.Clear();
                    _passwordBoxConfirm.Clear();
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
