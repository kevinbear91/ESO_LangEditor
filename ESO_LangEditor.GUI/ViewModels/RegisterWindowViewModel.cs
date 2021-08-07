using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.Services;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;

namespace ESO_LangEditor.GUI.ViewModels
{
    public class RegisterWindowViewModel : BindableBase
    {
        private string _userName;
        private string _userNickName;
        private string _registerCode;
        private PasswordBox _passwordBox;
        private PasswordBox _passwordBoxConfirm;

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

        private IUserAccess _userAccess;

        public RegisterWindowViewModel(IUserAccess userAccess)
        {
            _userAccess = userAccess;
        }

        public void Load(PasswordBox passwordBox, PasswordBox passwordBoxConfrim)
        {
            _passwordBox = passwordBox;
            _passwordBoxConfirm = passwordBoxConfrim;
        }

        public async void RegistrationUser()
        {
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

            MessageBox.Show(respond.ApiMessageCodeString());

        }
    }
}
