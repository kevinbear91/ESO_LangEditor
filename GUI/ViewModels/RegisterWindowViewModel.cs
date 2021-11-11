using Core.EnumTypes;
using Core.Models;
using GUI.Command;
using GUI.Services;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace GUI.ViewModels
{
    public class RegisterWindowViewModel : BindableBase
    {
        private string _userName;
        private string _userNickName;
        private string _registerCode;
        private PasswordBox _passwordBox;
        private PasswordBox _passwordBoxConfirm;
        private bool _waitResult;

        [Required, StringLength(maximumLength: 15, ErrorMessage = "最低5位最高15位", MinimumLength = 5)]
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        [Required, MaxLength(30, ErrorMessage = "最高30位")]
        public string UserNickName
        {
            get => _userNickName;
            set => SetProperty(ref _userNickName, value);
        }

        [StringLength(maximumLength: 20, ErrorMessage = "不得低于15个字符。", MinimumLength = 8)]
        public string RegisterCode
        {
            get => _registerCode;
            set => SetProperty(ref _registerCode, value);
        }

        public bool WaitResult
        {
            get => _waitResult;
            set => SetProperty(ref _waitResult, value);
        }

        private IUserAccess _userAccess;
        public ExcuteViewModelMethod RegisterCommand => new ExcuteViewModelMethod(RegistrationUser);

        public RegisterWindowViewModel(IUserAccess userAccess)
        {
            _userAccess = userAccess;
        }

        public void Load(PasswordBox passwordBox, PasswordBox passwordBoxConfrim)
        {
            _passwordBox = passwordBox;
            _passwordBoxConfirm = passwordBoxConfrim;
        }

        public async void RegistrationUser(object o)
        {
            var regex = new Regex(@"(?=.*[0-9])(?=.*[a-zA-Z])(?=([\x21-\x7e]+)[^a-zA-Z0-9]).{8,30}", RegexOptions.IgnorePatternWhitespace);

            if (string.IsNullOrEmpty(_passwordBox.Password) || string.IsNullOrEmpty(_passwordBoxConfirm.Password))
            {
                MessageBox.Show("请输入密码！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (_passwordBox.Password != _passwordBoxConfirm.Password)
            {
                _passwordBox.Clear();
                _passwordBoxConfirm.Clear();

                MessageBox.Show("两次输入的密码不一致！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!regex.IsMatch(_passwordBoxConfirm.Password))
            {
                _passwordBox.Clear();
                _passwordBoxConfirm.Clear();
                MessageBox.Show("密码必须大于8位小于30位，包含数字、英文字母与特殊符号！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                WaitResult = true;

                var respond = await _userAccess.AddNewUser(new RegistrationUserDto
                {
                    UserName = UserName,
                    UserNickName = UserNickName,
                    Password = _passwordBox.Password,
                    ConfirmPassword = _passwordBoxConfirm.Password,
                    RegisterCode = RegisterCode,
                });
                _passwordBox.Clear();
                _passwordBoxConfirm.Clear();

                if (respond.Code == (int)RespondCode.Success)
                {
                    MessageBox.Show(respond.Message, respond.Code.ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(respond.Message, respond.Code.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }

                WaitResult = false;
            }




        }


    }
}
