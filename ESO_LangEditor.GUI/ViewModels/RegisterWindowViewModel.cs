using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
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
            await _userAccess.AddNewUser(new RegistrationUserDto
            {
                UserName = UserName,
                UserNickName = UserNickName,
                Password = _passwordBox.Password,
                ConfirmPassword = _passwordBoxConfirm.Password,
                RegisterCode = RegisterCode,
            });

            //TO DO

        }




    }
}
